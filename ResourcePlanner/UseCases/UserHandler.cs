using ResourcePlanner.Domain;
using ResourcePlanner.Interfaces.Adapters.CRUD;

namespace ResourcePlanner.UseCases
{
    /// <summary>
    /// A handler class for use cases related to users, 
    /// providing methods for retrieving, creating, updating, and deleting users.
    /// </summary>
    public class UserHandler
    {
        private readonly ICrudAdapter<User, string> _userAdapter;

        /// <summary>
        /// Initializes a new instance of the UserHandler class with the specified user adapter.
        /// </summary>
        /// <param name="userAdapter">The adapter used for interacting with the backend API for user-related operations.</param>
        public UserHandler(ICrudAdapter<User, string> userAdapter)
        {
            this._userAdapter = userAdapter;
        }

        /// <summary>
        /// Retrieves a user based on the provided user ID.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <returns>The user object corresponding to the specified ID, or null if not found.</returns>
        public async Task<User?> GetUser(string userId)
        {
            return await _userAdapter.ReadAsync(userId);
        }

        /// <summary>
        /// Retrieves all users for the specified institution ID.
        /// </summary>
        /// <param name="institutionId">The ID of the institution for which to retrieve users.</param>
        /// <returns>A collection of users associated with the specified institution ID, or null if not found.</returns>
        public async Task<IEnumerable<User>?> GetAllUsers(string institutionId)
        {
            return await _userAdapter.ReadAllAsync(institutionId);
        }

        /// <summary>
        /// Creates a new user with the provided user details.
        /// </summary>
        /// <param name="user">The user object containing the information of the user to create.</param>
        /// <returns>True if the user was successfully created, otherwise false.</returns>
        public async Task<bool> CreateUser(User user)
        {
            return await _userAdapter.CreateAsync(user);
        }

        /// <summary>
        /// Updates an existing user's information with the provided user details.
        /// </summary>
        /// <param name="user">The user object containing the updated information.</param>
        /// <returns>True if the user was successfully updated, otherwise false.</returns>
        public async Task<bool> UpdateUser(User user)
        {
            return await _userAdapter.UpdateAsync(user);
        }

        /// <summary>
        /// Deletes a user based on the provided user ID.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns>True if the user was successfully deleted, otherwise false.</returns>
        public async Task<bool> DeleteUser(string userId)
        {
            return await _userAdapter.DeleteAsync(userId);
        }
    }
}