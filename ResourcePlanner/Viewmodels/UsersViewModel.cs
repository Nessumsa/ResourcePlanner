using ResourcePlanner.Domain;
using ResourcePlanner.Infrastructure;
using ResourcePlanner.Infrastructure.Adapters;
using ResourcePlanner.UseCases;
using ResourcePlanner.Utilities;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ResourcePlanner.Viewmodels
{
    class UsersViewModel : Bindable
    {
        private UserHandler? _userHandler;

        public ICommand MakeNewCMD { get; }
        public ICommand DeleteCMD { get; }
        public ICommand SaveCMD { get; private set; }

        private ObservableCollection<User> _userList;
        public ObservableCollection<User> Userlist
        {
            get { return _userList; }
            set
            {
                _userList = value;
                OnPropertyChanged();
            }
        }

        private User? _selectedUser;
        public User? SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
                PopulateUserProfile();
                SaveCMD = new CommandRelay(Update, IsUserSelected);
            }
        }

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

        public string[] Roles { get; } = { "admin", "user" };
        private string _selectedRole;

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

        public UsersViewModel()
        {
            this.MakeNewCMD = new CommandRelay(ResetForm, IsUserSelected);
            this.DeleteCMD = new CommandRelay(Delete, IsUserSelected);
            this.SaveCMD = new CommandRelay(Create, CanCreate);

            this._userList = new ObservableCollection<User>();

            this._name = string.Empty;
            this._email = string.Empty;
            this._phone = string.Empty;
            this._selectedRole = string.Empty;
            this._username = string.Empty;
            this._password = string.Empty;

            LogOnScreenViewModel.UserLoggedIn += InitView;
        }

        private void ResetForm()
        {
            ResetFields();
            SelectedUser = null;
            SaveCMD = new CommandRelay(Create, CanCreate);
        }

        private async void Create()
        {
            if (UserManager.Instance.InstitutionId == null || _userHandler == null)
                return;

            User user = new User(Name, Email, Phone, SelectedRole, Username, Password, UserManager.Instance.InstitutionId);
            bool userCreated = await _userHandler.CreateUser(user);
            if (userCreated)
            {
                ResetFields();
                SelectedUser = null;
                await PopulateUserList();
            }
        }
        private bool CanCreate()
        {
            return !string.IsNullOrEmpty(Name) &&
                   !string.IsNullOrEmpty(Email) &&
                   !string.IsNullOrEmpty(Phone) &&
                   !string.IsNullOrEmpty(SelectedRole) &&
                   !string.IsNullOrEmpty(Username) &&
                   !string.IsNullOrEmpty(Password);
        }

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

            await _userHandler.UpdateUser(SelectedUser);
        }

        private async void Delete()
        {
            if (SelectedUser == null || SelectedUser.Id == null || _userHandler == null)
                return;

            bool userDeleted = await _userHandler.DeleteUser(SelectedUser.Id);
            if (userDeleted)
            {
                ResetFields();
                SelectedUser = null;
                await PopulateUserList();
            }
        }
        private bool IsUserSelected() => SelectedUser != null;

        private async void InitView()
        {
            UserHttpAdapter userHttpAdapter = new UserHttpAdapter(RestApiClient.Instance.Client);
            this._userHandler = new UserHandler(userHttpAdapter);

            await PopulateUserList();
        }

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

        private void ResetFields()
        {
            Name = Email = Phone = SelectedRole = Username = Password = string.Empty;
        }
    }
}
