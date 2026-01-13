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

        private const string NON_ALPHA_CHARACTER = "[^a-zA-Z]";

        public ExpressionParser(CalculatorConfiguration configuration, ICalculatorLogger calculatorLogger)
        {
            _configuration = configuration;
            _calculatorLogger = calculatorLogger;
        }

        public MathExpression? Parse(string statement)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(statement))
            {
                _calculatorLogger.Log("Math statement may not be empty", CalculatorLogType.ParseError);
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
                _calculatorLogger.Log("Math statement may only have one assignment operator", CalculatorLogType.ParseError);
                return null;
            }

            // Assignment / Arithmetic (FunctionSignature = MathExpression)
            if (assignmentOperatorCount == 1)
            {
                return ParseAssignment(statement);
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
                    return ParseValueExpression(statement);
                }

                // Next Operator (Arithmetic Statement)
                else
                {
                    // Parse these as sub-statements (RECURSE)
                    //
                    var leftStatement = statement.Substring(0, operatorIndex);
                    var rightStatement = statement.Substring(operatorIndex + 1, statement.Length - operatorIndex - 1);

                    // -> ParseImpl (RECURSE)
                    var leftExpression = Parse(leftStatement);
                    var rightExpression = Parse(rightStatement);

                    // Success!
                    if (leftExpression != null && rightExpression != null)
                        return new MathExpression(nextOperator, leftExpression, rightExpression);

                    else
                        return null;
                }
            }
        }

        // Parses statement as an assignment statement. 
        private MathExpression? ParseAssignment(string statement)
        {
            // LHS = RHS
            var statementSplit = statement.Split(_configuration.SymbolTable.GetAssignmentOperator().Symbol,
                                                 StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            // Validation
            //
            // 1) Format:  Alphabetical characters only, no white spaces, parens, or underscores
            // 2) No existing symbols may exist
            // 

            if (statementSplit.Length != 2 ||
                string.IsNullOrWhiteSpace(statementSplit[0]) ||
                string.IsNullOrWhiteSpace(statementSplit[1]))
            {
                _calculatorLogger.Log("Improperly formed statement: " + statement, CalculatorLogType.SyntaxError);
                return null;
            }

            // Validates both function, or bare symbol
            if (!ValidateFunctionSymbol(statementSplit[0]))
            {
                _calculatorLogger.Log("Improper symbol name (must use alphabetical characters only): " + statement, CalculatorLogType.IllegalDeclaration);
                return null;
            }

            var isDefined = _configuration.SymbolTable.IsDefined(statementSplit[0]);
            var definedSymbolType = isDefined ? _configuration.SymbolTable.GetSymbolType(statementSplit[0]) : SymbolType.Constant;

            // Symbol Assignment
            if (isDefined)
            {
                switch (definedSymbolType)
                {
                    case SymbolType.Constant:
                    {
                        // Constant
                        var constant = _configuration.SymbolTable.Get(statementSplit[0]) as Constant;

                        // Recurse
                        var constantExpression = Parse(statementSplit[1]);

                        if (constantExpression == null)
                            return null;

                        else if (constantExpression.Type != MathExpressionType.Number)
                        {
                            _calculatorLogger.Log("Can only assign numeric values as constants", CalculatorLogType.IllegalDeclaration);
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
                        var variableExpression = Parse(statementSplit[1]);

                        if (variableExpression == null)
                            return null;

                        else if (variableExpression.Type != MathExpressionType.Number)
                        {
                            _calculatorLogger.Log("Can only assign numeric values as constants", CalculatorLogType.IllegalDeclaration);
                            return null;
                        }

                        else
                            return new MathExpression(variable, variableExpression);
                    }

                    case SymbolType.Function:       // Let this be done below
                        break;
                    case SymbolType.Operator:
                    {
                        _calculatorLogger.Log("Cannot redefine operator:  " + statementSplit[0], CalculatorLogType.IllegalDeclaration);
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
                _calculatorLogger.Log("Improperly formed function signature:  " + statementSplit[0], CalculatorLogType.ParseError);
                return null;
            }

            // Recurse
            var bodyMessage = string.Empty;
            var bodyExpression = Parse(statementSplit[1]);

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
                _calculatorLogger.Log("Improper function body expression:  " + statementSplit[1], CalculatorLogType.ParseError);
                return null;
            }
        }

        private FunctionSignature? ParseFunctionSignature(string signature)
        {
            // Validation
            //
            // 1) Cannot re-define symbol as variable (here)
            // 2) Signature form must be "f", or "f(arguments...)", or "funcName(arguments...)" 
            // 3) No recursion will be done for the argument list. These must be plain variables.
            //

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
                    // Re-Definition
                    if (_configuration.SymbolTable.IsDefined(variable.Symbol) &&
                        _configuration.SymbolTable.GetSymbolType(variable.Symbol) != SymbolType.Variable)
                    {
                        _calculatorLogger.Log("Cannot re-define symbol:  " + variable.Symbol, CalculatorLogType.IllegalDeclaration);
                        return null;
                    }

                    // CHECK RAW STRINGS - NOT JUST VARIABLES (could've been a constant or function)
                    if (!_configuration.SymbolTable.IsDefined(variable.Symbol))
                    {
                        _calculatorLogger.Log("Defining Variable:  " + variable.ToString(), CalculatorLogType.VariableDeclaration);
                        _configuration.SymbolTable.Add(variable, 0);
                    }
                }

                return new FunctionSignature(new Variable(symbol.GetSubString()), independentVariables);
            }
            else
                return null;
        }

        private MathExpression? ParseValueExpression(string expression)
        {
            // Procedure: If the expression is a number, then directly parse the number. Otherwise,
            //            it will be considered as a constant unless it is defined in the symbol 
            //            table.
            // 

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

            // Validate (new) Symbol
            if (ValidateSymbol(expression))
            {
                // NEW SYMBOL (CONSTANT)
                _calculatorLogger.Log("Defining Symbol:  " + expression, CalculatorLogType.ConstantDeclaration);

                var newConstant = new Constant(expression);

                _configuration.SymbolTable.Add(newConstant, 0);

                return new MathExpression(newConstant);
            }
            else
            {
                _calculatorLogger.Log("Improper symbol definition. Must use alphabetical characters only.", CalculatorLogType.ParseError);
                return null;
            }
        }

        private bool ValidateFunctionSymbol(string symbolExpression)
        {
            // Empty or Null
            if (string.IsNullOrWhiteSpace(symbolExpression))
                return false;

            // White Space 
            if (symbolExpression.Contains(" "))
                return false;

            var containsParens = symbolExpression.Contains(CalculatorConfiguration.LeftParenthesis) ||
                                 symbolExpression.Contains(CalculatorConfiguration.RightParenthesis);

            // Argument List
            if (containsParens)
            {
                // Remove Argument List
                var symbol = new SubstringLocator(symbolExpression, CalculatorConfiguration.LeftParenthesis, false);

                // Non Alphabetical Character(s)
                if (StringHelpers.RegexMatchIC(NON_ALPHA_CHARACTER, symbol.GetSubString()))
                    return false;

                return true;
            }

            // No Argument List
            else
            {
                // Non Alphabetical Character(s)
                if (StringHelpers.RegexMatchIC(NON_ALPHA_CHARACTER, symbolExpression))
                    return false;

                return true;
            }
        }

        private bool ValidateSymbol(string symbol)
        {
            // Empty or Null
            if (string.IsNullOrWhiteSpace(symbol))
                return false;

            // Non Alphabetical Character(s)
            if (StringHelpers.RegexMatchIC(NON_ALPHA_CHARACTER, symbol))
                return false;

            // White Space 
            if (symbol.Contains(" "))
                return false;

            return true;
        }
    }
}
