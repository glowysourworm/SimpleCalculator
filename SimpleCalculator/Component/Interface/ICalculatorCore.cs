using SimpleCalculator.Model.Calculation;

namespace SimpleCalculator.Component.Interface
{
    /// <summary>
    /// Primary component for the calculator. This component should contain all sub-components, and configuration instance
    /// which contains the up-to-date symbol table (reference).
    /// </summary>
    public interface ICalculatorCore
    {
        /// <summary>
        /// Validates raw math statement. Returns message about formatting if there is an error. Otherwise, 
        /// returns null.
        /// </summary>
        string? Validate(string statement);

        /// <summary>
        /// Creates SemanticTree from expression
        /// </summary>
        SemanticTree Expand(string statement);

        /// <summary>
        /// Calculates the semantic tree and produces a result
        /// </summary>
        SemanticTreeResult Evaluate(SemanticTree semanticTree);
    }
}
