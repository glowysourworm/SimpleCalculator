namespace SimpleCalculator.Model
{
    public enum OperatorType
    {
        Assignment = 0,
        Addition = 1,
        Subtraction = 2,
        Multiplication = 3,
        Division = 4,
        Modulo = 5
    }

    public enum SymbolType
    {
        Constant = 0,
        Variable = 1,
        Function = 2,
        Operator = 3
    }

    public enum KeywordType
    {
        /// <summary>
        /// Keyword for help on a topic
        /// </summary>
        Help = 0,

        /// <summary>
        /// Keyword for listing symbol values
        /// </summary>
        List = 1,

        /// <summary>
        /// Keyword for a constant
        /// </summary>
        Constant = 2,

        /// <summary>
        /// Keyword for a variable
        /// </summary>
        Variable = 3,

        /// <summary>
        /// Keyword for a function
        /// </summary>
        Function = 4,

        /// <summary>
        /// Keyword for an operator
        /// </summary>
        Operator = 5,

        /// <summary>
        /// Keyword for setting a symbol value
        /// </summary>
        Set = 6,

        /// <summary>
        /// Keyword for clearing a symbol
        /// </summary>
        Clear = 7,

        /// <summary>
        /// Keyword for plotting a function
        /// </summary>
        Plot = 8
    }

    public enum CalculatorLogType
    {
        /// <summary>
        /// Either user input, or normal message from the application
        /// </summary>
        Normal = 0,

        /// <summary>
        /// Parse error occurred while parsing
        /// </summary>
        ParseError = 1,

        /// <summary>
        /// Improper use of syntax by the user
        /// </summary>
        SyntaxError = 2,

        /// <summary>
        /// Constant declared in the application
        /// </summary>
        ConstantDeclaration = 3,

        /// <summary>
        /// Variable declared in the application
        /// </summary>
        VariableDeclaration = 4,

        /// <summary>
        /// Function declared in the application
        /// </summary>
        FunctionDeclaration = 5,

        /// <summary>
        /// Can occur when trying to re-define constant, variable, or function out of context. Also,
        /// occurs when trying to re-define an operator, or when there are illegal characters in the
        /// declaration.
        /// </summary>
        IllegalDeclaration = 6,

        /// <summary>
        /// Terminal output from application:  Help Menu, Commands, etc...
        /// </summary>
        Terminal = 7,

        /// <summary>
        /// Numeric or symbolic result from application
        /// </summary>
        Result = 8,

        /// <summary>
        /// Result error - division by zero
        /// </summary>
        DivideByZero = 9
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
