using Newtonsoft.Json;
using ResourcePlanner.Domain;
using ResourcePlanner.Interfaces.Adapters;
using ResourcePlanner.Interfaces.Adapters.CRUD;
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
        private readonly IReadAdapter<Resource, string> _resourceAdapter;
        private readonly IReadAdapter<User, string> _userAdapter;

        /// <summary>
        /// Initializes the adapter with an HttpClient for communication
        /// and adapters for retrieving associated resources and users.
        /// </summary>
        /// <param name="client">The HttpClient used for making requests to the backend API.</param>
        /// <param name="resourceAdapter">An adapter for fetching resources by their string ID.</param>
        /// <param name="userAdapter">An adapter for fetching users by their string ID.</param>
        public ErrorReportHttpAdapter(HttpClient client, 
                                      IReadAdapter<Resource, string> resourceAdapter,
                                      IReadAdapter<User, string> userAdapter)
        {
            this._client = client;
            this._resourceAdapter = resourceAdapter;
            this._userAdapter = userAdapter;
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
        /// Retrieves all active error reports for a given institution and populates their associated User and Resource.
        /// </summary>
        /// <param name="institutionId">The ID of the institution for which to fetch active error reports.</param>
        /// <returns>A collection of active error reports with associated resources and users.</returns>
        public async Task<IEnumerable<ErrorReport>> ReadAllActiveAsync(string institutionId)
        {
            var allErrorReports = await ReadAllAsync(institutionId);

            var activeErrorReports = allErrorReports?.Where(report => !report.Resolved) ?? Enumerable.Empty<ErrorReport>();

            foreach (var report in activeErrorReports)
            {
                if (!string.IsNullOrEmpty(report.ResourceId))
                {
                    var resource = await _resourceAdapter.ReadAsync(report.ResourceId);
                    report.AssociatedResource = resource;
                }

                if (!string.IsNullOrEmpty(report.UserId))
                {
                    var user = await _userAdapter.ReadAsync(report.UserId);
                    report.AssociatedUser = user;
                }
            }

            return activeErrorReports;
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