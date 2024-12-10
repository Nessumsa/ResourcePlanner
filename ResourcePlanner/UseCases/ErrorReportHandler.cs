using ResourcePlanner.Domain;
using ResourcePlanner.Interfaces.Adapters;

namespace ResourcePlanner.UseCases
{
    /// <summary>
    /// A handler class for use cases related to error reports, 
    /// providing methods for retrieving and resolving error reports.
    /// </summary>
    public class ErrorReportHandler
    {
        private readonly IErrorReportAdapter _errorReportAdapter;

        /// <summary>
        /// Initializes a new instance of the ErrorReportHandler class 
        /// with the specified error report adapter.
        /// </summary>
        /// <param name="errorReportAdapter">The adapter used for interacting with the backend API for error reports.</param>
        public ErrorReportHandler(IErrorReportAdapter errorReportAdapter)
        {
            this._errorReportAdapter = errorReportAdapter;
        }

        /// <summary>
        /// Retrieves all error reports for the specified institution.
        /// </summary>
        /// <param name="institutionId">The institution ID for which to retrieve error reports.</param>
        /// <returns>A collection of error reports, or null if no reports are found.</returns>
        public async Task<IEnumerable<ErrorReport>?> GetAllErrorReports(string institutionId)
        {
            return await _errorReportAdapter.ReadAllAsync(institutionId);
        }

        /// <summary>
        /// Retrieves all active (unresolved) error reports for the specified institution.
        /// </summary>
        /// <param name="institutionId">The institution ID for which to retrieve active error reports.</param>
        /// <returns>A collection of active error reports.</returns>
        public async Task<IEnumerable<ErrorReport>> GetAllActiveErrorReports(string institutionId)
        {
            return await _errorReportAdapter.ReadAllActiveAsync(institutionId);
        }

        /// <summary>
        /// Resolves a given error report by updating it in the backend.
        /// </summary>
        /// <param name="errorReport">The error report to resolve.</param>
        /// <returns>True if the report was successfully resolved, otherwise false.</returns>
        public async Task<bool> ResolveErrorReport(ErrorReport errorReport)
        {
            return await _errorReportAdapter.UpdateAsync(errorReport);
        }
    }
}