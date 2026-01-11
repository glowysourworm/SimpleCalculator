using SimpleCalculator.Component;

namespace SimpleCalculator.Model
{
    public class CalculatorConfiguration
    {
        public SymbolTable SymbolTable { get; private set; }

        public const char LeftParenthesis = '(';
        public const char RightParenthesis = ')';
        public const char FunctionVariableSeparator = ',';

        public CalculatorConfiguration()
        {
            this.SymbolTable = new SymbolTable();

            var assignment = new Operator("=", 0, OperatorType.Assignment);
            var addition = new Operator("+", 1, OperatorType.Addition);
            var subtraction = new Operator("-", 2, OperatorType.Subtraction);
            var muiltiplication = new Operator("*", 3, OperatorType.Multiplication);
            var division = new Operator("/", 4, OperatorType.Division);

            this.SymbolTable.Add(assignment, assignment);
            this.SymbolTable.Add(addition, addition);
            this.SymbolTable.Add(subtraction, subtraction);
            this.SymbolTable.Add(muiltiplication, muiltiplication);
            this.SymbolTable.Add(division, division);
        }
    }
}
