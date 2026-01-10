using System.Collections.ObjectModel;

using SimpleCalculator.Model;

using SimpleWpf.ViewModel;

namespace SimpleCalculator.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<OperatorViewModel> Operators { get; private set; }
        public ObservableCollection<LogMessageViewModel> OutputLog { get; private set; }

        public MainViewModel()
        {
            this.Operators = new ObservableCollection<OperatorViewModel>();
            this.OutputLog = new ObservableCollection<LogMessageViewModel>();
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
