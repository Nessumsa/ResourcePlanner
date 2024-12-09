using ResourcePlanner.Utilities;
using ResourcePlanner.Views;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace ResourcePlanner.Viewmodels
{
    public class MainViewModel : Bindable
    {
        private UserControl _currentView;

        public UserControl CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        private bool _loginVisibility = true;
        public bool LoginVisibility
        {
            get => _loginVisibility;
            set
            {
                _loginVisibility = value;
                OnPropertyChanged();
            }
        }
        private bool _loggedIn = false;
        public bool LoggedIn
        {
            get => _loggedIn;
            set
            {
                _loggedIn = value;
                OnPropertyChanged();
            }
        }

        public UserControl LogOnScreenControl { get; }
        public UserControl HomescreenControl { get; }
        public UserControl StatisticsControl { get; }
        public UserControl ResourcesControl { get; }
        public UserControl UsersControl { get; }
        public UserControl InstitutionControl { get; }
        public ICommand NavigateCommand { get; }

        public void ChangeBool()
        {
            LoggedIn = true;
            LoginVisibility = false;
        }

        public MainViewModel()
        {
            LogOnScreenControl = new LogOnScreen();
            HomescreenControl = new Homescreen();
            StatisticsControl = new Statistics();
            ResourcesControl = new Resources();
            UsersControl = new Users();
            InstitutionControl = new Institution();

            CurrentView = LogOnScreenControl;

            NavigateCommand = new RelayCommandNew(ExecuteNavigation);

            LogOnScreenViewModel.UserLoggedIn += ChangeBool;

            LogOnScreenViewModel.UserLoggedIn += OnUserLoggedIn;
        }

        private void ExecuteNavigation(object parameter)
        {
            string destination = parameter as string;

            switch (destination)
            {
                case "Homescreen":
                    CurrentView = HomescreenControl;
                    break;
                case "Statistics":
                    CurrentView = StatisticsControl;
                    break;
                case "Resources":
                    CurrentView = ResourcesControl;
                    break;
                case "Users":
                    CurrentView = UsersControl;
                    break;
                case "Institution":
                    CurrentView = InstitutionControl;
                    break;
                case "LogOn":
                    CurrentView = LogOnScreenControl;
                    break;
                default:
                    throw new ArgumentException("Unknown view: " + destination);
            }
        }

        private bool CanNavigatge()
        {
            return true; 
        }

        private void OnUserLoggedIn()
        {
            CurrentView = HomescreenControl; 
        }
    }
}
