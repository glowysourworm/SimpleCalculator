namespace SimpleCalculator.Model.Calculation
{
    public class CalculationData
    {
        /// <summary>
        /// Reference to the CalculatorConfiguration's symbol table
        /// </summary>
        public SymbolTable SymbolTable { get; private set; }

        public CalculationData(SymbolTable symbolTable)
        {
            this.SymbolTable = symbolTable;
        }
    }
}
