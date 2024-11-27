namespace ResourcePlanner.Infrastructure
{
    public class UserManager
    {
        private static UserManager? _instance;
        public string? UserId { get; private set; }
        public string? InstitutionId { get; private set; }

        public static UserManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UserManager();
                return _instance;
            }
        }

        private UserManager() {}

        public void Initialize(string userId, string institutionId)
        {
            UserId = userId;
            InstitutionId = institutionId;
        }

        public void Reset()
        {
            UserId = null;
            InstitutionId = null;
        }
    }
}
