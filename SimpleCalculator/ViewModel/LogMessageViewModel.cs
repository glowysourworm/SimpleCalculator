using SimpleWpf.ViewModel;

namespace SimpleCalculator.ViewModel
{
    public class LogMessageViewModel : ViewModelBase
    {
        string _message;
        bool _isError;
        bool _isAnswer;

        public string Message
        {
            get { return _message; }
            set { this.RaiseAndSetIfChanged(ref _message, value); }
        }
        public bool IsError
        {
            get { return _isError; }
            set { this.RaiseAndSetIfChanged(ref _isError, value); }
        }
        public bool IsAnswer
        {
            get { return _isAnswer; }
            set { this.RaiseAndSetIfChanged(ref _isAnswer, value); }
        }

        public LogMessageViewModel()
        {
            this.Message = string.Empty;
            this.IsError = false;
        }
    }
}
