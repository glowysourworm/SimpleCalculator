using SimpleWpf.ViewModel;

namespace SimpleCalculator.ViewModel
{
    public class VariableViewModel : ViewModelBase
    {
        char _symbol;

        public char Symbol
        {
            get { return _symbol; }
            set { this.RaiseAndSetIfChanged(ref _symbol, value); }
        }

        public VariableViewModel(char symbol)
        {
            this.Symbol = symbol;
        }
    }
}
