using ResourcePlanner.Domain;
using ResourcePlanner.Interfaces.Adapters;

namespace ResourcePlanner.UseCases
{
    /// <summary>
    /// A handler class for use cases related to institutions, 
    /// providing methods for retrieving and updating institution information.
    /// </summary>
    public class InstitutionHandler
    {
        private readonly IInstitutionAdapter<Institution, string> _institutionAdapter;

        /// <summary>
        /// Initializes a new instance of the InstitutionHandler class with the specified institution adapter.
        /// </summary>
        /// <param name="institutionAdapter">The adapter used for interacting with the backend API for institution-related operations.</param>
        public InstitutionHandler(IInstitutionAdapter<Institution, string> institutionAdapter)
        {
            this._institutionAdapter = institutionAdapter;
        }

        /// <summary>
        /// Retrieves an institution based on the provided institution ID.
        /// </summary>
        /// <param name="institutionId">The ID of the institution to retrieve.</param>
        /// <returns>The institution object corresponding to the specified ID, or null if not found.</returns>
        public async Task<Institution?> GetInstitution(string institutionId)
        {
            return await _institutionAdapter.ReadAsync(institutionId);
        }

        /// <summary>
        /// Updates the institution's information.
        /// </summary>
        /// <param name="institution">The institution object containing the updated information.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        public Task<bool> UpdateInstitution(Institution institution)
        {
            return _institutionAdapter.UpdateAsync(institution);
        }
    }
}