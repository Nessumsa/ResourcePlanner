using Newtonsoft.Json;
using ResourcePlanner.Domain;
using ResourcePlanner.Interfaces.Adapters;
using System.Net.Http;
using System.Text;

namespace ResourcePlanner.Infrastructure.Adapters
{
    /// <summary>
    /// An adapter that facilitates communication between the client/application 
    /// and the backend REST API, integrating with error reports.
    /// </summary>
    public class ErrorReportHttpAdapter : IErrorReportAdapter
    {
        private readonly HttpClient _client;

        /// <summary>
        /// Initializes the adapter with an HttpClient for communication.
        /// </summary>
        public ErrorReportHttpAdapter(HttpClient client)
        {
            this._client = client;
        }

        /// <summary>
        /// Retrieves all error reports for a given institution.
        /// </summary>
        /// <param name="institutionId">The ID of the institution for which to fetch error reports.</param>
        /// <returns>A collection of error reports.</returns>
        /// <exception cref="HttpRequestException">Thrown when the request fails.</exception>
        public async Task<IEnumerable<ErrorReport>?> ReadAllAsync(string institutionId)
        {
            var response = await _client.GetAsync($"/api/error-reports/all?institutionId={institutionId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<ErrorReport>>(content);
            }

            throw new HttpRequestException($"Failed to fetch error reports for institution:{institutionId}.");
        }

        /// <summary>
        /// Retrieves all active error reports for a given institution.
        /// </summary>
        /// <param name="institutionId">The ID of the institution for which to fetch active error reports.</param>
        /// <returns>A collection of active error reports.</returns>
        public async Task<IEnumerable<ErrorReport>> ReadAllActiveAsync(string institutionId)
        {
            var allErrorReports = await ReadAllAsync(institutionId);
            return allErrorReports?.Where(report => !report.Resolved) ?? Enumerable.Empty<ErrorReport>();
        }

        /// <summary>
        /// Updates an existing error report in the system.
        /// </summary>
        /// <param name="entity">The error report entity to update.</param>
        /// <returns>A boolean indicating whether the update was successful.</returns>
        public async Task<bool> UpdateAsync(ErrorReport entity)
        {
            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(entity),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PutAsync($"/api/error-reports/{entity.Id}", jsonContent);
            return response.IsSuccessStatusCode;
        }
    }
}