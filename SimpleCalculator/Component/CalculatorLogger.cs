using SimpleCalculator.Component.Interface;
using SimpleCalculator.Model;

namespace SimpleCalculator.Component
{
    public class CalculatorLogger : ICalculatorLogger
    {
        CalculatorLogDelegate _callback;

        public CalculatorLogger(CalculatorLogDelegate callback)
        {
            _callback = callback;
        }

        public void Log(string message, CalculatorLogType type)
        {
            if (_callback != null)
            {
                _callback(message, type);
            }
            else
                throw new Exception("Must register logger before using the callback");
        }

        public void RegisterLogger(CalculatorLogDelegate callback)
        {
            _callback = callback;
        }
    }
}
