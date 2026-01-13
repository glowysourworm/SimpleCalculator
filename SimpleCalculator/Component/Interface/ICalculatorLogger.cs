using SimpleCalculator.Model;

namespace SimpleCalculator.Component.Interface
{
    public delegate void CalculatorLogDelegate(string message, CalculatorLogType type);

    public interface ICalculatorLogger
    {
        /// <summary>
        /// Registers the primary callback for logging. This will be used by the backend
        /// during calculations.
        /// </summary>
        void RegisterLogger(CalculatorLogDelegate callback);

        /// <summary>
        /// Raises callback method to handle log request
        /// </summary>
        void Log(string message, CalculatorLogType type);
    }
}
