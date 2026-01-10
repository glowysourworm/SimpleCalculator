using SimpleCalculator.Component.Interface;
using SimpleCalculator.Model;
using SimpleCalculator.Model.Calculation;
using SimpleCalculator.Model.MathExpression;

namespace SimpleCalculator.Component
{
    public class ExpressionParser : IExpressionParser
    {
        private readonly CalculatorConfiguration _configuration;

        public ExpressionParser(CalculatorConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SemanticTree Parse(string statement)
        {
            var root = ParseImpl(statement);

            return new SemanticTree(root);
        }

        private SemanticTreeNode ParseImpl(string statement)
        {
            // Procedure
            //
            // 0) Locate parenthesis pairs - CHECK FOR ENTIRE STATEMENT
            // 1) Locate outer-most parenthesis sub-tree(s)
            // 2) Locate operators between these sub-tree(s)
            //     - Treat outer-most paren'd sub-tree (becomes left operand)
            //     - Then, operator
            //     - Then, right operand
            //
            // 3) Parse each sub-tree type as a sub-node of the current node, recursively
            //

            // CHECK ENTIRE STATEMENT BRACKETING
            if (statement.StartsWith(CalculatorConfiguration.LeftParenthesis) &&
                statement.EndsWith(CalculatorConfiguration.RightParenthesis))
                statement = statement.Substring(1, statement.Length - 2);

            // Get outermost paren'd sub-statements
            var parenthesisData = ExpressionCalculator.GetOutermostParens(statement);

            // Operator Index
            Operator? nextOperator = null;
            var operatorIndex = ExpressionCalculator.LocateNextOperator(_configuration, statement, parenthesisData.Outermost, out nextOperator);

            // Value Expression: (no operator present)
            if (nextOperator == null)
            {
                SemanticTreeNodeType nodeType = SemanticTreeNodeType.Expression;

                // Operand Node (Left)
                return ParseAsValueExpression(statement, parenthesisData.Outermost, out nodeType);
            }

            // Next Operator (Arithmetic Statement / Assignment)
            else
            {
                // Parse these as sub-statements (RECURSE)
                //
                var leftStatement = statement.Substring(0, operatorIndex);
                var rightStatement = statement.Substring(operatorIndex + 1, statement.Length - operatorIndex - 1);

                // -> ParseImpl (RECURSE)
                SemanticTreeNode leftNode = ParseImpl(leftStatement);
                SemanticTreeNode rightNode = ParseImpl(rightStatement);

                // Expression is not a ValueExpression (This must be set up with the SymbolTable, defined as a function)
                return new SemanticTreeNode(new Expression(statement), SemanticTreeNodeType.Expression, leftNode, rightNode, nextOperator);
            }
        }

        private SemanticTreeNode ParseAsValueExpression(string expression, List<SubstringLocator> outermostParenStatements, out SemanticTreeNodeType nodeType)
        {
            // Value Expression:  Number
            double numericValue = 0;
            if (double.TryParse(expression, out numericValue))
            {
                nodeType = SemanticTreeNodeType.Number;
                return new SemanticTreeNode(new ValueExpression(expression, ValueExpressionType.Number, numericValue), SemanticTreeNodeType.Number);
            }

            // Value Expression: Constant
            foreach (var constant in _configuration.SymbolTable.Constants)
            {
                if (constant.Symbol == expression)
                {
                    nodeType = SemanticTreeNodeType.Constant;
                    return new SemanticTreeNode(_configuration.SymbolTable.GetValue(constant), SemanticTreeNodeType.Constant);
                }
            }

            // Value Expression: Variable
            foreach (var variable in _configuration.SymbolTable.Variables)
            {
                if (variable.Symbol == expression)
                {
                    nodeType = SemanticTreeNodeType.Variable;
                    return new SemanticTreeNode(_configuration.SymbolTable.GetValue(variable), SemanticTreeNodeType.Variable);
                }
            }

            // Value Expression: Function (THESE MUST BE PRE-VALIDATED!)
            foreach (var function in _configuration.SymbolTable.Functions)
            {
                if (function.Symbol == expression)
                {
                    nodeType = SemanticTreeNodeType.Function;
                    return new SemanticTreeNode(_configuration.SymbolTable.GetValue(function), SemanticTreeNodeType.Function);
                }
            }

            // Parsed Operand:  Will be a number unless there is an undefined constant, variable, or function
            throw new Exception("Undefined Value Expression:  " + expression);
        }
    }
}
