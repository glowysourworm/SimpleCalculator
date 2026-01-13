using System.Windows;
using System.Windows.Controls;

using SimpleCalculator.Model;
using SimpleCalculator.ViewModel;

namespace SimpleCalculator.TemplateSelector
{
    public class CodeStyleSelector : StyleSelector
    {
        public CodeStyleSelector()
        {
        }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            var log = item as CalculatorLogViewModel;

            if (log == null)
                throw new Exception("CodeDataTemplateSelector selects templates for CalculatorLogViewModel types only");

            switch (log.Type)
            {
                case CalculatorLogType.Normal:
                    return App.Current.Resources["CodeNormalStyle"] as Style;
                case CalculatorLogType.ParseError:
                    return App.Current.Resources["CodeParseErrorStyle"] as Style;
                case CalculatorLogType.SyntaxError:
                    return App.Current.Resources["CodeSyntaxErrorStyle"] as Style;
                case CalculatorLogType.ConstantDeclaration:
                case CalculatorLogType.VariableDeclaration:
                case CalculatorLogType.FunctionDeclaration:
                    return App.Current.Resources["CodeDeclarationStyle"] as Style;
                case CalculatorLogType.IllegalDeclaration:
                    return App.Current.Resources["CodeIllegalDeclarationStyle"] as Style;
                case CalculatorLogType.Terminal:
                    return App.Current.Resources["CodeTerminalStyle"] as Style;
                case CalculatorLogType.Result:
                    return App.Current.Resources["CodeResultStyle"] as Style;
                case CalculatorLogType.DivideByZero:
                    return App.Current.Resources["CodeDivideByZeroStyle"] as Style;
                default:
                    throw new Exception("Unhandled Calculator Log Type");
            }

            return base.SelectStyle(item, container);
        }
    }
}
