using System.Text.RegularExpressions;

using SimpleCalculator.Component.Interface;
using SimpleCalculator.Extension;
using SimpleCalculator.Model;

using SimpleWpf.Utilities;

namespace SimpleCalculator.Component
{
    public class ExpressionParser : IExpressionParser
    {
        private readonly CalculatorConfiguration _configuration;
        private readonly ICalculatorLogger _calculatorLogger;

        public ExpressionParser(CalculatorConfiguration configuration, ICalculatorLogger calculatorLogger)
        {
            _configuration = configuration;
            _calculatorLogger = calculatorLogger;
        }

        public MathExpression? Parse(string statement, out string? message)
        {
            message = null;

            // Validation
            if (string.IsNullOrWhiteSpace(statement))
            {
                message = "Math statement may not be empty";
                return null;
            }

            // Procedure: This process mimics the semantic tree; but it does not evaluate the expression (node). It just
            //            prepares them and knows whether they're evaluatable.
            //
            // 0) Determine type of expression: Assignment, or Arithmetic, or defined symbol (can be directly evaluated)
            // 1) Locate function signatures, and paren'd substatements. (Functions are essentially just symbols)
            // 2) Taking outermost paren'd statement(s), determine sub-expressions for this recursion iteration, which 
            //    involves splitting by operators. Store these in order of operation.
            // 3) Recurse until all statements are accounted for. If there are expressions that have undefined symbols, 
            //    then you store those details with the MathExpression(s)
            //

            // CHECK ENTIRE STATEMENT BRACKETING
            statement = statement.RemoveAllOuterParens();

            // Number of assignment operators
            int assignmentOperatorCount = 0;
            StringHelpers.RegexMatchIC(_configuration.SymbolTable.GetAssignmentOperator().Symbol, statement, out assignmentOperatorCount);

            if (assignmentOperatorCount > 1)
            {
                message = "Math statement may only have one assignment operator";
                return null;
            }

            // Assignment / Arithmetic (FunctionSignature = MathExpression)
            if (assignmentOperatorCount == 1)
            {
                return ParseAssignment(statement, out message);
            }

            // Arithmetic: Sub-Expressions (Recurse), store results and return
            else
            {
                // Get outermost paren'd sub-statements
                var parenthesisData = ExpressionCalculator.GetOutermostParens(statement);

                // Operator Index
                Operator? nextOperator = null;

                // Orders child expressions by parens and order of operations
                var operatorIndex = ExpressionCalculator.LocateNextOperator(_configuration, statement, parenthesisData.Outermost, out nextOperator);

                // Value Expression: (no operator present)
                if (nextOperator == null)
                {
                    // Operand Node (Left)
                    return ParseValueExpression(statement, out message);
                }

                // Next Operator (Arithmetic Statement)
                else
                {
                    // Parse these as sub-statements (RECURSE)
                    //
                    var leftStatement = statement.Substring(0, operatorIndex);
                    var rightStatement = statement.Substring(operatorIndex + 1, statement.Length - operatorIndex - 1);

                    // -> ParseImpl (RECURSE)
                    var leftExpression = Parse(leftStatement, out message);
                    var rightExpression = Parse(rightStatement, out message);

                    // Success!
                    if (leftExpression != null && rightExpression != null)
                        return new MathExpression(nextOperator, leftExpression, rightExpression);

                    else
                        return null;
                }
            }
        }

        // Parses statement as an assignment statement. 
        private MathExpression? ParseAssignment(string statement, out string? message)
        {
            message = null;

            // LHS = RHS
            var statementSplit = statement.Split(_configuration.SymbolTable.GetAssignmentOperator().Symbol,
                                                 StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            // Validation
            if (statementSplit.Length != 2 ||
                string.IsNullOrWhiteSpace(statementSplit[0]) ||
                string.IsNullOrWhiteSpace(statementSplit[1]))
            {
                message = "Improperly formed statement: " + statement;
                return null;
            }

            // Symbol Assignment
            if (_configuration.SymbolTable.IsDefined(statementSplit[0]))
            {
                switch (_configuration.SymbolTable.GetSymbolType(statementSplit[0]))
                {
                    case SymbolType.Constant:
                    {
                        // Constant
                        var constant = _configuration.SymbolTable.Get(statementSplit[0]) as Constant;

                        // Recurse
                        var constantExpression = Parse(statementSplit[1], out message);

                        if (constantExpression == null)
                            return null;

                        else if (constantExpression.Type != MathExpressionType.Number)
                        {
                            message = "Can only assign numeric values as constants";
                            return null;
                        }
                        else
                            return new MathExpression(constant, constantExpression);
                    }

                    case SymbolType.Variable:
                    {
                        // Variable
                        var variable = _configuration.SymbolTable.Get(statementSplit[0]) as Variable;

                        // Recurse
                        var variableExpression = Parse(statementSplit[1], out message);

                        if (variableExpression == null)
                            return null;

                        else if (variableExpression.Type != MathExpressionType.Number)
                        {
                            message = "Can only assign numeric values as constants";
                            return null;
                        }

                        else
                            return new MathExpression(variable, variableExpression);
                    }

                    case SymbolType.Function:       // Let this be done below
                        break;
                    case SymbolType.Operator:
                    {
                        message = "Cannot redefine operator:  " + statementSplit[0];
                        return null;
                    }
                    default:
                        throw new Exception("Unhandled Symbol Type");
                }
            }

            // Function Signature Assignment
            var signature = ParseFunctionSignature(statementSplit[0]);

            if (signature == null)
            {
                message = "Improperly formed function signature:  " + statementSplit[0];
                return null;
            }

            // Recurse
            var bodyMessage = string.Empty;
            var bodyExpression = Parse(statementSplit[1], out bodyMessage);

            // Function (as assignment expression)
            if (bodyExpression != null)
            {
                // Function (Symbol) will be assigned with the body expression also, which 
                // will be stored in the symbol table on evaluation.
                var function = new Function(signature, bodyExpression);

                return new MathExpression(function, bodyExpression);
            }

            // Assignment Error
            else
            {
                message = "Improper function body expression:  " + statementSplit[1];
                return null;
            }
        }

        private FunctionSignature? ParseFunctionSignature(string signature)
        {
            // Matches:  f(x)  (1 time),  f(x  (0 times)
            //           f(xy) (1 time)
            //           f(x)g(y) (2 times)
            //           fine(xy) (1 time)
            //           fine(x,y) (1 time)
            //           fine(x,y,) (1 time)
            //
            var functionRegex = "[a-zA-Z]+\\([a-zA-Z|,]+\\)";

            var match = Regex.Match(signature, functionRegex);

            if (match.Success && match.Captures.Count == 1)
            {
                var symbol = new SubstringLocator(signature, CalculatorConfiguration.LeftParenthesis, false);

                var variables = signature.Split(new string[] {
                    symbol.GetSubString(),
                    CalculatorConfiguration.LeftParenthesis.ToString(),
                    CalculatorConfiguration.RightParenthesis.ToString(),
                    CalculatorConfiguration.FunctionVariableSeparator.ToString() },
                    StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                if (variables.Length == 0)
                    return null;

                // NEW VARIABLE(S)
                var independentVariables = variables.Select(x => new Variable(x)).ToArray();

                foreach (var variable in independentVariables)
                {
                    // CHECK RAW STRINGS - NOT JUST VARIABLES (could've been a constant or function)
                    if (!_configuration.SymbolTable.IsDefined(variable.Symbol))
                    {
                        _calculatorLogger.Log("Defining Variable:  " + variable.ToString(), false);
                        _configuration.SymbolTable.Add(variable, 0);
                    }
                }

                return new FunctionSignature(new Variable(symbol.GetSubString()), independentVariables);
            }
            else
                return null;
        }

        private MathExpression? ParseValueExpression(string expression, out string? message)
        {
            // Procedure: If the expression is a number, then directly parse the number. Otherwise,
            //            it will be considered as a constant unless it is defined in the symbol 
            //            table.
            // 

            message = null;

            // Value Expression:  Number
            double numericValue = 0;
            if (double.TryParse(expression, out numericValue))
            {
                return new MathExpression(numericValue);
            }

            // Value Expression: Constant
            foreach (var constant in _configuration.SymbolTable.Constants)
            {
                if (constant.Symbol == expression)
                {
                    return new MathExpression(constant);
                }
            }

            // Value Expression: Variable
            foreach (var variable in _configuration.SymbolTable.Variables)
            {
                if (variable.Symbol == expression)
                {
                    return new MathExpression(variable);
                }
            }

            // Value Expression: Function (THESE MUST BE PRE-VALIDATED!)
            foreach (var function in _configuration.SymbolTable.Functions)
            {
                if (function.Symbol == expression)
                {
                    return new MathExpression(function);
                }
            }

            // NEW SYMBOL (CONSTANT)
            _calculatorLogger.Log("Defining Symbol:  " + expression, false);

            var newConstant = new Constant(expression);

            _configuration.SymbolTable.Add(newConstant, 0);

            return new MathExpression(newConstant);
        }
    }
}
