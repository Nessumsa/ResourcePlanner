using ResourcePlanner.Infrastructure.Managers;
using ResourcePlanner.UseCases;
using ResourcePlanner.Utilities.MVVM;
using System.Windows.Input;

namespace ResourcePlanner.Viewmodels
{
    /// <summary>
    /// ViewModel for the login screen, managing user login and password reset functionalities.
    /// </summary>
    public class LogOnScreenViewModel : Bindable
    {
        /// Event triggered when a user successfully logs in.
        public static event Action? UserLoggedIn;

        // Handler for the use case for user login.
        private LoginUser _loginUser;

        /// Command to attempt logging in.
        public ICommand LoginCMD { get; set; }

        /// Command to initiate password reset.
        public ICommand ForgotPasswordCMD { get; set; }

        // Backing fields for the Username and Password properties.
        private string _username;
        private string _password;

        /// The username entered by the user.
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        /// The password entered by the user.
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Initializes a new instance of the LogOnScreenViewModel class.
        /// </summary>
        public LogOnScreenViewModel()
        {
            // Initialize the login use case with the API client instance.
            this._loginUser = new(RestApiClient.Instance);

            // Define commands with their respective methods and conditions.
            this.LoginCMD = new RelayCommand(Login, CanLogin);
            this.ForgotPasswordCMD = new RelayCommand(ResetPassword, CanResetPassword);

            this._username = string.Empty;
            this._password = string.Empty;
        }

        /// <summary>
        /// Attempts to log in using the provided username and password.
        /// </summary>
        private async void Login()
        {
            // Authenticate the user using the login use case.
            bool authenticated = await _loginUser.Execute("localhost", "5000", Username, Password);

            // Trigger the UserLoggedIn event if authentication is successful.
            if (authenticated)
                UserLoggedIn?.Invoke();
        }

        /// <summary>
        /// Determines if the LoginCMD can execute.
        /// </summary>
        /// <returns>True if both username and password are not empty; otherwise, false.</returns>
        private bool CanLogin()
        {
            return !string.IsNullOrEmpty(Username) &&
                   !string.IsNullOrEmpty(Password);
        }

        /// <summary>
        /// Initiates the password reset process. Currently not implemented.
        /// </summary>
        public void ResetPassword()
        {
            // Placeholder for password reset functionality.
        }

        /// <summary>
        /// Determines if the ForgotPasswordCMD can execute.
        /// </summary>
        /// <returns>True since password reset is always available (currently not conditional).</returns>
        public bool CanResetPassword()
        {
            return true;
        }
    }
}