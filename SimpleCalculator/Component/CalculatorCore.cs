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

        public string Format(string statement)
        {
            return _expressionFormatter.PreFormat(statement);
        }

        public MathExpression? Validate(string statement, out string? message)
        {
            // Procedure
            //
            // 1) Check raw string format
            // 2) Run IExpressionParser
            // 3) Check symbols to evaluate to numeric values
            //

            // Check raw format
            message = _expressionFormatter.ValidatePreFormat(statement);

            if (message != null)
                return null;

            // Parse Expression
            var expression = _expressionParser.Parse(statement, out message);

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
                                message = "Undefined symbol: " + recurseExpression.Symbol.ToString();
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
            string? message = null;
            double numericResult = 0;

            switch (expression.Type)
            {
                case MathExpressionType.Number:
                case MathExpressionType.Constant:
                case MathExpressionType.Variable:
                case MathExpressionType.Function:
                    return EvaluateAsValueExpression(expression, out message);

                case MathExpressionType.Arithmetic:
                    return EvaluateAsArithmeticExpression(expression, out message);
                case MathExpressionType.Assignment:
                    return EvaluateAsAssignmentExpression(expression, out message);
                case MathExpressionType.Expression:     // Nothing to do                            
                    break;
                default:
                    throw new Exception("Unhandled MathExpressionType");
            }

            return new MathExpressionResult(MathExpressionType.Expression, "Unknown statement");
        }

        private MathExpressionResult EvaluateAsValueExpression(MathExpression expression, out string? message)
        {
            message = null;
            double number = 0;

            if (expression.Type == MathExpressionType.Number)
            {
                if (!double.TryParse(expression.Expression.ToString(), out number))
                    message = "Error trying to evaluate numeric symbol:  " + expression.Symbol?.ToString();
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
                var recursiveEvaluation = Evaluate(bodyExpression);

                message = recursiveEvaluation.ErrorMessage;
                number = recursiveEvaluation.NumericResult;
            }
            else
                throw new Exception("Unhandled Value Expression Type");

            return new MathExpressionResult(number);
        }

        private MathExpressionResult EvaluateAsArithmeticExpression(MathExpression expression, out string? message)
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

            message = leftResult?.ErrorMessage ?? rightResult?.ErrorMessage ?? null;

            if (leftResult == null ||
                rightResult == null)
                return new MathExpressionResult(MathExpressionType.Arithmetic, "Unknown evaluation error");

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
                        return new MathExpressionResult(MathExpressionType.Arithmetic, "Divide by zero error");

                    break;
                default:
                    throw new Exception("Unhandled Arithmetic Operator Type");
            }

            return new MathExpressionResult(result, MathExpressionType.Arithmetic);
        }

        private MathExpressionResult EvaluateAsAssignmentExpression(MathExpression expression, out string? message)
        {
            // No operator is currently used for assignment expressions. There was none needed after
            // parsing the MathExpression. The right operand was used as the body expression.
            //
            if (expression.RightOperand == null ||
                expression.Type != MathExpressionType.Assignment)
                throw new Exception("Improper use of MathExpression for assignment operator");

            // Evaluate the right operand - which will become the function value (and expression)
            var rightResult = Evaluate(expression.RightOperand);

            message = rightResult?.ErrorMessage ?? null;

            if (rightResult == null)
                return new MathExpressionResult(MathExpressionType.Assignment, "Unable to evaluate expression body");

            // Procedure
            //
            // 1) Recall function symbol from the assignemnt expression
            // 2) Set Function in symbol table
            //
            switch (expression.Symbol.SymbolType)
            {
                case SymbolType.Constant:

                    // Show Expression
                    _logger.Log("Defining Constant:  " + expression.ToString(), false);

                    // Define Constant
                    if (_configuration.SymbolTable.IsDefined(expression.Symbol))
                        _configuration.SymbolTable.SetValue(expression.Symbol as Constant, rightResult.NumericResult);

                    else
                        _configuration.SymbolTable.Add(expression.Symbol as Constant, rightResult.NumericResult);

                    break;
                case SymbolType.Variable:

                    // Show Expression
                    _logger.Log("Defining Variable:  " + expression.ToString(), false);

                    // Define Variable
                    if (_configuration.SymbolTable.IsDefined(expression.Symbol))
                        _configuration.SymbolTable.SetValue(expression.Symbol as Variable, rightResult.NumericResult);

                    else
                        _configuration.SymbolTable.Add(expression.Symbol as Variable, rightResult.NumericResult);

                    break;
                case SymbolType.Function:

                    // Show Expression
                    _logger.Log("Defining Function:  " + expression.ToString(), false);

                    // Define Variable
                    if (_configuration.SymbolTable.IsDefined(expression.Symbol))
                        _configuration.SymbolTable.SetValue(expression.Symbol as Function, expression.Symbol as Function);
                    else
                        _configuration.SymbolTable.Add(expression.Symbol as Function, expression.Symbol as Function);

                    break;
                case SymbolType.Operator:
                    _logger.Log("Cannot Re-define Operator:  " + expression.Symbol, true);
                    break;
                default:
                    throw new Exception("Unhandled symbol type");
            }

            return new MathExpressionResult(MathExpressionType.Assignment, null);
        }
    }
}
