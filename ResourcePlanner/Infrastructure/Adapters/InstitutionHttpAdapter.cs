using Newtonsoft.Json;
using ResourcePlanner.Domain;
using ResourcePlanner.Interfaces.Adapters;
using System.Net.Http;
using System.Text;

namespace ResourcePlanner.Infrastructure.Adapters
{
    /// <summary>
    /// An adapter that facilitates communication between the client/application 
    /// and the backend API for institution data.
    /// </summary>
    public class InstitutionHttpAdapter : IInstitutionAdapter<Institution, string>
    {
        private readonly HttpClient _client;

        /// <summary>
        /// Initializes the adapter with an HttpClient for communication.
        /// </summary>
        public InstitutionHttpAdapter(HttpClient client)
        {
            this._client = client;
        }

        /// <summary>
        /// Retrieves an institution by its ID.
        /// </summary>
        /// <param name="institutionId">The ID of the institution to retrieve.</param>
        /// <returns>The institution object if the request is successful, otherwise null.</returns>
        /// <exception cref="HttpRequestException">Thrown when the request fails.</exception>
        public async Task<Institution?> ReadAsync(string institutionId)
        {
            var response = await _client.GetAsync($"/api/institutions/{institutionId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Institution>(content);
            }
            throw new HttpRequestException($"Failed to fetch institution with ID:{institutionId}.");
        }

        /// <summary>
        /// Updates an existing institution.
        /// </summary>
        /// <param name="entity">The institution entity to update.</param>
        /// <returns>A boolean indicating whether the update was successful.</returns>
        public async Task<bool> UpdateAsync(Institution entity)
        {
            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(entity),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PutAsync($"/api/institutions/{entity.Id}", jsonContent);
            return response.IsSuccessStatusCode;
        }
    }
}