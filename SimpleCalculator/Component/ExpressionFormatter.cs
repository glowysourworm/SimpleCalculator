using SimpleCalculator.Component.Interface;
using SimpleCalculator.Model;

namespace SimpleCalculator.Component
{
    public class ExpressionFormatter : IExpressionFormatter
    {
        private readonly CalculatorConfiguration _configuration;

        public ExpressionFormatter(CalculatorConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string? ValidatePreFormat(string statement)
        {
            if (string.IsNullOrWhiteSpace(statement))
                return "Invalid statement";

            // Assignment Operator (should have validation for configuration)
            var assignmentOperator = _configuration.SymbolTable
                                                   .Operators
                                                   .FirstOrDefault(x => x.Type == OperatorType.Assignment);

            if (assignmentOperator == null)
                return "Invalid configuration. Must define an assignment operator";

            if (string.IsNullOrWhiteSpace(statement))
                return "Invalid expression. Must be a numeric, arithmetic, or function expression.";

            if (statement.Count(x => x.ToString() == assignmentOperator.Symbol) > 1)
                return "Invalid expression. Must contain up to one assignment operator.";

            return null;
        }

        public string PreFormat(string statement)
        {
            return statement.Replace(" ", "");
        }
    }
}
