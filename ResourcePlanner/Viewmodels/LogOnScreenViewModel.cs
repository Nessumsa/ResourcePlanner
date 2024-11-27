using ResourcePlanner.Infrastructure;
using ResourcePlanner.UseCases;
using ResourcePlanner.Utilities;
using System.Windows.Input;

namespace ResourcePlanner.Viewmodels
{
    internal class LogOnScreenViewModel : Bindable
    {
        public event Action? UserLoggedIn;
        private LoginUser _loginUser;
        public ICommand LoginCMD { get; set; }
        public ICommand ForgotPasswordCMD { get; set; }

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public LogOnScreenViewModel() 
        {
            this._loginUser = new(RestApiClient.Instance);
            this.LoginCMD = new CommandRelay(Login, CanLogin);
            this.ForgotPasswordCMD = new CommandRelay(ResetPassword, CanResetPassword);
            this._username = "";
            this._password = "";
        }

        private async void Login() 
        {
            bool authenticated = await _loginUser.Execute("localhost", "5000", Username, Password);
            if (authenticated)
                UserLoggedIn?.Invoke();
        }
        private bool CanLogin() 
        {
            return !string.IsNullOrEmpty(Username) && 
                   !string.IsNullOrEmpty(Password);
        }
        public void ResetPassword() { }
        public bool CanResetPassword() 
        {
            return true;
        }
    }
}
