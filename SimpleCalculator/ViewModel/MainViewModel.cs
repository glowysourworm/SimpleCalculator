using System.Collections.ObjectModel;

using SimpleCalculator.Model;

using SimpleWpf.ViewModel;

namespace SimpleCalculator.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        CalculatorThemeViewModel _theme;

        public CalculatorThemeViewModel Theme
        {
            get { return _theme; }
            set { this.RaiseAndSetIfChanged(ref _theme, value); }
        }

        public ObservableCollection<ConstantViewModel> Constants { get; private set; }
        public ObservableCollection<VariableViewModel> Variables { get; private set; }
        public ObservableCollection<FunctionViewModel> Functions { get; private set; }
        public ObservableCollection<OperatorViewModel> Operators { get; private set; }

        public ObservableCollection<LogMessageViewModel> OutputLog { get; private set; }
        public ObservableCollection<string> StatementLog { get; private set; }

        private int _statementCursor;

        public MainViewModel()
        {
            this.Theme = new CalculatorThemeViewModel();
            this.Constants = new ObservableCollection<ConstantViewModel>();
            this.Variables = new ObservableCollection<VariableViewModel>();
            this.Functions = new ObservableCollection<FunctionViewModel>();
            this.Operators = new ObservableCollection<OperatorViewModel>();
            this.OutputLog = new ObservableCollection<LogMessageViewModel>();
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
            this.Operators.Clear();
            this.Constants.Clear();
            this.Variables.Clear();
            this.Functions.Clear();

            foreach (var ioperator in table.Operators)
            {
                this.Operators.Add(new OperatorViewModel()
                {
                    Type = ioperator.Type,
                    Order = ioperator.Order,
                    Symbol = ioperator.Symbol
                });
            }

            foreach (var constant in table.Constants)
            {
                this.Constants.Add(new ConstantViewModel()
                {
                    Symbol = constant.Symbol,
                    Value = table.GetValue(constant)
                });
            }

            foreach (var variable in table.Variables)
            {
                this.Variables.Add(new VariableViewModel()
                {
                    Symbol = variable.Symbol,
                    Value = table.GetValue(variable)
                });
            }

            foreach (var function in table.Functions)
            {
                this.Functions.Add(new FunctionViewModel()
                {
                    Expression = function.ToString()
                });
            }
        }
        public void AddCodeLine(string line, bool isError = false, bool isAnswer = false)
        {
            this.OutputLog.Add(new LogMessageViewModel()
            {
                IsError = isError,
                Message = line,
                IsAnswer = isAnswer
            });
        }
    }
}
