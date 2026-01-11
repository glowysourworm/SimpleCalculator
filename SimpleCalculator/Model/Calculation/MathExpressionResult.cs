using SimpleCalculator.Component;

namespace SimpleCalculator.Model.Calculation
{
    public class MathExpressionResult
    {
        public MathExpressionType OperationType { get; set; }
        public string? ErrorMessage { get; }
        public double NumericResult { get; }

        public MathExpressionResult(MathExpressionType operationType, string? errorMessage)
        {
            this.OperationType = operationType;
            this.ErrorMessage = errorMessage;
            this.NumericResult = 0;
        }

        public MathExpressionResult(double numericResult, MathExpressionType type = MathExpressionType.Arithmetic)
            : this(type, null)
        {
            this.NumericResult = numericResult;
        }

        /// <summary>
        /// Numeric Result
        /// </summary>
        public MathExpressionResult(double numericResult)
            : this(MathExpressionType.Number, null)
        {
            this.NumericResult = numericResult;
        }

        public override string ToString()
        {
            return string.Format("Result={0} + Error={1}", this.NumericResult, this.ErrorMessage ?? string.Empty);
        }
    }
}
