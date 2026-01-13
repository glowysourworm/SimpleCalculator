using SimpleCalculator.Component.Interface;
using SimpleCalculator.Model;
using SimpleCalculator.Model.Calculation;

namespace SimpleCalculator.Component
{
    public class CalculatorCore : ICalculatorCore
    {
        private readonly CalculatorConfiguration _configuration;
        private readonly ICalculatorLogger _logger;
        private readonly IExpressionParser _expressionParser;
        private readonly IExpressionFormatter _expressionFormatter;

        public CalculatorCore(CalculatorConfiguration configuration,
                              ICalculatorLogger logger,
                              IExpressionParser expressionParser,
                              IExpressionFormatter expressionFormatter)
        {
            _configuration = configuration;
            _expressionFormatter = expressionFormatter;
            _expressionParser = expressionParser;
            _logger = logger;
        }

        public StatementType CalculateStatementType(string statement)
        {
            if (string.IsNullOrWhiteSpace(statement))
                return StatementType.Invalid;

            // There could be pieces of a math expression in here.
            var statementParts = statement.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList();

            // OperatorLed
            if (_configuration.SymbolTable.Operators.Any(x => x.Symbol == statementParts[0]))
                return StatementType.OperatorLed;

            // Keyword
            if (_configuration.Keywords.Any(x => x.Name == statementParts[0]))
                return StatementType.Terminal;

            return StatementType.Math;
        }

        public string FormatMathStatement(string statement)
        {
            return _expressionFormatter.PreFormat(statement);
        }

        public MathExpression? Validate(string statement)
        {
            // Procedure
            //
            // 1) Check raw string format
            // 2) Run IExpressionParser
            // 3) Check symbols to evaluate to numeric values
            //

            // Check raw format
            if (!_expressionFormatter.ValidatePreFormat(statement))
                return null;

            // Parse Expression
            var expression = _expressionParser.Parse(statement);

            if (expression == null)
                return null;

            var recurseExpression = expression;

            // Check Symbols
            do
            {
                if (recurseExpression.Symbol != null)
                {
                    // Numeric Values
                    switch (recurseExpression.Type)
                    {
                        case MathExpressionType.Constant:
                        case MathExpressionType.Variable:
                        case MathExpressionType.Function:

                            // Check Definition
                            if (!_configuration.SymbolTable.IsDefined(recurseExpression.Symbol))
                            {
                                _logger.Log("Undefined symbol: " + recurseExpression.Symbol.ToString(), CalculatorLogType.ParseError);
                                return null;
                            }
                            break;

                        case MathExpressionType.Number:
                        case MathExpressionType.Arithmetic:
                        case MathExpressionType.Assignment:
                        case MathExpressionType.Expression:     // Nothing to do                            
                            break;
                        default:
                            throw new Exception("Unhandled MathExpressionType");
                    }
                }

                recurseExpression = recurseExpression.LeftOperand ?? null;

            } while (recurseExpression != null);

            return expression;
        }

        public MathExpressionResult Evaluate(MathExpression expression)
        {
            switch (expression.Type)
            {
                case MathExpressionType.Number:
                case MathExpressionType.Constant:
                case MathExpressionType.Variable:
                case MathExpressionType.Function:
                    return EvaluateAsValueExpression(expression);

                case MathExpressionType.Arithmetic:
                    return EvaluateAsArithmeticExpression(expression);
                case MathExpressionType.Assignment:
                    return EvaluateAsAssignmentExpression(expression);
                case MathExpressionType.Expression:                     // These are body expressions for functions
                    return Evaluate(expression);
                default:
                    throw new Exception("Unhandled MathExpressionType");
            }
        }

        #region (private) Evaluation Methods
        private MathExpressionResult EvaluateAsValueExpression(MathExpression expression)
        {
            double number = 0;

            if (expression.Type == MathExpressionType.Number)
            {
                if (!double.TryParse(expression.Expression.ToString(), out number))
                {
                    _logger.Log("Error trying to evaluate numeric symbol:  " + expression.Symbol?.ToString(), CalculatorLogType.ParseError);

                    return new MathExpressionResult(expression, true, 0);
                }
            }

            else if (expression.Type == MathExpressionType.Constant)
            {
                number = _configuration.SymbolTable.GetValue(expression.Symbol as Constant);
            }

            else if (expression.Type == MathExpressionType.Variable)
            {
                number = _configuration.SymbolTable.GetValue(expression.Symbol as Variable);
            }

            else if (expression.Type == MathExpressionType.Function)
            {
                // Function has been pre-parsed in a previous assign statement (our current expression just carries that symbol)
                var function = _configuration.SymbolTable.GetValue(expression.Symbol as Function);

                // Recall the body of the function. This will have been pre-validated!
                var bodyExpression = new MathExpression(function.Body.Expression);

                // Recurse!
                return Evaluate(bodyExpression);
            }
            else
                throw new Exception("Unhandled Value Expression Type");

            return new MathExpressionResult(expression, false, number);
        }
        private MathExpressionResult EvaluateAsArithmeticExpression(MathExpression expression)
        {
            if (expression.LeftOperand == null ||
                expression.RightOperand == null ||
                expression.Operator == null ||
                expression.Operator.Type == OperatorType.Assignment ||
                expression.Type != MathExpressionType.Arithmetic)
                throw new Exception("Improper use of MathExpression for arithmetic operator");

            var leftResult = Evaluate(expression.LeftOperand);
            var rightResult = Evaluate(expression.RightOperand);
            var result = 0.0D;
            var divideByZero = false;

            if (leftResult == null ||
                rightResult == null)
                return new MathExpressionResult(expression, true, 0);

            switch (expression.Operator.Type)
            {
                case OperatorType.Addition:
                    result = leftResult.NumericResult + rightResult.NumericResult;
                    break;
                case OperatorType.Subtraction:
                    result = leftResult.NumericResult - rightResult.NumericResult;
                    break;
                case OperatorType.Multiplication:
                    result = leftResult.NumericResult * rightResult.NumericResult;
                    break;
                case OperatorType.Division:
                    divideByZero = (rightResult.NumericResult == 0);

                    if (!divideByZero)
                    {
                        result = leftResult.NumericResult / rightResult.NumericResult;
                    }
                    else
                    {
                        _logger.Log(expression.Expression, CalculatorLogType.DivideByZero);

                        return new MathExpressionResult(expression, true, 0);
                    }

                    break;
                case OperatorType.Modulo:
                    result = leftResult.NumericResult % rightResult.NumericResult;
                    break;
                default:
                    throw new Exception("Unhandled Arithmetic Operator Type");
            }

            return new MathExpressionResult(expression, false, result);
        }
        private MathExpressionResult EvaluateAsAssignmentExpression(MathExpression expression)
        {
            // No operator is currently used for assignment expressions. There was none needed after
            // parsing the MathExpression. The right operand was used as the body expression.
            //
            if (expression.RightOperand == null ||
                expression.Type != MathExpressionType.Assignment)
                throw new Exception("Improper use of MathExpression for assignment operator");

            // Evaluate the right operand - which will become the function value (and expression)
            var rightResult = Evaluate(expression.RightOperand);

            if (rightResult == null)
            {
                _logger.Log("Unable to evaluate expression body", CalculatorLogType.ParseError);

                return new MathExpressionResult(expression, true, 0);
            }

            // Procedure
            //
            // 1) Recall function symbol from the assignemnt expression
            // 2) Set Function in symbol table
            //
            switch (expression.Symbol.SymbolType)
            {
                case SymbolType.Constant:

                    // Set Constant
                    if (_configuration.SymbolTable.IsDefined(expression.Symbol))
                        _configuration.SymbolTable.SetValue(expression.Symbol as Constant, rightResult.NumericResult);

                    else
                        throw new Exception("Trying to assign to undefined symbol:  ICalculatorCore");

                    break;
                case SymbolType.Variable:

                    // Set Variable
                    if (_configuration.SymbolTable.IsDefined(expression.Symbol))
                        _configuration.SymbolTable.SetValue(expression.Symbol as Variable, rightResult.NumericResult);

                    else
                        throw new Exception("Trying to assign to undefined symbol:  ICalculatorCore");

                    break;
                case SymbolType.Function:

                    // Set Function
                    if (_configuration.SymbolTable.IsDefined(expression.Symbol))
                    {
                        var function = _configuration.SymbolTable.Get(expression.Symbol.Symbol) as Function;

                        // Set body expression
                        function.SetBody(expression.RightOperand);
                    }
                    else
                        throw new Exception("Trying to assign to undefined symbol:  ICalculatorCore");

                    break;
                case SymbolType.Operator:
                    _logger.Log("Cannot redefine operator:  " + expression.Symbol, CalculatorLogType.IllegalDeclaration);
                    break;
                default:
                    throw new Exception("Unhandled symbol type");
            }

            return new MathExpressionResult(expression, true, 0);
        }
        #endregion
    }
}
