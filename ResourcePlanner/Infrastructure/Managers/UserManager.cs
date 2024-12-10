namespace ResourcePlanner.Infrastructure.Managers
{
    /// <summary>
    /// A singleton class that manages the currently logged-in user's information, 
    /// such as UserId and InstitutionId.
    /// </summary>
    public class UserManager
    {
        private static UserManager? _instance;

        /// <summary>
        /// Gets the UserId of the currently logged-in user.
        /// </summary>
        public string? UserId { get; private set; }

        /// <summary>
        /// Gets the InstitutionId associated with the currently logged-in user.
        /// </summary>
        public string? InstitutionId { get; private set; }

        /// <summary>
        /// Gets the singleton instance of the UserManager.
        /// </summary>
        public static UserManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UserManager();
                return _instance;
            }
        }

        private UserManager() { }

        /// <summary>
        /// Initializes the UserManager with the user's information.
        /// </summary>
        /// <param name="userId">The ID of the logged-in user.</param>
        /// <param name="institutionId">The ID of the institution the user is associated with.</param>
        public void Initialize(string userId, string institutionId)
        {
            UserId = userId;
            InstitutionId = institutionId;
        }

        /// <summary>
        /// Resets the UserManager, clearing the UserId and InstitutionId.
        /// </summary>
        public void Reset()
        {
            UserId = null;
            InstitutionId = null;
        }
    }
}