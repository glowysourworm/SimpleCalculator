namespace SimpleCalculator.Model.MathExpression
{
    /// <summary>
    /// Represents an abstract math expression, which requires evaluation before it can produce a value.
    /// </summary>
    public class Expression : ExpressionBase
    {
        public Expression(string raw) : base(raw)
        {
        }
    }
}
