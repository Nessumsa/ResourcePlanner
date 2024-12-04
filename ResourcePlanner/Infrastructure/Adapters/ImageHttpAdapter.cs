using ResourcePlanner.Interfaces.Adapters;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ResourcePlanner.Infrastructure.Adapters
{
    public class ImageHttpAdapter : IImageAdapter
    {
        private readonly HttpClient _httpClient;
        public ImageHttpAdapter(HttpClient client)
        {
            this._httpClient = client;
        }
        public async Task<string?> UploadAsync(string filePath)
        {
            using var form = new MultipartFormDataContent();
            await using var stream = File.OpenRead(filePath);
            var streamContent = new StreamContent(stream)
            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/octet-stream") }
            };

            form.Add(streamContent, "file", Path.GetFileName(filePath));

            var response = await _httpClient.PostAsync("/api/images", form);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();

            return null;
        }
        public async Task<bool> DeleteAsync(string filepath)
        {
            var response = await _httpClient.DeleteAsync($"/api/images/delete?url={filepath}");
            if (response.IsSuccessStatusCode)
                return true;

            return false;
        }
    }
}
