using SimpleCalculator.Component;

namespace SimpleCalculator.Model
{
    public class CalculatorConfiguration
    {
        public SymbolTable SymbolTable { get; private set; }
        public CalculatorTheme Theme { get; private set; }

        public static string AssignmentOperator { get; private set; }
        public static char LeftParenthesis { get; private set; }
        public static char RightParenthesis { get; private set; }
        public static char FunctionVariableSeparator { get; private set; }

        public CalculatorConfiguration()
        {
            this.SymbolTable = new SymbolTable();
            this.Theme = new CalculatorTheme();

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

            // Defaults
            AssignmentOperator = "=";
            LeftParenthesis = '(';
            RightParenthesis = ')';
            FunctionVariableSeparator = ',';

            AddFunctions();
            AddConstants();
        }

        public CalculatorConfiguration(CalculatorConfiguration configuration)
        {
            this.SymbolTable = new SymbolTable();
            this.Theme = new CalculatorTheme()
            {
                CodeErrorForegroundColor = configuration.Theme.CodeErrorForegroundColor,
                CodeForegroundColor = configuration.Theme.CodeForegroundColor,
                PrimaryBackgroundColor = configuration.Theme.PrimaryBackgroundColor,
                PrimaryForegroundColor = configuration.Theme.PrimaryForegroundColor,
                SecondaryBackgroundColor = configuration.Theme.SecondaryBackgroundColor,
                TitleBackgroundColor = configuration.Theme.TitleBackgroundColor,
                TitleForegroundColor = configuration.Theme.TitleForegroundColor,
            };

            // Constants
            foreach (var constant in configuration.SymbolTable.Constants)
                this.SymbolTable.Add(constant, configuration.SymbolTable.GetValue(constant));

            // Variables
            foreach (var variable in configuration.SymbolTable.Variables)
                this.SymbolTable.Add(variable, configuration.SymbolTable.GetValue(variable));

            // Operators
            foreach (var ioperator in configuration.SymbolTable.Operators)
                this.SymbolTable.Add(ioperator, ioperator);

            // Functions
            foreach (var function in configuration.SymbolTable.Functions)
                this.SymbolTable.Add(function, function);

            // Defaults
            AssignmentOperator = configuration.SymbolTable.GetAssignmentOperator()?.Symbol ?? "=";
            LeftParenthesis = '(';
            RightParenthesis = ')';
            FunctionVariableSeparator = ',';

            AddFunctions();
            AddConstants();
        }

        private void AddFunctions()
        {
        }

        private void AddConstants()
        {

        }
    }
}
