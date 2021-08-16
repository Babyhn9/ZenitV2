

using System;
using System.Windows.Input;

namespace ZenitV2.MVVMCore
{
    public class Command<T> : ICommand
    {
        private Func<T, bool> _canExecute;
        private Action<T> _exectute;

        public Command(Action<T> execute, Func<T, bool> canExecute = null) 
        {
            _exectute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute((T)parameter);


        public void Execute(object parameter) => _exectute((T)parameter);
       
    }

    public class Command : ICommand
    {
        private Func<object, bool> _canExecute;
        private Action<object> _exectute;


        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public Command(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _exectute = execute;
            _canExecute = canExecute;
        }
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        public void Execute(object parameter) => _exectute(parameter);
    }
}
