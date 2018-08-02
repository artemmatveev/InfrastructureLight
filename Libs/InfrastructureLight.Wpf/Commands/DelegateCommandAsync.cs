using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InfrastructureLight.Wpf.Commands
{
    public class DelegateCommandAsync : ICommand
    {
        private bool _isExecuting;

        private readonly Func<Task> _execute;
        private readonly Predicate<object> _canExecute;

        public DelegateCommandAsync(Func<Task> execute) : this(execute, null) { }
        public DelegateCommandAsync(Func<Task> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            if (!_isExecuting && _canExecute == null) return true;
            return (!_isExecuting && _canExecute(parameter));
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public async void Execute(object parameter)
        {
            _isExecuting = true;

            try
            {
                await _execute();
            }
            finally
            {
                _isExecuting = false;
            }
        }

        #endregion
    }
}
