using System;
using System.Windows.Input;

namespace Warfare
{
    public class DefaultCommand : ICommand
    {
        private Action _execute;
        public event EventHandler CanExecuteChanged;

        public DefaultCommand(Action execute) => _execute = execute;
        public void Execute(object parameter) => _execute();
        public bool CanExecute(object parameter) => true;
    }

    public class DefaultCommand<T> : ICommand
    {
        private Action<T> _execute;
        public event EventHandler CanExecuteChanged;

        public DefaultCommand(Action<T> execute) => _execute = execute;
        public void Execute(object parameter) => _execute((T)parameter);
        public bool CanExecute(object parameter) => true;
    }
}
