using ResourcePlanner.Interfaces.Managers;

namespace ResourcePlanner.UseCases
{
    /// <summary>
    /// A use case handler for logging in a user by connecting to the backend API and authenticating the user.
    /// </summary>
    public class LoginUser
    {
        private readonly IRestApiClient _restApiClient;

        /// <summary>
        /// Initializes a new instance of the LoginUser class with the specified REST API client.
        /// </summary>
        /// <param name="restApiClient">The REST API client used for making requests to the backend.</param>
        public LoginUser(IRestApiClient restApiClient)
        {
            this._restApiClient = restApiClient;
        }

        /// <summary>
        /// Executes the user login process by connecting to the backend API and authenticating the user.
        /// </summary>
        /// <param name="host">The host address of the backend API.</param>
        /// <param name="port">The port number for the backend API.</param>
        /// <param name="username">The username of the user attempting to log in.</param>
        /// <param name="password">The password of the user attempting to log in.</param>
        /// <returns>True if the login is successful, otherwise false.</returns>
        public async Task<bool> Execute(string host, string port, string username, string password)
        {
            bool connection = await _restApiClient.ConnectAsync(host, port);
            if (connection)
            {
                bool authenticated = await _restApiClient.LoginAsync(username, password);
                if (authenticated)
                    return true;
            }
            return false;
        }
    }
}