using System.Windows;
using System.Windows.Input;

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
            _logger = new CalculatorLogger(LogMessage);
            _calculator = new CalculatorCore(_configuration, _logger, new ExpressionParser(_configuration, _logger), new ExpressionFormatter(_configuration));

            // Theme
            _viewModel.Theme.Update(_configuration.Theme);

            // Theme -> Style Bindings
            InitializeComponent();

            // Welcome Messages
            _viewModel.AddCodeLine("Welcome to Simple Calculator!");

            // Configuration
            _viewModel.UpdateSymbols(_configuration.SymbolTable);

            this.DataContext = _viewModel;

            // Focus on Input...
            this.InputTB.Focus();
        }

        private void LogMessage(string message, bool isError)
        {
            _viewModel.AddCodeLine(message, isError, false);
        }

        private void InputTB_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var statement = this.InputTB.Text;
                var formattedStatement = _calculator.Format(statement);

                string? errorMessage = null;
                var expression = _calculator.Validate(formattedStatement, out errorMessage);

                if (errorMessage != null)
                {
                    _viewModel.AddCodeLine(errorMessage, true);
                }
                else if (expression == null)
                {
                    _viewModel.AddCodeLine("Unknown error evaluating math expression", true);
                }
                else
                {
                    // Evaluate
                    var result = _calculator.Evaluate(expression);

                    // Error
                    if (result.ErrorMessage != null)
                        _viewModel.AddCodeLine(result.ErrorMessage, true);

                    // Success!
                    else
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
            _viewModel.AddCodeLine(inputStatement, false);

            switch (result.OperationType)
            {
                // Output Value
                case MathExpressionType.Number:
                    _viewModel.AddCodeLine(FormatNumericResult(result.NumericResult), false, true);
                    break;
                case MathExpressionType.Constant:
                case MathExpressionType.Variable:
                    //_viewModel.AddCodeLine(FormatNumericResult(result.NumericResult), false, true);
                    break;

                case MathExpressionType.Arithmetic:
                    _viewModel.AddCodeLine(FormatNumericResult(result.NumericResult), false, true);
                    break;
                case MathExpressionType.Assignment:
                    //_viewModel.AddCodeLine(FormatNumericResult(result.NumericResult), false, true);
                    break;
                case MathExpressionType.Function:
                    _viewModel.AddCodeLine(FormatNumericResult(result.NumericResult), false, true);
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
    }
}