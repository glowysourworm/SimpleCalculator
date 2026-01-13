namespace SimpleCalculator.Model
{
    public class CalculatorConfiguration
    {
        public SymbolTable SymbolTable { get; private set; }
        public CalculatorTheme Theme { get; private set; }
        public List<Keyword> Keywords { get; private set; }

        public static string AssignmentOperator { get; private set; }
        public static char LeftParenthesis { get; private set; }
        public static char RightParenthesis { get; private set; }
        public static char LeftSquareBracket { get; private set; }
        public static char RightSquareBracket { get; private set; }
        public static char FunctionVariableSeparator { get; private set; }

        public CalculatorConfiguration()
        {
            this.SymbolTable = new SymbolTable();
            this.Theme = new CalculatorTheme();
            this.Keywords = new List<Keyword>();

            var assignment = new Operator("=", 0, OperatorType.Assignment);
            var addition = new Operator("+", 1, OperatorType.Addition);
            var subtraction = new Operator("-", 2, OperatorType.Subtraction);
            var muiltiplication = new Operator("*", 3, OperatorType.Multiplication);
            var division = new Operator("/", 4, OperatorType.Division);
            var modulo = new Operator("%", 5, OperatorType.Modulo);

            this.SymbolTable.Add(assignment, assignment);
            this.SymbolTable.Add(addition, addition);
            this.SymbolTable.Add(subtraction, subtraction);
            this.SymbolTable.Add(muiltiplication, muiltiplication);
            this.SymbolTable.Add(division, division);
            this.SymbolTable.Add(modulo, modulo);

            this.Keywords.Add(new Keyword("help", KeywordType.Help, "Type help [topic] to display information about that topic, or \"help\" to list possible topics."));
            this.Keywords.Add(new Keyword("list", KeywordType.List, "Lists all currently declared symbols and their values:  constants, variables, functions, and operators"));
            this.Keywords.Add(new Keyword("constant", KeywordType.Constant, "Declare a constant by typing \"declare constant [name] = [value]\""));
            this.Keywords.Add(new Keyword("variable", KeywordType.Variable, "Declare a variable by typing \"declare variable [name] = [value]\""));
            this.Keywords.Add(new Keyword("function", KeywordType.Function, "Declare a function by typing \"declare function [name] = [value]\""));
            this.Keywords.Add(new Keyword("operator", KeywordType.Operator, "Operators cannot be reset or re-declared. Type \"list operator\" to show list of operators"));
            this.Keywords.Add(new Keyword("declare", KeywordType.Declare, "Declare a symbol by typing \"declare [constant | variable | function] [name]\""));
            this.Keywords.Add(new Keyword("clear", KeywordType.Clear, "Clear a symbol by typing \"clear [name] \""));
            this.Keywords.Add(new Keyword("plot", KeywordType.Plot, "Plot a function by typing \"plot [function] [domain] \"."));

            // Defaults
            AssignmentOperator = "=";
            LeftParenthesis = '(';
            RightParenthesis = ')';
            LeftSquareBracket = '[';
            RightSquareBracket = ']';
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
                TerminalFontSize = configuration.Theme.TerminalFontSize,
                SecondaryForegroundColor = configuration.Theme.SecondaryForegroundColor,
                TerminalForegroundColor = configuration.Theme.TerminalForegroundColor,
                PrimaryBackgroundColor = configuration.Theme.PrimaryBackgroundColor,
                PrimaryForegroundColor = configuration.Theme.PrimaryForegroundColor,
                SecondaryBackgroundColor = configuration.Theme.SecondaryBackgroundColor,
                TitleBackgroundColor = configuration.Theme.TitleBackgroundColor,
                TitleForegroundColor = configuration.Theme.TitleForegroundColor,
            };
            this.Keywords = new List<Keyword>();

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

            // Keywords
            foreach (var keyword in configuration.Keywords)
                this.Keywords.Add(keyword);

            // Defaults
            AssignmentOperator = configuration.SymbolTable.GetAssignmentOperator()?.Symbol ?? "=";
            LeftParenthesis = '(';
            RightParenthesis = ')';
            LeftSquareBracket = '[';
            RightSquareBracket = ']';
            FunctionVariableSeparator = ',';

            AddFunctions();
            AddConstants();
        }

        public Keyword GetKeyword(string name)
        {
            return this.Keywords.FirstOrDefault(x => x.Name == name);
        }

        private void AddFunctions()
        {
        }

        private void AddConstants()
        {

        }
    }
}
