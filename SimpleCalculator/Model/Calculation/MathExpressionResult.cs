namespace SimpleCalculator.Model.Calculation
{
    public class MathExpressionResult
    {
        public MathExpressionType OperationType { get; set; }
        public bool IsError { get; }
        public double NumericResult { get; }

        public MathExpressionResult(MathExpressionType operationType, bool error)
        {
            this.OperationType = operationType;
            this.IsError = error;
            this.NumericResult = 0;
        }

        public MathExpressionResult(double numericResult, MathExpressionType type = MathExpressionType.Arithmetic)
            : this(type, false)
        {
            this.NumericResult = numericResult;
        }

        /// <summary>
        /// Numeric Result
        /// </summary>
        public MathExpressionResult(double numericResult)
            : this(MathExpressionType.Number, false)
        {
            this.NumericResult = numericResult;
        }

        public override string ToString()
        {
            return string.Format("Result={0} + Error={1}", this.NumericResult, this.IsError);
        }
    }
}
