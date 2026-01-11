namespace SimpleCalculator.Component.Interface
{
    public interface ICalculatorLogger
    {
        /// <summary>
        /// Registers the primary callback for logging. This will be used by the backend
        /// during calculations.
        /// </summary>
        void RegisterLogger(Action<string, bool> callback);

        /// <summary>
        /// Raises callback method to handle log request
        /// </summary>
        void Log(string message, bool isError);
    }
}
