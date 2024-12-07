using Newtonsoft.Json;
using ResourcePlanner.Domain;
using ResourcePlanner.Interfaces.Adapters.CRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlanner.Infrastructure.Adapters
{
    class ResourceHttpAdapter : ICrudAdapter<Resource,string>
    {
        private readonly HttpClient _client;

        public ResourceHttpAdapter(HttpClient client)
        {
            this._client = client;
        }

        public async Task<Resource?> ReadAsync(string resourceId)
        {
            var response = await _client.GetAsync($"/api/resources/{resourceId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Resource>(content);
            }
            throw new HttpRequestException($"Failed to fetch resource with ID:{resourceId}.");
        }

        public async Task<IEnumerable<Resource>?> ReadAllAsync(string institutionId)
        {
            var response = await _client.GetAsync($"/api/resources/all?institutionId={institutionId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<Resource>>(content);
            }
            throw new HttpRequestException($"Failed to fetch resources for institution:{institutionId}.");
        }

        public async Task<bool> CreateAsync(Resource entity)
        {
            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(entity),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PostAsync("/api/resources", jsonContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(Resource entity)
        {
            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(entity),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PutAsync($"/api/resources/{entity.Id}", jsonContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string resourceId)
        {
            var response = await _client.DeleteAsync($"/api/resources/{resourceId}");
            return response.IsSuccessStatusCode;
        }
    }
}
