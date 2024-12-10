using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ResourcePlanner.Utilities.MVVM
{
    /// <summary>
    /// A base class that implements the INotifyPropertyChanged interface to enable data binding in MVVM.
    /// </summary>
    public class Bindable : INotifyPropertyChanged
    {
        /// <summary>
        /// Event that is raised when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event to notify listeners that a property value has changed.
        /// </summary>
        /// <param name="name">
        /// The name of the property that changed. 
        /// This is automatically provided by the CallerMemberName attribute if not specified.
        /// </param>
        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}