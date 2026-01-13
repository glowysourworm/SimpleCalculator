using SimpleCalculator.Component.Interface;
using SimpleCalculator.Model;

namespace SimpleCalculator.Component
{
    public class ExpressionFormatter : IExpressionFormatter
    {
        private readonly CalculatorConfiguration _configuration;
        private readonly ICalculatorLogger _logger;

        public ExpressionFormatter(CalculatorConfiguration configuration, ICalculatorLogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public bool ValidatePreFormat(string statement)
        {
            string? message = null;

            if (string.IsNullOrWhiteSpace(statement))
                message = "Invalid statement";

            // Assignment Operator (should have validation for configuration)
            var assignmentOperator = _configuration.SymbolTable
                                                   .Operators
                                                   .FirstOrDefault(x => x.Type == OperatorType.Assignment);

            if (assignmentOperator == null)
                message = "Invalid configuration. Must define an assignment operator";

            if (string.IsNullOrWhiteSpace(statement))
                message = "Invalid expression. Must be a numeric, arithmetic, or function expression.";

            if (statement.Count(x => x.ToString() == assignmentOperator.Symbol) > 1)
                message = "Invalid expression. Must contain up to one assignment operator.";

            if (message != null)
                _logger.Log(message, CalculatorLogType.ParseError);

            return message == null;
        }

        public string PreFormat(string statement)
        {
            return statement.Replace(" ", "");
        }
    }
}
