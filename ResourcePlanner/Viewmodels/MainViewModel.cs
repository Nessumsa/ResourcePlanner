using ResourcePlanner.Views;
using System.ComponentModel;
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

        public LogOnScreen LogOnScreenControl { get; }
        public Homescreen HomescreenControl { get; }
        public Statistics StatisticsControl { get; }
        public Resources ResourcesControl { get; }
        public Users UsersControl { get; }
        public Institution InstitutionControl { get; }

        public ICommand NavigateCommand { get; }

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
                case "LogOnScreen":
                    CurrentView = LogOnScreenControl;
                    break;
                default:
                    throw new ArgumentException("Unknown view: " + destination);
            }
        }

        private void OnUserLoggedIn()
        {
            CurrentView = HomescreenControl; 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
