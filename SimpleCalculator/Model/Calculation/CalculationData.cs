using SimpleCalculator.Model.MathExpression;

namespace SimpleCalculator.Model.Calculation
{
    public class CalculationData
    {
        /// <summary>
        /// Reference to the CalculatorConfiguration's symbol table
        /// </summary>
        public SymbolTable SymbolTable { get; private set; }

        // Results for ValueExpression evaluations. These are calculated as the tree is parsed.
        private Dictionary<ValueExpression, double> _numericResults;

        public CalculationData(SymbolTable symbolTable)
        {
            this.SymbolTable = symbolTable;

            _numericResults = new Dictionary<ValueExpression, double>();
        }

        public void AddResult(ValueExpression expression, double numericValue)
        {
            _numericResults[expression] = numericValue;
        }

        public double GetResult(ValueExpression expression)
        {
            return _numericResults[expression];
        }
    }
}
