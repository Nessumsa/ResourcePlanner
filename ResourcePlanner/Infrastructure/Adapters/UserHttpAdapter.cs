using Newtonsoft.Json;
using ResourcePlanner.Domain;
using ResourcePlanner.Interfaces.Adapters.CRUD;
using System.Net.Http;
using System.Text;

namespace ResourcePlanner.Infrastructure.Adapters
{
    /// <summary>
    /// An adapter that facilitates communication between the client/application 
    /// and the backend API for user management.
    /// </summary>
    public class UserHttpAdapter : ICrudAdapter<User, string>
    {
        private readonly HttpClient _client;

        /// <summary>
        /// Initializes the adapter with an HttpClient for communication.
        /// </summary>
        public UserHttpAdapter(HttpClient client)
        {
            this._client = client;
        }

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <returns>The user object if the request is successful, otherwise null.</returns>
        /// <exception cref="HttpRequestException">Thrown when the request fails.</exception>
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

        /// <summary>
        /// Retrieves all users for a given institution.
        /// </summary>
        /// <param name="institutionId">The ID of the institution to fetch users for.</param>
        /// <returns>A collection of users if the request is successful, otherwise null.</returns>
        /// <exception cref="HttpRequestException">Thrown when the request fails.</exception>
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

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="entity">The user entity to create.</param>
        /// <returns>A boolean indicating whether the user creation was successful.</returns>
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

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="entity">The user entity to update.</param>
        /// <returns>A boolean indicating whether the update was successful.</returns>
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

        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns>A boolean indicating whether the deletion was successful.</returns>
        public async Task<bool> DeleteAsync(string userId)
        {
            var response = await _client.DeleteAsync($"/api/users/{userId}");
            return response.IsSuccessStatusCode;
        }
    }
}