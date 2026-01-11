namespace SimpleCalculator.Model
{
    /// <summary>
    /// An expression is any alpha-numeric math expression. It differs from a function in that
    /// it does not have an assigned symbol.
    /// </summary>
    public class ExpressionBase
    {
        public string Expression { get; }

        public ExpressionBase(string expression)
        {
            this.Expression = expression;
        }
    }
}
