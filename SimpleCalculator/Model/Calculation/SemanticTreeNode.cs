using SimpleCalculator.Component;
using SimpleCalculator.Model.MathExpression;

namespace SimpleCalculator.Model.Calculation
{
    public class SemanticTreeNode
    {
        public ExpressionBase Expression { get; }
        public SemanticTreeNodeType Type { get; private set; }
        public SemanticTreeNode LeftNode { get; private set; }
        public SemanticTreeNode? RightNode { get; private set; }
        public Operator? Operator { get; private set; }

        /// <summary>
        /// Constructs a simple value expression node
        /// </summary>
        /// <param name="valueExpression"></param>
        /// <param name="type"></param>
        public SemanticTreeNode(ValueExpression valueExpression,
                                SemanticTreeNodeType type)
        {
            this.Expression = valueExpression;
            this.Type = type;
        }

        /// <summary>
        /// Constructs a node with both left and right operands
        /// </summary>
        public SemanticTreeNode(ExpressionBase expression,
                                SemanticTreeNodeType type,
                                SemanticTreeNode left,
                                SemanticTreeNode? right,
                                Operator? theOperator)
        {
            this.Expression = expression;
            this.Type = type;
            this.LeftNode = left;
            this.RightNode = right;
            this.Operator = theOperator;
        }

        private bool Validate(SymbolTable symbolTable)
        {
            if (this.LeftNode == null)
                return false;

            double result = 0;

            switch (this.Type)
            {
                // Parses as a number
                case SemanticTreeNodeType.Number:
                    return this.Expression is ValueExpression && (this.Expression as ValueExpression).Type == ValueExpressionType.Number;

                // Has defined symbol
                case SemanticTreeNodeType.Constant:
                case SemanticTreeNodeType.Variable:
                case SemanticTreeNodeType.Function:
                    return this.Expression is ValueExpression && symbolTable.IsDefined(this.Expression.Raw);

                case SemanticTreeNodeType.Expression:
                    return this.RightNode != null && this.Operator != null;
                default:
                    throw new Exception("Unhandled ISemanticTreeNode type");
            }
        }

        public SemanticTreeNodeEvaluationResult Evaluate(CalculationData data)
        {
            if (!Validate(data.SymbolTable))
                throw new Exception("Trying to evaluate SemanticTreeNode without either: 1) Parsing it first, or 2) Without a valid structure");

            switch (this.Type)
            {
                // ValueExpression -> Evaluate
                case SemanticTreeNodeType.Number:
                case SemanticTreeNodeType.Constant:
                case SemanticTreeNodeType.Variable:
                case SemanticTreeNodeType.Function:
                {
                    // Value Expressions are pre-evaluated
                    //
                    var numericValue = (this.Expression as ValueExpression).GetNumericValue();

                    return new SemanticTreeNodeEvaluationResult(numericValue, SemanticTreeNodeOperation.NumericEvaulation);
                }
                case SemanticTreeNodeType.Expression:
                    return EvaluateAsExpression(data);

                default:
                    throw new Exception("Unhandled ISemanticTreeNode type");
            }
        }

        private SemanticTreeNodeEvaluationResult EvaluateAsExpression(CalculationData data)
        {
            if (this.LeftNode == null ||
                this.RightNode == null ||
                this.Operator == null)
                throw new Exception("Invalid Semantic Tree Node:  Must parse and validate before calling Evaluate()");

            var leftResult = this.LeftNode.Evaluate(data);
            var rightResult = this.RightNode.Evaluate(data);
            var result = 0.0D;
            var divideByZero = false;
            var operation = SemanticTreeNodeOperation.Arithmetic;

            switch (this.Operator.Type)
            {
                case OperatorType.Addition:
                    result = leftResult.Value + rightResult.Value;
                    break;
                case OperatorType.Subtraction:
                    result = leftResult.Value - rightResult.Value;
                    break;
                case OperatorType.Multiplication:
                    result = leftResult.Value * rightResult.Value;
                    break;
                case OperatorType.Division:
                    divideByZero = (rightResult.Value == 0);
                    result = !divideByZero ? (leftResult.Value / rightResult.Value) : 0;
                    break;
                case OperatorType.Assignment:
                    result = 0;
                    operation = SemanticTreeNodeOperation.Assignment;
                    break;
                default:
                    throw new Exception("Unhandled Semantic Tree Node Operator Arithmetic Function Type");
            }

            return new SemanticTreeNodeEvaluationResult(result, operation, divideByZero ? "Divide by zero error!" : null);
        }
    }
}
