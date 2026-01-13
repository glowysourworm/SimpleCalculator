using SimpleCalculator.Model.Interface;

namespace SimpleCalculator.Model
{
    public class Constant : ISymbol
    {
        public string Symbol { get; }
        public SymbolType SymbolType { get; }

        public Constant(string symbol)
        {
            this.Symbol = symbol;
            this.SymbolType = SymbolType.Constant;
        }

        public override bool Equals(object? obj)
        {
            return this.Symbol.Equals(((ISymbol)obj).Symbol);
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
