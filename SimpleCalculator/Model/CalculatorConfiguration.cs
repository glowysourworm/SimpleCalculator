using SimpleCalculator.Component;
using SimpleCalculator.Model.MathExpression;

namespace SimpleCalculator.Model
{
    public class CalculatorConfiguration
    {
        public SymbolTable SymbolTable { get; private set; }

        public const char LeftParenthesis = '(';
        public const char RightParenthesis = ')';

        public CalculatorConfiguration()
        {
            this.SymbolTable = new SymbolTable();

            this.SymbolTable.Add(new Operator("=", 0, OperatorType.Assignment), new Expression("="));
            this.SymbolTable.Add(new Operator("+", 1, OperatorType.Addition), new Expression("+"));
            this.SymbolTable.Add(new Operator("-", 2, OperatorType.Subtraction), new Expression("-"));
            this.SymbolTable.Add(new Operator("*", 3, OperatorType.Multiplication), new Expression("*"));
            this.SymbolTable.Add(new Operator("/", 4, OperatorType.Division), new Expression("/"));
        }
    }
}
