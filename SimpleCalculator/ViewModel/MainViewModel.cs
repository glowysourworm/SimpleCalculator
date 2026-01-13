using System.Collections.ObjectModel;

using SimpleCalculator.Model;

using SimpleWpf.ViewModel;

namespace SimpleCalculator.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public CalculatorConfigurationViewModel Configuration { get; private set; }

        public ObservableCollection<CalculatorLogViewModel> OutputLog { get; private set; }
        public ObservableCollection<string> StatementLog { get; private set; }

        private int _statementCursor;

        public MainViewModel()
        {
            this.Configuration = new CalculatorConfigurationViewModel();
            this.OutputLog = new ObservableCollection<CalculatorLogViewModel>();
            this.StatementLog = new ObservableCollection<string>();

            _statementCursor = -1;
        }

        public void AddStatement(string statement)
        {
            // Insert at the front
            this.StatementLog.Insert(0, statement);

            _statementCursor = -1;
        }

        public string? PreviousStatement()
        {
            if (!this.StatementLog.Any())
                return null;

            else if (_statementCursor == -1)
                return this.StatementLog[++_statementCursor];

            else if (_statementCursor < this.StatementLog.Count - 1)
                return this.StatementLog[++_statementCursor];

            else
                return this.StatementLog.Last();
        }

        public string? NextStatement()
        {
            if (!this.StatementLog.Any())
                return null;

            else if (_statementCursor == -1)
                return this.StatementLog[++_statementCursor];

            else if (_statementCursor > 0)
                return this.StatementLog[--_statementCursor];

            else
                return this.StatementLog.First();
        }

        public void UpdateSymbols(SymbolTable table)
        {
            this.Configuration.Operators.Clear();
            this.Configuration.Constants.Clear();
            this.Configuration.Variables.Clear();
            this.Configuration.Functions.Clear();

            foreach (var ioperator in table.Operators)
            {
                this.Configuration.Operators.Add(new OperatorViewModel()
                {
                    Type = ioperator.Type,
                    Order = ioperator.Order,
                    Symbol = ioperator.Symbol
                });
            }

            foreach (var constant in table.Constants)
            {
                this.Configuration.Constants.Add(new ConstantViewModel()
                {
                    Symbol = constant.Symbol,
                    Value = table.GetValue(constant)
                });
            }

            foreach (var variable in table.Variables)
            {
                this.Configuration.Variables.Add(new VariableViewModel()
                {
                    Symbol = variable.Symbol,
                    Value = table.GetValue(variable)
                });
            }

            foreach (var function in table.Functions)
            {
                this.Configuration.Functions.Add(new FunctionViewModel()
                {
                    Expression = function.ToString()
                });
            }
        }
        public void AddCodeLine(string line, CalculatorLogType type)
        {
            this.OutputLog.Add(new CalculatorLogViewModel()
            {
                Message = line,
                Type = type
            });
        }
    }
}
