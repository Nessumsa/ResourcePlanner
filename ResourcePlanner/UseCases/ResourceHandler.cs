using ResourcePlanner.Domain;
using ResourcePlanner.Interfaces.Adapters.CRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourcePlanner.UseCases
{
    class ResourceHandler
    {
        private readonly ICrudAdapter<Resource, string> _resourceAdapter;
        public ResourceHandler(ICrudAdapter<Resource, string> resourceAdapter)
        {
            this._resourceAdapter = resourceAdapter;
        }

        public async Task<Resource?> ReadUser(string resourceId)
        {
            return await _resourceAdapter.ReadAsync(resourceId);
        }

        public async Task<IEnumerable<Resource>?> ReadAll(string insitutionId)
        {
            return await _resourceAdapter.ReadAllAsync(insitutionId);
        }

        public async Task<bool> CreateResource(Resource resource)
        {
            return await _resourceAdapter.CreateAsync(resource);
        }

        public async Task<bool> UpdateResource(Resource resource)
        {
            return await _resourceAdapter.UpdateAsync(resource);
        }

        public async Task<bool> DeleteResource(string resourceId)
        {
            return await _resourceAdapter.DeleteAsync(resourceId);
        }
    }
}
