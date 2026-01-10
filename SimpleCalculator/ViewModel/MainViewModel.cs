using System.Collections.ObjectModel;

using SimpleCalculator.Model;

using SimpleWpf.ViewModel;

namespace SimpleCalculator.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<OperatorViewModel> Operators { get; private set; }
        public ObservableCollection<LogMessageViewModel> OutputLog { get; private set; }
        public ObservableCollection<string> StatementLog { get; private set; }

        private int _statementCursor;

        public MainViewModel()
        {
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

        public void AddOperator(Operator ioperator)
        {
            this.Operators.Add(new OperatorViewModel()
            {
                Type = ioperator.Type,
                Order = ioperator.Order,
                Symbol = ioperator.Symbol
            });
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
