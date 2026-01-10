using SimpleCalculator.Model.Calculation;

namespace SimpleCalculator.Component.Interface
{
    public interface IExpressionParser
    {
        /// <summary>
        /// Parses the statement into an Expression. All constants, variables, and functions must be valid and defined
        /// before this will complete successfully. Please use the validation methiods here to be aware that the statement
        /// is properly formatted.
        /// </summary>
        SemanticTree Parse(string statement);
    }
}
