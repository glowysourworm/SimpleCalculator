using SimpleCalculator.Model;
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
        /// Formats math statement:  1) Removes white space, TBD... (Try regex's)
        /// </summary>
        string Format(string statement);

        /// <summary>
        /// Validates raw math statement. Returns message about formatting if there is an error. Otherwise, 
        /// returns null.
        /// </summary>
        MathExpression? Validate(string statement);

        /// <summary>
        /// Calculates the semantic tree and produces a result
        /// </summary>
        MathExpressionResult Evaluate(MathExpression expression);
    }
}
