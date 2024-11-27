using System.Windows.Input;

namespace ResourcePlanner.Utilities
{
    public class CommandRelay : ICommand
    {
        private Action _execute;
        private Func<bool> _canExecute;

        public CommandRelay(Action execute, Func<bool> canExecute)
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
    }
}
