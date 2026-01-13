using SimpleCalculator.Model.Interface;

namespace SimpleCalculator.Model
{
    public class Operator : ISymbol
    {
        public string Symbol { get; }
        public SymbolType SymbolType { get; }

        public uint Order { get; private set; }

        public OperatorType Type { get; }

        public Operator(string symbol, uint order, OperatorType type)
        {
            this.Symbol = symbol;
            this.Order = order;
            this.Type = type;
            this.SymbolType = SymbolType.Operator;
        }

        public int CompareTo(Operator? other)
        {
            return this.Order.CompareTo(other?.Order ?? 0);
        }

        public override bool Equals(object? obj)
        {
            return this.Symbol.Equals(((ISymbol)obj).Symbol);
        }

        public override int GetHashCode()
        {
            return this.Symbol.GetHashCode();
        }
    }
}
