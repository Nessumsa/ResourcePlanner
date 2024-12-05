using System.Windows.Input;

namespace ResourcePlanner.Utilities
{
    public class RelayCommand : ICommand
    {
        private Action _execute;
        private Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute();
        }

        public void Execute(object? parameter)
        {
            _execute();
        }

        public void UpdateCommand(Action newExecute, Func<bool> newCanExecute)
        {
            _execute = newExecute;
            _canExecute = newCanExecute;
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
