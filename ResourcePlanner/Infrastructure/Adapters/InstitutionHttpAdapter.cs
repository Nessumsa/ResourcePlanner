using Newtonsoft.Json;
using ResourcePlanner.Domain;
using ResourcePlanner.Interfaces.Adapters;
using System.Net.Http;
using System.Text;

namespace ResourcePlanner.Infrastructure.Adapters
{
    internal class InstitutionHttpAdapter : IInstitutionAdapter<Institution, string>
    {
        private readonly HttpClient _client;
        public InstitutionHttpAdapter(HttpClient client)
        {
            this._client = client;
        }
        public async Task<Institution?> ReadAsync(string institutionId)
        {
            var response = await _client.GetAsync($"/api/institutions/{institutionId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Institution>(content);
            }
            throw new HttpRequestException($"Failed to fetch user with ID:{institutionId}.");
        }

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
