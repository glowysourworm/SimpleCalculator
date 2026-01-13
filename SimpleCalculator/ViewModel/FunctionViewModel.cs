using System.Collections.ObjectModel;

using SimpleWpf.ViewModel;

namespace SimpleCalculator.ViewModel
{
    public class FunctionViewModel : ViewModelBase
    {
        string _expression;
        bool _isValid;
        ObservableCollection<ConstantViewModel> _constants;
        ObservableCollection<VariableViewModel> _variables;

        public string Expression
        {
            get { return _expression; }
            set { this.RaiseAndSetIfChanged(ref _expression, value); }
        }
        public bool IsValid
        {
            get { return _isValid; }
            set { this.RaiseAndSetIfChanged(ref _isValid, value); }
        }
        public ObservableCollection<ConstantViewModel> Constants
        {
            get { return _constants; }
            set { this.RaiseAndSetIfChanged(ref _constants, value); }
        }
        public ObservableCollection<VariableViewModel> Variables
        {
            get { return _variables; }
            set { this.RaiseAndSetIfChanged(ref _variables, value); }
        }

        public FunctionViewModel()
        {
            this.Expression = string.Empty;
            this.IsValid = false;
            this.Constants = new ObservableCollection<ConstantViewModel>();
            this.Variables = new ObservableCollection<VariableViewModel>();
        }
    }
}
