using ResourcePlanner.Domain;
using ResourcePlanner.Infrastructure.Adapters;
using ResourcePlanner.Infrastructure.Managers;
using ResourcePlanner.UseCases;
using ResourcePlanner.Utilities.MVVM;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ResourcePlanner.Viewmodels
{
    /// <summary>
    /// ViewModel for managing users within the application.
    /// Provides functionality to create, update, delete, and list users.
    /// </summary>
    class UsersViewModel : Bindable
    {
        // Handler for user-related operations.
        private UserHandler? _userHandler;

        /// Command for creating a new user or resetting the form for new user creation.
        public ICommand MakeNewCMD { get; }

        /// Command for deleting a selected user.
        public ICommand DeleteCMD { get; }

        /// Command for saving user details, either by creating or updating.
        public RelayCommand SaveCMD { get; private set; }

        // Observable collection of users.
        private ObservableCollection<User> _userList;

        /// List of users displayed in the UI.
        public ObservableCollection<User> Userlist
        {
            get { return _userList; }
            set
            {
                _userList = value;
                OnPropertyChanged();
            }
        }

        // Currently selected user in the UI.
        private User? _selectedUser;

        /// The currently selected user.
        public User? SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
                PopulateUserProfile();
                SaveCMD.UpdateCommand(Update, IsUserSelected);
            }
        }

        // Fields for user input.
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                OnPropertyChanged();
            }
        }

        /// Predefined roles a user can have.
        public string[] Roles { get; } = { "admin", "user" };

        private string _selectedRole;

        /// The role selected for the user.
        public string SelectedRole
        {
            get { return _selectedRole; }
            set
            {
                _selectedRole = value;
                OnPropertyChanged();
            }
        }

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

        /// <summary>
        /// Constructor initializes commands and resets user details.
        /// </summary>
        public UsersViewModel()
        {
            this.MakeNewCMD = new RelayCommand(ResetForm, IsUserSelected);
            this.DeleteCMD = new RelayCommand(Delete, IsUserSelected);
            this.SaveCMD = new RelayCommand(Create, CanCreate);

            this._userList = new ObservableCollection<User>();

            ResetFields();

            // Subscribe to the UserLoggedIn event to initialize the view.
            LogOnScreenViewModel.UserLoggedIn += InitView;
        }

        /// <summary>
        /// Resets the form and prepares for creating a new user.
        /// </summary>
        private void ResetForm()
        {
            ResetFields();
            SelectedUser = null;
            SaveCMD.UpdateCommand(Create, CanCreate);
        }

        /// <summary>
        /// Creates a new user based on the form inputs.
        /// </summary>
        private async void Create()
        {
            if (UserManager.Instance.InstitutionId == null || _userHandler == null)
                return;

            var user = new User(Name, Email, Phone, SelectedRole, Username, Password, UserManager.Instance.InstitutionId);
            bool userCreated = await _userHandler.CreateUser(user);
            if (userCreated)
            {
                ResetFields();
                SelectedUser = null;
                await PopulateUserList();
            }
        }

        /// <summary>
        /// Determines whether the create command can execute.
        /// </summary>
        private bool CanCreate()
        {
            return !string.IsNullOrEmpty(Name) &&
                   !string.IsNullOrEmpty(Email) &&
                   !string.IsNullOrEmpty(Phone) &&
                   !string.IsNullOrEmpty(SelectedRole) &&
                   !string.IsNullOrEmpty(Username) &&
                   !string.IsNullOrEmpty(Password);
        }

        /// <summary>
        /// Updates details of the selected user.
        /// </summary>
        private async void Update()
        {
            if (SelectedUser == null || _userHandler == null)
                return;

            SelectedUser.Name = Name;
            SelectedUser.Email = Email;
            SelectedUser.Phone = Phone;
            SelectedUser.Role = SelectedRole;

            if (!Username.Equals("*****"))
                SelectedUser.Username = Username;
            if (!Password.Equals("*****"))
                SelectedUser.Password = Password;

            bool userUpdated = await _userHandler.UpdateUser(SelectedUser);
            if (userUpdated)
            {
                ResetForm();
                await PopulateUserList();
            }
        }

        /// <summary>
        /// Deletes the selected user.
        /// </summary>
        private async void Delete()
        {
            if (SelectedUser == null || SelectedUser.Id == null || _userHandler == null)
                return;

            bool userDeleted = await _userHandler.DeleteUser(SelectedUser.Id);
            if (userDeleted)
            {
                ResetForm();
                await PopulateUserList();
            }
        }

        /// <summary>
        /// Checks if a user is selected.
        /// </summary>
        private bool IsUserSelected() => SelectedUser != null;

        /// <summary>
        /// Initializes the view and populates the user list.
        /// </summary>
        private async void InitView()
        {
            UserHttpAdapter userHttpAdapter = new UserHttpAdapter(RestApiClient.Instance.Client);
            this._userHandler = new UserHandler(userHttpAdapter);

            await PopulateUserList();
        }

        /// <summary>
        /// Fetches and populates the list of users.
        /// </summary>
        private async Task PopulateUserList()
        {
            if (UserManager.Instance.InstitutionId == null || _userHandler == null)
                return;

            this.Userlist.Clear();
            var users = await _userHandler.GetAllUsers(UserManager.Instance.InstitutionId);
            if (users != null)
            {
                foreach (var user in users)
                    Userlist.Add(user);
            }
        }

        /// <summary>
        /// Populates form fields with details of the selected user.
        /// </summary>
        private void PopulateUserProfile()
        {
            if (SelectedUser == null)
                return;

            Name = SelectedUser.Name ?? string.Empty;
            Email = SelectedUser.Email ?? string.Empty;
            Phone = SelectedUser.Phone ?? string.Empty;
            SelectedRole = SelectedUser.Role ?? string.Empty;
            Username = "*****";
            Password = "*****";
        }

        /// <summary>
        /// Resets form fields to default values.
        /// </summary>
        private void ResetFields()
        {
            Name = Email = Phone = Username = Password = string.Empty;
        }
    }
}