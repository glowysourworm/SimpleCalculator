using System.Windows.Media;

using SimpleCalculator.Model;

using SimpleWpf.ViewModel;

namespace SimpleCalculator.ViewModel
{
    public class CalculatorThemeViewModel : ViewModelBase
    {
        Brush _titleForegroundColor;
        Brush _titleBackgroundColor;
        Brush _codeForegroundColor;
        Brush _codeErrorForegroundColor;
        Brush _primaryBackgroundColor;
        Brush _primaryForegroundColor;
        Brush _secondaryBackgroundColor;

        public Brush TitleForegroundColor
        {
            get { return _titleForegroundColor; }
            set { this.RaiseAndSetIfChanged(ref _titleForegroundColor, value); }
        }
        public Brush TitleBackgroundColor
        {
            get { return _titleBackgroundColor; }
            set { this.RaiseAndSetIfChanged(ref _titleBackgroundColor, value); }
        }
        public Brush CodeForegroundColor
        {
            get { return _codeForegroundColor; }
            set { this.RaiseAndSetIfChanged(ref _codeForegroundColor, value); }
        }
        public Brush CodeErrorForegroundColor
        {
            get { return _codeErrorForegroundColor; }
            set { this.RaiseAndSetIfChanged(ref _codeErrorForegroundColor, value); }
        }
        public Brush PrimaryBackgroundColor
        {
            get { return _primaryBackgroundColor; }
            set { this.RaiseAndSetIfChanged(ref _primaryBackgroundColor, value); }
        }
        public Brush PrimaryForegroundColor
        {
            get { return _primaryForegroundColor; }
            set { this.RaiseAndSetIfChanged(ref _primaryForegroundColor, value); }
        }
        public Brush SecondaryBackgroundColor
        {
            get { return _secondaryBackgroundColor; }
            set { this.RaiseAndSetIfChanged(ref _secondaryBackgroundColor, value); }
        }

        public CalculatorThemeViewModel()
        {
            this.TitleBackgroundColor = Brushes.White;
            this.TitleForegroundColor = Brushes.Black;
            this.CodeForegroundColor = Brushes.Black;
            this.CodeErrorForegroundColor = Brushes.Red;
            this.PrimaryBackgroundColor = Brushes.White;
            this.PrimaryForegroundColor = Brushes.Black;
            this.SecondaryBackgroundColor = Brushes.White;
        }

        public CalculatorThemeViewModel(CalculatorTheme theme)
        {
            Update(theme);
        }

        public void Update(CalculatorTheme theme)
        {
            this.TitleForegroundColor = new SolidColorBrush(theme.TitleForegroundColor);
            this.TitleBackgroundColor = new SolidColorBrush(theme.TitleBackgroundColor);
            this.PrimaryBackgroundColor = new SolidColorBrush(theme.PrimaryBackgroundColor);
            this.PrimaryForegroundColor = new SolidColorBrush(theme.PrimaryForegroundColor);
            this.SecondaryBackgroundColor = new SolidColorBrush(theme.SecondaryBackgroundColor);
            this.CodeErrorForegroundColor = new SolidColorBrush(theme.CodeForegroundColor);
            this.CodeErrorForegroundColor = new SolidColorBrush(theme.CodeErrorForegroundColor);
        }
    }
}
