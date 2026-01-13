using System.Collections.ObjectModel;

using SimpleWpf.ViewModel;

namespace SimpleCalculator.ViewModel
{
    public class CalculatorConfigurationViewModel : ViewModelBase
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

        public CalculatorConfigurationViewModel()
        {
            this.Theme = new CalculatorThemeViewModel();
            this.Constants = new ObservableCollection<ConstantViewModel>();
            this.Variables = new ObservableCollection<VariableViewModel>();
            this.Functions = new ObservableCollection<FunctionViewModel>();
            this.Operators = new ObservableCollection<OperatorViewModel>();
        }
    }
}
