using ResourcePlanner.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ResourcePlanner.Viewmodels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private UserControl _currentView;

        public UserControl CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public ICommand NavigateCommand { get; }

        public MainViewModel()
        {
            NavigateCommand = new RelayCommandNew(ExecuteNavigation);
            CurrentView = new LogOnScreen(); // Default view
        }

        private void ExecuteNavigation(object parameter)
        {
            string destination = parameter as string;

            switch (destination)
            {
                case "Homescreen":
                    CurrentView = new Homescreen();
                    break;
                case "Statistics":
                    CurrentView = new Statistics();
                    break;
                case "Resources":
                    CurrentView = new Resources();
                    break;
                case "Users":
                    CurrentView = new Users();
                    break;
                case "Institution":
                    CurrentView = new Institution();
                    break;
                case "LogOnScreen":
                    CurrentView = new LogOnScreen();
                    break;
                default:
                    throw new ArgumentException("Unknown view: " + destination);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
