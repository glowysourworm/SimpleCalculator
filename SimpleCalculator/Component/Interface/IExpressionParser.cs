using SimpleCalculator.Model;

namespace SimpleCalculator.Component.Interface
{
    public interface IExpressionParser
    {
        /// <summary>
        /// Parses the statement into an Expression. The state of the MathExpression depends on the readiness
        /// of all varialbes, functions, and constants in the expression.
        /// </summary>
        MathExpression? Parse(string statement);
    }
}
