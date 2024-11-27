using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ResourcePlanner.Utilities
{
    public class Bindable
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
