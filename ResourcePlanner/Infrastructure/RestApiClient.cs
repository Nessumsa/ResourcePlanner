using Newtonsoft.Json;
using ResourcePlanner.DTOs;
using ResourcePlanner.Interfaces;
using System.Net.Http;
using System.Text;

namespace ResourcePlanner.Infrastructure
{
    public class RestApiClient : IRestApiClient
    {
        private static RestApiClient? _instance;
        private HttpClient? _client;
        private string _accessToken;
        private bool _initialized = false;

        public static RestApiClient Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new RestApiClient();
                return _instance;
            }
        }

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
            this._accessToken = "";
        }

        public async Task<bool> ConnectAsync(string host, string port)
        {
            string baseUrl = $"http://{host}:{port}";
            _client = new HttpClient { BaseAddress = new Uri(baseUrl) };

            var response = await _client.GetAsync("/Test");
            //return response.IsSuccessStatusCode
            if (response != null)
            {
                _initialized = true;
                return true;
            }

            _client = null;
            _initialized = false;
            return false;
        }

        public bool DisconnectAsync()
        {
            _client?.Dispose();
            _client = null;
            _initialized = false;
            return true;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
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

                if (loginResponse.UserRole.Equals("admin") && loginResponse?.AccessToken != null)
                {
                    _accessToken = loginResponse.AccessToken;
                    _client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);

                    UserManager.Instance.Initialize(loginResponse.UserId, loginResponse.InstitutionId);
                    return true;
                }
            }
            return false;
        }
    }
}
