using SimpleCalculator.Component;

namespace SimpleCalculator.Model.Calculation
{
    public class SemanticTreeResult
    {
        public SemanticTreeResultStatus Status { get; }
        public string Message { get; }
        public double NumericResult { get; }

        public SemanticTreeResult(SemanticTreeResultStatus status,
                                  string message,
                                  double numericResult)
        {
            this.Status = status;
            this.Message = message;
            this.NumericResult = numericResult;
        }
    }
}
