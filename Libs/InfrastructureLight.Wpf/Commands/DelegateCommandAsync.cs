using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InfrastructureLight.Wpf.Commands
{
    public class DelegateCommandAsync : ICommand
    {
        private bool isExecuting;

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
            if (!isExecuting && _canExecute == null) return true;
            return (!isExecuting && _canExecute(parameter));
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public async void Execute(object parameter)
        {
            isExecuting = true;

            try
            {
                await _execute();
            }
            finally
            {
                isExecuting = false;
            }
        }

        #endregion
    }
}
