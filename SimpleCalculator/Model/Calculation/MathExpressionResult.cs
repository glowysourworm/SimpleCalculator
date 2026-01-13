namespace SimpleCalculator.Model.Calculation
{
    public class MathExpressionResult
    {
        public MathExpression Expression { get; set; }
        public bool IsError { get; }
        public double NumericResult { get; }

        public MathExpressionResult(MathExpression expression, bool error, double numericResult)
        {
            this.Expression = expression;
            this.IsError = error;
            this.NumericResult = numericResult;
        }

        public override string ToString()
        {
            return string.Format("Expression={0}  Result={1}  Error={2}", this.Expression, this.NumericResult, this.IsError);
        }
    }
}
