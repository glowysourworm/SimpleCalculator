using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using SimpleCalculator.Component;
using SimpleCalculator.Component.Interface;
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

        private void Output(string message, CalculatorLogType type)
        {
            _viewModel.AddCodeLine(message, type);
        }

        private void InputTB_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var statement = this.InputTB.Text;
                var formattedStatement = _calculator.Format(statement);

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

                        // Save statement to recall using up / down arrows
                        _viewModel.AddStatement(statement);

                        // Clear input text
                        this.InputTB.Text = string.Empty;

                        // Update Symbols
                        _viewModel.UpdateSymbols(_configuration.SymbolTable);
                    }
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

        private void OutputSuccessResult(MathExpressionResult result, string inputStatement)
        {
            // Echo statement
            Output(inputStatement, CalculatorLogType.Normal);

            switch (result.OperationType)
            {
                // Output Value
                case MathExpressionType.Number:
                    Output(FormatNumericResult(result.NumericResult), CalculatorLogType.Result);
                    break;
                case MathExpressionType.Constant:
                case MathExpressionType.Variable:
                    //_viewModel.AddCodeLine(FormatNumericResult(result.NumericResult), false, true);
                    break;

                case MathExpressionType.Arithmetic:
                    Output(FormatNumericResult(result.NumericResult), CalculatorLogType.Result);
                    break;
                case MathExpressionType.Assignment:
                    //_viewModel.AddCodeLine(FormatNumericResult(result.NumericResult), false, true);
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
    }
}