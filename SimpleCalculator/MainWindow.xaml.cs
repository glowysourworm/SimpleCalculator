using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using SimpleCalculator.Component;
using SimpleCalculator.Component.Interface;
using SimpleCalculator.Extension;
using SimpleCalculator.Model;
using SimpleCalculator.Model.Calculation;
using SimpleCalculator.ViewModel;

namespace SimpleCalculator
{
    public partial class MainWindow : Window
    {
        private readonly CalculatorConfiguration _configuration;
        private readonly MainViewModel _viewModel;
        private readonly ICalculatorCore _calculator;
        private readonly ICalculatorLogger _logger;

        public MainWindow()
        {
            _configuration = new CalculatorConfiguration();
            _viewModel = new MainViewModel();
            _logger = new CalculatorLogger(Output);
            _calculator = new CalculatorCore(_configuration, _logger, new ExpressionParser(_configuration, _logger), new ExpressionFormatter(_configuration, _logger));

            // Theme
            _viewModel.Configuration.Theme.Update(_configuration.Theme);

            // Theme -> Style Bindings
            InitializeComponent();

            // Welcome Messages
            Output("Welcome to Simple Calculator!", CalculatorLogType.Normal);
            Output("Type \"help\" to display help menu", CalculatorLogType.Terminal);

            // Configuration
            _viewModel.UpdateSymbols(_configuration.SymbolTable);

            this.DataContext = _viewModel;

            // Focus on Input...
            this.InputTB.Focus();
        }

        private void Output(string message, CalculatorLogType type, params object[] arguments)
        {
            if (arguments.Length > 0)
                _viewModel.AddCodeLine(string.Format(message, arguments), type);
            else
                _viewModel.AddCodeLine(message, type);
        }

        private void InputTB_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var statement = this.InputTB.Text;

                // Statement Type
                var statementType = _calculator.CalculateStatementType(statement);

                switch (statementType)
                {
                    case StatementType.Invalid:
                        Output("Please enter command, or type \"help\" to display the help menu", CalculatorLogType.Normal);
                        break;
                    case StatementType.OperatorLed:
                        RunOperatorLedStatement(statement);
                        break;
                    case StatementType.Math:
                        RunMathStatement(statement);
                        break;
                    case StatementType.Terminal:
                        RunTerminalStatement(statement);
                        break;
                    default:
                        throw new Exception("Unhandled Statement Type");
                }
            }

            else if (e.Key == Key.Up)
            {
                this.InputTB.Text = _viewModel.PreviousStatement() ?? this.InputTB.Text;
            }
            else if (e.Key == Key.Down)
            {
                this.InputTB.Text = _viewModel.NextStatement() ?? this.InputTB.Text;
            }
        }

        private void ConfigurationMI_Click(object sender, RoutedEventArgs e)
        {
            var window = new Window();

            window.Content = new ConfigurationView();
            window.DataContext = _viewModel.Configuration;

            var decoder = PngBitmapDecoder.Create(new Uri("pack://application:,,,/Resources/Images/settings.png"), BitmapCreateOptions.None, BitmapCacheOption.Default);
            window.Icon = decoder.Frames[0];
            window.Title = "Configuration";
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.SizeToContent = SizeToContent.WidthAndHeight;
            window.Show();
        }
        private void AboutMI_Click(object sender, RoutedEventArgs e)
        {

        }

        #region (private) Run Methods
        private void RunOperatorLedStatement(string statement)
        {

        }
        private void RunMathStatement(string statement)
        {
            // Format to remove white spaces, etc... (any other syntax rules)
            //
            var formattedStatement = _calculator.FormatMathStatement(statement);

            // -> (Logger)
            var expression = _calculator.Validate(formattedStatement);

            if (expression != null)
            {
                // Evaluate (Logger) (Error messages will be logged in this call stack)
                var result = _calculator.Evaluate(expression);

                // Success!
                if (result != null)
                {
                    OutputSuccessResult(result, formattedStatement);

                    // Adds to the statement queue, updates symbols and UI
                    EndStatement(statement, true);
                }
            }
        }
        private void RunTerminalStatement(string statement)
        {
            // Pre-validated Statement (at least one keyword)
            var statementParts = statement.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            var keywords = statementParts.Select(x => _configuration.GetKeyword(x))
                                         .Where(x => x != null)
                                         .ToList();

            switch (keywords[0].Type)
            {
                case KeywordType.Help:

                    // Topic
                    if (keywords.Count > 1)
                        Output(keywords[1].HelpText, CalculatorLogType.Terminal);

                    // Help (main)
                    else
                    {
                        foreach (var keyword in _configuration.Keywords)
                            Output("{0, -15}{1, -30}", CalculatorLogType.Terminal, keyword.Name, keyword.HelpText);
                    }

                    break;

                case KeywordType.List:

                    // List [symbol]
                    if (keywords.Count > 1)
                    {
                        switch (keywords[1].Type)
                        {
                            case KeywordType.Constant:
                                foreach (var constant in _configuration.SymbolTable.Constants)
                                    Output(string.Format("Constant {0} = {1}", constant, _configuration.SymbolTable.GetValue(constant)), CalculatorLogType.Terminal);
                                break;
                            case KeywordType.Variable:
                                foreach (var variable in _configuration.SymbolTable.Variables)
                                    Output(string.Format("Variable {0} = {1}", variable, _configuration.SymbolTable.GetValue(variable)), CalculatorLogType.Terminal);
                                break;
                            case KeywordType.Function:
                                foreach (var function in _configuration.SymbolTable.Functions)
                                    Output(string.Format("Function {0}", function), CalculatorLogType.Terminal);
                                break;
                            case KeywordType.Operator:
                                foreach (var ioperator in _configuration.SymbolTable.Operators)
                                    Output(string.Format("Operator {0},  Type {1}", ioperator.Symbol, ioperator.Type), CalculatorLogType.Terminal);
                                break;
                            default:
                                Output("Please enter the type of list [constant | variable | function | operator]", CalculatorLogType.Terminal);
                                break;
                        }
                    }

                    else
                        Output("Please enter the type of list [constant | variable | function | operator]", CalculatorLogType.Terminal);

                    break;

                case KeywordType.Declare:

                    // declare [constant | variable | function] [symbol]
                    //
                    if (keywords.Count > 1 && statementParts.Length == 3)
                    {
                        // Procedure
                        //
                        // 1) Locate symbol, assignment operator, and expression
                        // 2) Check symbol table for existing symbol
                        // 3) Complete accordingly
                        //

                        string symbol = statementParts[2];

                        if (_configuration.SymbolTable.IsDefined(symbol))
                            Output("Symbol already defined. You can use \"clear\" to delete the current symbol", CalculatorLogType.Terminal);

                        else
                        {
                            switch (keywords[1].Type)
                            {
                                case KeywordType.Constant:

                                    if (!symbol.ValidateSymbol())
                                        Output("Please use appropriate syntax: declare [constant | variable | function] [symbol]", CalculatorLogType.Terminal);
                                    else
                                    {
                                        _configuration.SymbolTable.Add(new Constant(symbol), 0);
                                        Output("Constant {0} declared", CalculatorLogType.ConstantDeclaration, symbol);
                                    }


                                    break;
                                case KeywordType.Variable:

                                    if (!symbol.ValidateSymbol())
                                        Output("Please use appropriate syntax: declare [constant | variable | function] [symbol]", CalculatorLogType.Terminal);
                                    else
                                    {
                                        _configuration.SymbolTable.Add(new Variable(symbol), 0);
                                        Output("Variable {0} declared", CalculatorLogType.VariableDeclaration, symbol);
                                    }

                                    break;
                                case KeywordType.Function:

                                    if (!symbol.ValidateFunctionSymbol())
                                        Output("Please use appropriate syntax: declare [constant | variable | function] [symbol]", CalculatorLogType.Terminal);
                                    else
                                    {
                                        // Have to validate that the function variables are declared as variables; and that there
                                        // is at least one independent variable.
                                        //
                                        var signature = symbol.ReadAsFunctionSignature();

                                        if (signature == null ||
                                            signature.IndependentVariables.Count() == 0 ||
                                            signature.IndependentVariables.Any(x => !_configuration.SymbolTable.IsDefined(x)) ||
                                            signature.IndependentVariables.Any(x => _configuration.SymbolTable.Constants.Any(z => z.Symbol == x.Symbol)))
                                        {
                                            Output("Function declaration must have at least 1 independent variable, already declared, which is not a constant", CalculatorLogType.Terminal);
                                        }
                                        else
                                        {
                                            // Create Function (Default expression of "0")
                                            var function = new Function(signature, new MathExpression("0"));

                                            // Add Function to Symbol Table
                                            _configuration.SymbolTable.Add(function, function);

                                            Output("Function {0} declared", CalculatorLogType.FunctionDeclaration, function);
                                        }
                                    }

                                    break;
                                default:
                                    Output("Please use appropriate syntax: set [constant | variable | function] [symbol]", CalculatorLogType.Terminal);
                                    break;
                            }
                        }
                    }

                    else
                        Output("Please use appropriate syntax: set [constant | variable | function] [symbol]", CalculatorLogType.Terminal);

                    break;

                case KeywordType.Clear:

                    // clear [symbol]
                    //
                    if (statementParts.Length == 2)
                    {
                        // Procedure
                        //
                        // 1) Locate symbol, and dependent symbols
                        // 2) Complete accordingly
                        //

                        string symbol = statementParts[1];

                        if (!_configuration.SymbolTable.IsDefined(symbol))
                            Output("Undefined symbol. Please use \"list\" to show currently defined symbols.", CalculatorLogType.Terminal);

                        else
                        {
                            switch (_configuration.SymbolTable.GetSymbolType(symbol))
                            {
                                case SymbolType.Constant:
                                case SymbolType.Variable:
                                case SymbolType.Function:
                                    _configuration.SymbolTable.Remove(symbol);
                                    Output("Symbol {0} cleared", CalculatorLogType.Terminal, symbol);
                                    break;
                                default:
                                    Output("Cannot clear symbols other than:  constants, variables, or functions", CalculatorLogType.Terminal);
                                    break;
                            }
                        }
                    }
                    else
                        Output("Please use appropriate syntax: clear [symbol]", CalculatorLogType.Terminal);

                    break;

                case KeywordType.Constant:
                case KeywordType.Variable:
                case KeywordType.Function:
                case KeywordType.Operator:
                    Output("Please type \"help\" for how to use commands for set and declare", CalculatorLogType.Terminal);
                    break;
                case KeywordType.Plot:
                    Output("Please type \"help\" for how to use plot command", CalculatorLogType.Terminal);
                    break;
                default:
                    throw new Exception("Unhandled Keyword Type");
            }

            EndStatement(statement, true);
        }

        private void EndStatement(string statement, bool success)
        {
            // Save statement to recall using up / down arrows
            _viewModel.AddStatement(statement);

            // Clear input text
            this.InputTB.Text = string.Empty;

            // Update Symbols
            _viewModel.UpdateSymbols(_configuration.SymbolTable);
        }
        private void OutputSuccessResult(MathExpressionResult result, string inputStatement)
        {
            // Echo statement
            Output(inputStatement, CalculatorLogType.Normal);

            switch (result.Expression.Type)
            {
                // Output Value
                case MathExpressionType.Number:
                    Output(FormatNumericResult(result.NumericResult), CalculatorLogType.Result);
                    break;
                case MathExpressionType.Constant:
                case MathExpressionType.Variable:
                    Output(FormatNumericResult(result.NumericResult), CalculatorLogType.Result);
                    break;

                case MathExpressionType.Arithmetic:
                    Output(FormatNumericResult(result.NumericResult), CalculatorLogType.Result);
                    break;
                case MathExpressionType.Assignment:
                    Output("Symbol {0} set to {1}", CalculatorLogType.Result, result.Expression.Symbol.Symbol, result.Expression.RightOperand);
                    break;
                case MathExpressionType.Function:
                    Output(FormatNumericResult(result.NumericResult), CalculatorLogType.Result);
                    break;
                case MathExpressionType.Expression:
                    break;
                default:
                    throw new Exception("Unhandled Math Expression Type");
            }
        }
        private string FormatNumericResult(double number)
        {
            if (double.IsInteger(number))
                return "= " + number.ToString("N0");

            else
                return "= " + number.ToString();
        }
        #endregion
    }
}