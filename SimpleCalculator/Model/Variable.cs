using SimpleCalculator.Component;
using SimpleCalculator.Model.Interface;

namespace SimpleCalculator.Model
{
    public class Variable : ISymbol
    {
        public string Symbol { get; }
        public SymbolType SymbolType { get; }

        public Variable(string symbol) : base()
        {
            this.Symbol = symbol;
            this.SymbolType = SymbolType.Variable;
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
