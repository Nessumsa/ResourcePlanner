using ResourcePlanner.Domain;
using ResourcePlanner.Interfaces.Adapters;

namespace ResourcePlanner.UseCases
{
    public class InstitutionHandler
    {
        private readonly IInstitutionAdapter<Institution, string> _institutionAdapter;
        public InstitutionHandler(IInstitutionAdapter<Institution, string> institutionAdapter)
        {
            this._institutionAdapter = institutionAdapter;
        }

        public async Task<Institution?> GetInstitution(string institutionId)
        {
            return await _institutionAdapter.ReadAsync(institutionId);
        }

        public Task<bool> UpdateInstitution(Institution institution)
        {
            return _institutionAdapter.UpdateAsync(institution);
        }
    }
}
