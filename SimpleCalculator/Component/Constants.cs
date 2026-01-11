namespace SimpleCalculator.Component
{
    public enum OperatorType
    {
        Assignment = 0,
        Addition = 1,
        Subtraction = 2,
        Multiplication = 3,
        Division = 4
    }

    public enum SymbolType
    {
        Constant = 0,
        Variable = 1,
        Function = 2,
        Operator = 3
    }

    /// <summary>
    /// Type of math expression as identified by the calculator
    /// </summary>
    public enum MathExpressionType
    {
        /// <summary>
        /// (Symbol) A numeric value
        /// </summary>
        Number = 0,

        /// <summary>
        /// (Symbol) A constant defined with a numeric value
        /// </summary>
        Constant = 1,

        /// <summary>
        /// (Symbol) A variable defined in a function as the independent variable
        /// </summary>
        Variable = 2,

        /// <summary>
        /// (Symbol) This node type would be a symbol that represents a pre-defined function. Examples:  f, f(x), f(x, y), sin(x,y,z), etc...
        /// </summary>
        Function = 3,

        /// <summary>
        /// Math expression that consists of two operands, which may or may not be symbols.
        /// </summary>
        Arithmetic = 4,

        /// <summary>
        /// Math Expression consists of an assignment of an expression to a function signature (symbol)
        /// </summary>
        Assignment = 5,

        /// <summary>
        /// Any arithmetic expression consisting of symbols (may or may not be part of a function body)
        /// </summary>
        Expression = 6
    }
}
