
using System.Windows.Input;

namespace Workers.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action _executeNoParam;
        private readonly Action<object> _executeWithParam;
        private readonly Func<bool> _canExecuteNoParam;
        private readonly Func<object, bool> _canExecuteWithParam;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _executeNoParam = execute;
            _canExecuteNoParam = canExecute;
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _executeWithParam = execute;
            _canExecuteWithParam = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecuteNoParam != null)
            {
                return _canExecuteNoParam();
            }
            return _canExecuteWithParam == null || _canExecuteWithParam(parameter);
        }

        public void Execute(object parameter)
        {
            if (_executeNoParam != null)
            {
                _executeNoParam();
            }
            else
            {
                _executeWithParam(parameter);
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

