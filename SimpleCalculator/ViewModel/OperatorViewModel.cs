using SimpleCalculator.Model;

using SimpleWpf.ViewModel;

namespace SimpleCalculator.ViewModel
{
    public class OperatorViewModel : ViewModelBase
    {
        string _symbol;
        uint _order;
        OperatorType _type;

        public string Symbol
        {
            get { return _symbol; }
            set { this.RaiseAndSetIfChanged(ref _symbol, value); }
        }
        public uint Order
        {
            get { return _order; }
            set { this.RaiseAndSetIfChanged(ref _order, value); }
        }
        public OperatorType Type
        {
            get { return _type; }
            set { this.RaiseAndSetIfChanged(ref _type, value); }
        }
    }
}
