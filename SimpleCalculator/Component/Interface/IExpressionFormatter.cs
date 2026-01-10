namespace SimpleCalculator.Component.Interface
{
    /// <summary>
    /// Component responsible for statement preperation; and user feedback
    /// </summary>
    public interface IExpressionFormatter
    {
        /// <summary>
        /// Checks validity of input characters, and format of input characters. This happens before 
        /// parsing and syntax checking.
        /// </summary>
        string? ValidatePreFormat(string statement);

        /// <summary>
        /// Prepares statement for parsing:  removes white space; ...
        /// </summary>
        string PreFormat(string statement);
    }
}
