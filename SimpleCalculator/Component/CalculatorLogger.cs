using SimpleCalculator.Component.Interface;

namespace SimpleCalculator.Component
{
    public class CalculatorLogger : ICalculatorLogger
    {
        Action<string, bool> _callback;

        public CalculatorLogger(Action<string, bool> callback)
        {
            _callback = callback;
        }

        public void Log(string message, bool isError)
        {
            if (_callback != null)
            {
                _callback(message, isError);
            }
            else
                throw new Exception("Must register logger before using the callback");
        }

        public void RegisterLogger(Action<string, bool> callback)
        {
            _callback = callback;
        }
    }
}
