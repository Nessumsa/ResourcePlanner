using ResourcePlanner.Interfaces.Adapters;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ResourcePlanner.Infrastructure.Adapters
{
    /// <summary>
    /// An adapter that facilitates communication between the client/application 
    /// and the backend API for image uploading and deletion.
    /// </summary>
    public class ImageHttpAdapter : IImageAdapter
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes the adapter with an HttpClient for communication.
        /// </summary>
        public ImageHttpAdapter(HttpClient client)
        {
            this._httpClient = client;
        }

        /// <summary>
        /// Uploads an image to the backend.
        /// </summary>
        /// <param name="filePath">The file path of the image to upload.</param>
        /// <returns>A string representing the response from the server, or null if the upload fails.</returns>
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

        /// <summary>
        /// Deletes an image from the backend.
        /// </summary>
        /// <param name="filepath">The file path of the image to delete.</param>
        /// <returns>A boolean indicating whether the deletion was successful.</returns>
        public async Task<bool> DeleteAsync(string filepath)
        {
            var response = await _httpClient.DeleteAsync($"/api/images?filePath={filepath}");
            return response.IsSuccessStatusCode;
        }
    }
}