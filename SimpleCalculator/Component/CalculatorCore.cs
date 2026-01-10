using SimpleCalculator.Component.Interface;
using SimpleCalculator.Extension;
using SimpleCalculator.Model;
using SimpleCalculator.Model.Calculation;

namespace SimpleCalculator.Component
{
    public class CalculatorCore : ICalculatorCore
    {
        private readonly CalculatorConfiguration _configuration;
        private readonly IExpressionParser _expressionParser;
        private readonly IExpressionFormatter _expressionFormatter;

        public CalculatorCore(CalculatorConfiguration configuration, IExpressionParser expressionParser, IExpressionFormatter expressionFormatter)
        {
            _configuration = configuration;
            _expressionFormatter = expressionFormatter;
            _expressionParser = expressionParser;
        }

        public string Format(string statement)
        {
            return _expressionFormatter.PreFormat(statement);
        }

        public string? Validate(string statement)
        {
            // Check raw format
            var message = _expressionFormatter.ValidatePreFormat(statement);

            if (message != null)
                return message;

            var symbols = statement.RemoveOperatorsAndParens(_configuration);

            foreach (var symbol in symbols)
            {
                // Check any numeric values
                if (!_configuration.SymbolTable.IsDefined(symbol))
                {
                    double number = 0;
                    if (!double.TryParse(symbol, out number))
                    {
                        return "Undefined, non-numeric symbol " + symbol;
                    }
                }
            }

            return null;
        }

        public SemanticTree Expand(string statement)
        {
            return _expressionParser.Parse(statement);
        }

        public SemanticTreeResult Evaluate(SemanticTree semanticTree)
        {
            return semanticTree.Execute(_configuration.SymbolTable);
        }
    }
}
