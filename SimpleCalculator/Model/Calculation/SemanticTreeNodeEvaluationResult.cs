using SimpleCalculator.Component;

namespace SimpleCalculator.Model.Calculation
{
    /// <summary>
    /// Class that represents the evaluation result of a semantic tree node
    /// </summary>
    public class SemanticTreeNodeEvaluationResult
    {
        /// <summary>
        /// Type of node. (see enumeration for more details)
        /// </summary>
        public SemanticTreeNodeOperation Operation { get; set; }

        /// <summary>
        /// Numeric Result
        /// </summary>
        public double Value { get; private set; }

        /// <summary>
        /// Set if there is an error in the evaluation of the tree node
        /// </summary>
        public string? ErrorMessage { get; private set; }

        public SemanticTreeNodeEvaluationResult(double value, SemanticTreeNodeOperation operation, string? error = null)
        {
            this.Operation = operation;
            this.Value = value;
            this.ErrorMessage = error;
        }
    }
}
