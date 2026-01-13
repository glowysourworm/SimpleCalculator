using SimpleCalculator.Model;

using SimpleWpf.ViewModel;

namespace SimpleCalculator.ViewModel
{
    public class CalculatorLogViewModel : ViewModelBase
    {
        string _message;
        CalculatorLogType _type;

        public string Message
        {
            get { return _message; }
            set { this.RaiseAndSetIfChanged(ref _message, value); }
        }
        public CalculatorLogType Type
        {
            get { return _type; }
            set { this.RaiseAndSetIfChanged(ref _type, value); }
        }

        public CalculatorLogViewModel()
        {
            this.Message = string.Empty;
            this.Type = CalculatorLogType.Normal;
        }
    }
}
