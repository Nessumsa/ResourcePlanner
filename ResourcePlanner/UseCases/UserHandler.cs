using ResourcePlanner.Domain;
using ResourcePlanner.Interfaces.Adapters.CRUD;

namespace ResourcePlanner.UseCases
{
    public class UserHandler
    {
        private readonly ICrudAdapter<User, string> _userAdapter;
        public UserHandler(ICrudAdapter<User, string> userAdapter)
        {
            this._userAdapter = userAdapter;
        }

        public async Task<User?> ReadUser(string userId)
        {
            return await _userAdapter.ReadAsync(userId);
        }

        public async Task<IEnumerable<User>?> ReadAll(string insitutionId)
        {
            return await _userAdapter.ReadAllAsync(insitutionId);
        }

        public async Task<bool> CreateUser(User user)
        {
            return await _userAdapter.CreateAsync(user);
        }

        public async Task<bool> UpdateUser(User user)
        {
            return await _userAdapter.UpdateAsync(user);
        }

        public async Task<bool> DeleteUser(string userId) 
        {
            return await _userAdapter.DeleteAsync(userId);
        }
    }
}
