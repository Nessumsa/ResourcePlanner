using Newtonsoft.Json;
using ResourcePlanner.DTOs;
using ResourcePlanner.Interfaces.Managers;
using System.Net.Http;
using System.Text;

namespace ResourcePlanner.Infrastructure.Managers
{
    /// <summary>
    /// A singleton class that manages the HttpClient for communication 
    /// with the backend API and handles the access token for authentication.
    /// </summary>
    public class RestApiClient : IRestApiClient
    {
        private static RestApiClient? _instance;
        private HttpClient? _client;
        private string _accessToken;
        private bool _initialized = false;

        /// <summary>
        /// Gets the singleton instance of the RestApiClient.
        /// </summary>
        public static RestApiClient Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new RestApiClient();
                return _instance;
            }
        }

        /// <summary>
        /// Gets the HttpClient instance used for communication with the backend API.
        /// Throws an exception if the client is not initialized.
        /// </summary>
        public HttpClient Client
        {
            get
            {
                if (!_initialized || _client == null)
                    throw new InvalidOperationException("HttpClient is not initialized.");

                return _client;
            }
        }

        private RestApiClient()
        {
            _accessToken = "";
        }

        /// <summary>
        /// Establishes a connection to the backend API by initializing 
        /// the HttpClient with the specified host and port.
        /// </summary>
        /// <param name="host">The host address of the API.</param>
        /// <param name="port">The port number of the API.</param>
        /// <returns>True if the connection is successful, otherwise false.</returns>
        public async Task<bool> ConnectAsync(string host, string port)
        {
            string baseUrl = $"http://{host}:{port}";
            _client = new HttpClient { BaseAddress = new Uri(baseUrl) };

            var response = await _client.GetAsync("/Test");
            if (response != null)
            {
                _initialized = true;
                return true;
            }

            _client = null;
            _initialized = false;
            return false;
        }

        /// <summary>
        /// Disconnects from the backend API by disposing of the HttpClient 
        /// and resetting the connection status.
        /// </summary>
        /// <returns>Always returns true.</returns>
        public bool DisconnectAsync()
        {
            _client?.Dispose();
            _client = null;
            _initialized = false;
            return true;
        }

        /// <summary>
        /// Logs in a user by sending their credentials to the backend API 
        /// and retrieves an access token for further requests.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>True if the login is successful and the user is an admin, otherwise false.</returns>
        public async Task<bool> LoginAsync(string username, string password)
        {
            if (_client == null)
                return false;

            LoginRequestDto request = new() { Username = username, Password = password };

            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(request),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PostAsync("/api/login", jsonContent);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(responseContent);

                if (loginResponse == null)
                    return false;

                if (loginResponse.UserRole!.Equals("admin") && loginResponse?.AccessToken != null)
                {
                    _accessToken = loginResponse.AccessToken;
                    _client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);

                    UserManager.Instance.Initialize(loginResponse.UserId!, loginResponse.InstitutionId!);
                    return true;
                }
            }
            return false;
        }
    }
}