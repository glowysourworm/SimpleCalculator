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
        Number = 0,
        Constant = 1,
        Variable = 2,
        Function = 3
    }

    public enum ValueExpressionType
    {
        /// <summary>
        /// Value is just a simple number
        /// </summary>
        Number = 0,

        /// <summary>
        /// Value must be evaluated from the symbol table, and may require recursive evaluation
        /// in the case of functions.
        /// </summary>
        Evaluated = 1
    }

    public enum SemanticTreeNodeType
    {
        /// <summary>
        /// A numeric value
        /// </summary>
        Number = 0,

        /// <summary>
        /// A constant defined with a numeric value
        /// </summary>
        Constant = 1,

        /// <summary>
        /// A variable defined in a function as the independent variable
        /// </summary>
        Variable = 2,

        /// <summary>
        /// This node type would be a symbol that represents a pre-defined function. Examples:  f, f(x), f(x, y), sin(x,y,z), etc...
        /// </summary>
        Function = 3,

        /// <summary>
        /// Implies that the current node is a sub-tree that does not
        /// directly resolve into one of the other types. A function, for
        /// example, would be considered its own ISemanticTree, which would
        /// have a separate method of calculation. (Although, the function
        /// node will not necessarily be treated that way. The function will
        /// have its own designator, like "f")
        /// </summary>
        Expression = 4
    }

    public enum SemanticTreeNodeOperation
    {
        /// <summary>
        /// Evaluating the node results in a numeric value:  Number, Constant, Variable, or Function
        /// </summary>
        NumericEvaulation = 0,

        /// <summary>
        /// Evaluating the node results in a recursive operation which performs numeric arithmetic. This
        /// may involve any type of sub-nodes, but will produce a number.
        /// </summary>
        Arithmetic = 1,

        /// <summary>
        /// This will assign the right operand to the left operand (of the assignment operator). This will
        /// only be valid if:  1) All variables, and constants, on the RHS are defined prior to evaluation.
        /// and 2) The LHS symbol is properly formatted. (see ISymbolParser)
        /// </summary>
        Assignment = 2
    }

    public enum SemanticTreeResultStatus
    {
        None = 0,
        Success = 1,
        SynataxError = 2,
        ExecutionError = 3
    }
}
