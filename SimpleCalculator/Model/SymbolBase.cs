namespace SimpleCalculator.Model
{
    public abstract class SymbolBase
    {
        public string Symbol { get; private set; }

        public SymbolBase(string symbol)
        {
            this.Symbol = symbol;
        }

        public override bool Equals(object? obj)
        {
            return this.Symbol.Equals((obj as SymbolBase)?.Symbol);
        }

        public override int GetHashCode()
        {
            return this.Symbol.GetHashCode();
        }

        public override string ToString()
        {
            return this.Symbol;
        }
    }
}
