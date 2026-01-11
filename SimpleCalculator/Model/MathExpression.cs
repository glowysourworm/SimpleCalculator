using SimpleCalculator.Component;
using SimpleCalculator.Model.Interface;

namespace SimpleCalculator.Model
{
    /// <summary>
    /// Un-evaluated expression. This may be parsed into other symbol types, or evaluated
    /// in the semantic tree.
    /// </summary>
    public class MathExpression : ExpressionBase
    {
        /// <summary>
        /// Type of math expression
        /// </summary>
        public MathExpressionType Type { get; private set; }

        /// <summary>
        /// Symbol for value expression. This type of expression will have a symbol that
        /// can be ascribed a value directly. This could be a: Number, Constant, Variable, 
        /// or Function. A function is not to be confused with an assignment type expression - 
        /// which has both a FunctionSignature, and a FunctionBody.
        /// </summary>
        public ISymbol? Symbol { get; private set; }

        /// <summary>
        /// Type of operator (for arithmetic type only)
        /// </summary>
        public Operator? Operator { get; private set; }

        /// <summary>
        /// Left operand (for arithmetic expression only)
        /// </summary>
        public MathExpression? LeftOperand { get; private set; }

        /// <summary>
        /// Right operand (for arithmetic expression only)
        /// </summary>
        public MathExpression? RightOperand { get; private set; }

        /// <summary>
        /// Creates an Expression type MathExpression
        /// </summary>
        public MathExpression(string expression) : base(expression)
        {
            this.Type = MathExpressionType.Expression;
        }

        /// <summary>
        /// Creates an arithmetic expression
        /// </summary>
        public MathExpression(Operator ioperator, MathExpression leftOperand, MathExpression rightOperand)
            : base(leftOperand.Expression + ioperator.Symbol + rightOperand.Expression)
        {
            this.LeftOperand = leftOperand;
            this.RightOperand = rightOperand;
            this.Type = MathExpressionType.Arithmetic;
            this.Operator = ioperator;
        }

        /// <summary>
        /// (SYMBOL) Createse an expression for a numeric value
        /// </summary>
        public MathExpression(double numericValue)
            : base(numericValue.ToString())
        {
            this.Type = MathExpressionType.Number;
        }

        /// <summary>
        /// (SYMBOL) Creates an expression for a constant
        /// </summary>
        public MathExpression(Constant constant) : base(constant.ToString())
        {
            this.Type = MathExpressionType.Constant;
            this.Symbol = constant;
        }

        /// <summary>
        /// (SYMBOL) Creates an expression for a variable
        /// </summary>
        public MathExpression(Variable variable) : base(variable.ToString())
        {
            this.Type = MathExpressionType.Variable;
            this.Symbol = variable;
        }

        /// <summary>
        /// (SYMBOL) Creates an expression for a function
        /// </summary>
        public MathExpression(Function function) : base(function.ToString())
        {
            this.Type = MathExpressionType.Function;
            this.Symbol = function;
        }

        /// <summary>
        /// Creates an assignment expression for a constant
        /// </summary>
        public MathExpression(Constant constant, MathExpression rightOperand) : base(constant.Symbol + "=" + rightOperand.Expression)
        {
            this.Type = MathExpressionType.Assignment;
            this.RightOperand = rightOperand;
            this.Symbol = constant;
        }

        /// <summary>
        /// Creates an assignment expression for a variable
        /// </summary>
        public MathExpression(Variable variable, MathExpression rightOperand) : base(variable.Symbol + "=" + rightOperand.Expression)
        {
            this.Type = MathExpressionType.Assignment;
            this.RightOperand = rightOperand;
            this.Symbol = variable;
        }

        /// <summary>
        /// Creates an assignment expression for a function
        /// </summary>
        public MathExpression(Function function, MathExpression rightOperand) : base(function.Symbol + "=" + rightOperand.Expression)
        {
            this.Type = MathExpressionType.Assignment;
            this.RightOperand = rightOperand;
            this.Symbol = function;
        }

        public override string ToString()
        {
            return this.Expression;
        }
    }
}
