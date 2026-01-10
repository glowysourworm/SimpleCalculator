using SimpleCalculator.Component;

namespace SimpleCalculator.Model
{
    public class Operator : SymbolBase
    {
        public uint Order { get; private set; }

        public OperatorType Type { get; }

        public Operator(string symbol, uint order, OperatorType type) : base(symbol)
        {
            this.Order = order;
            this.Type = type;
        }

        public int CompareTo(Operator? other)
        {
            return this.Order.CompareTo(other?.Order ?? 0);
        }
    }
}
