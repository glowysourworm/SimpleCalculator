using System.Windows;
using System.Windows.Input;

using SimpleCalculator.Component;
using SimpleCalculator.Component.Interface;
using SimpleCalculator.Model;
using SimpleCalculator.Model.Calculation;
using SimpleCalculator.ViewModel;

using MathExpression = SimpleCalculator.Model.MathExpression.Expression;

namespace SimpleCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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

        private void InputTB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var codeLine = this.InputTB.Text;
                var expression = new MathExpression(codeLine);

                var errorMessage = _calculator.Validate(codeLine);

                if (errorMessage != null)
                {
                    _viewModel.AddCodeLine(errorMessage, true);
                }
                else
                {
                    // Expand
                    var semanticTree = _calculator.Expand(codeLine);

                    // Evaluate
                    var result = _calculator.Evaluate(semanticTree);

                    // Error
                    if (result.Status != SemanticTreeResultStatus.Success)
                        _viewModel.AddCodeLine(result.Message, true);

                    else
                    {
                        _viewModel.AddCodeLine(codeLine, false);
                        _viewModel.AddCodeLine(FormatNumericResult(result), false, true);
                    }

                    this.InputTB.Text = string.Empty;
                }
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