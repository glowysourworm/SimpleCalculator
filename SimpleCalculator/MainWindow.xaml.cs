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

        public MainWindow()
        {
            _configuration = new CalculatorConfiguration();
            _viewModel = new MainViewModel();
            _calculator = new CalculatorCore(_configuration, new ExpressionParser(_configuration), new ExpressionFormatter(_configuration));

            InitializeComponent();

            // Welcome Messages
            _viewModel.AddCodeLine("Welcome to Simple Calculator!");

            // Configuration
            foreach (var oper in _configuration.SymbolTable.Operators)
            {
                _viewModel.AddOperator(oper);
            }

            this.DataContext = _viewModel;
        }

        private void InputTB_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var statement = this.InputTB.Text;
                var formattedStatement = _calculator.Format(statement);

                var errorMessage = _calculator.Validate(formattedStatement);

                if (errorMessage != null)
                {
                    _viewModel.AddCodeLine(errorMessage, true);
                }
                else
                {
                    // Expand
                    var semanticTree = _calculator.Expand(formattedStatement);

                    // Evaluate
                    var result = _calculator.Evaluate(semanticTree);

                    // Error
                    if (result.Status != SemanticTreeResultStatus.Success)
                        _viewModel.AddCodeLine(result.Message, true);

                    // Success!
                    else
                    {
                        _viewModel.AddCodeLine(formattedStatement, false);
                        _viewModel.AddCodeLine(FormatNumericResult(result), false, true);

                        // Save statement to recall using up / down arrows
                        _viewModel.AddStatement(statement);

                        // Clear input text
                        this.InputTB.Text = string.Empty;
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

        private string FormatNumericResult(SemanticTreeResult result)
        {
            if (double.IsInteger(result.NumericResult))
                return "= " + result.NumericResult.ToString("N0");

            else
                return "= " + result.NumericResult.ToString();
        }
    }
}