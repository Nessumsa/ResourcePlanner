using Newtonsoft.Json;
using ResourcePlanner.Domain;
using ResourcePlanner.Interfaces.Adapters.CRUD;
using System.Net.Http;
using System.Text;

namespace ResourcePlanner.Infrastructure.Adapters
{
    public class UserHttpAdapter : ICrudAdapter<User, string>
    {
        private readonly HttpClient _client;

        public UserHttpAdapter(HttpClient client)
        {
            this._client = client;
        }

        public async Task<User?> ReadAsync(string userId)
        {
            var response = await _client.GetAsync($"/api/users/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(content);
            }
            throw new HttpRequestException($"Failed to fetch user with ID:{userId}.");
        }

        public async Task<IEnumerable<User>?> ReadAllAsync(string institutionId)
        {
            var response = await _client.GetAsync($"/api/users/all?institutionId={institutionId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<User>>(content);
            }
            throw new HttpRequestException($"Failed to fetch users for institution:{institutionId}.");
        }

        public async Task<bool> CreateAsync(User entity)
        {
            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(entity),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PostAsync("/api/users", jsonContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(User entity)
        {
            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(entity), 
                Encoding.UTF8, 
                "application/json"
            );

            var response = await _client.PutAsync($"/api/users/{entity.Id}", jsonContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string userId)
        {
            var response = await _client.DeleteAsync($"/api/users/{userId}");
            return response.IsSuccessStatusCode;
        }
    }
}