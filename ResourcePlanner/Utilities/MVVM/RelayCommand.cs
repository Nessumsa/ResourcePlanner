using System.Windows.Input;

namespace ResourcePlanner.Utilities.MVVM
{
    /// <summary>
    /// A command implementation for use in MVVM 
    /// that relays execution logic and state to delegate methods.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private Action _execute;
        private Func<bool> _canExecute;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="execute">The action to execute when the command is invoked.</param>
        /// <param name="canExecute">A function that determines whether the command can execute.</param>
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Occurs when changes affect whether the command should execute.
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Determines whether the command can execute in its current state.
        /// </summary>
        /// <returns>True if the command can execute; otherwise, false.</returns>
        public bool CanExecute(object? parameter)
        {
            return _canExecute();
        }

        /// <summary>
        /// Executes the command logic.
        /// </summary>
        public void Execute(object? parameter)
        {
            _execute();
        }

        /// <summary>
        /// Updates the command's execution logic and state evaluation logic.
        /// </summary>
        /// <param name="newExecute">The new action to execute when the command is invoked.</param>
        /// <param name="newCanExecute">The new function that determines whether the command can execute.</param>
        public void UpdateCommand(Action newExecute, Func<bool> newCanExecute)
        {
            _execute = newExecute;
            _canExecute = newCanExecute;
            CommandManager.InvalidateRequerySuggested();
        }
    }
}