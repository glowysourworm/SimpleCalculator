namespace SimpleCalculator.Model.MathExpression
{
    public abstract class ExpressionBase
    {
        /// <summary>
        /// Expression for the value of the symbol
        /// </summary>
        public string Raw { get; private set; }

        protected ExpressionBase(string raw)
        {
            this.Raw = raw;
        }

        public override bool Equals(object? obj)
        {
            return this.Raw.Equals((obj as SymbolBase)?.Symbol);
        }

        public override int GetHashCode()
        {
            return this.Raw.GetHashCode();
        }

        public override string ToString()
        {
            return this.Raw;
        }
    }
}
