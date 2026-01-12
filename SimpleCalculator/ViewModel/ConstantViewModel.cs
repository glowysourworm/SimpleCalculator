using SimpleWpf.ViewModel;

namespace SimpleCalculator.ViewModel
{
    public class ConstantViewModel : ViewModelBase
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

        public ConstantViewModel()
        {
            this.Symbol = string.Empty;
            this.Value = 0;
        }
    }
}
