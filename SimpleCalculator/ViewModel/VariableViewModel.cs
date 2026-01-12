using SimpleWpf.ViewModel;

namespace SimpleCalculator.ViewModel
{
    public class VariableViewModel : ViewModelBase
    {
        string _symbol;
        double _value;

        public string Symbol
        {
            get { return _symbol; }
            set { this.RaiseAndSetIfChanged(ref _symbol, value); }
        }
        public double Value
        {
            get { return _value; }
            set { this.RaiseAndSetIfChanged(ref _value, value); }
        }

        public VariableViewModel()
        {
            this.Symbol = string.Empty;
            this.Value = 0;
        }
    }
}
