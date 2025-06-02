using Pml.Domain.IRepositories.Client;
using Pml.Shared.Entities.Models.Client;

namespace Pml.Infrastructure.Client.ClientRepositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IClientRepository _clientRepository;

        public RoleRepository(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }



        /// <summary>
        /// Gets a role by its unique identifier.
        /// </summary>
        /// <param name="id">The role ID.</param>
        /// <returns>The role entity or null if not found.</returns>
        public async Task<Role> GetByIdAsync(int id)
        {
            try
            {
                var query = "SELECT * FROM Role WHERE Id = @Id";
                var parameters = new { Id = id };
                var result = await _clientRepository.ExecuteQueryAsync(query, parameters);
                var roleData = result.FirstOrDefault();

                if (roleData == null)
                    return null;

                return MapToRole(roleData);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting role by ID {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get roles by userId
        /// </summary>

        public async Task<IEnumerable<string>> GetRolesByUserIdAsync(int userId)
        {
            try
            {
                var query = @"
                    SELECT r.* 
                    FROM Role r
                    JOIN [User] u ON u.UserRoleCode = r.Id
                    WHERE u.Id = @UserId AND r.IsActive = 1";
                var parameters = new { UserId = userId };
                var result = await _clientRepository.ExecuteQueryAsync(query, parameters);
                return (IEnumerable<string>)result.Select(MapToRole);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting roles by user ID {userId}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets a role by its name.
        /// </summary>
        /// <param name="roleName">The role name.</param>
        /// <returns>The role entity or null if not found.</returns>
        public async Task<Role> GetByNameAsync(string roleName)
        {
            try
            {
                var query = "SELECT * FROM Role WHERE RoleName = @RoleName AND IsActive = 1";
                var parameters = new { RoleName = roleName };
                var result = await _clientRepository.ExecuteQueryAsync(query, parameters);
                var roleData = result.FirstOrDefault();

                if (roleData == null)
                    return null;

                return MapToRole(roleData);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting role by name {roleName}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all roles.
        /// </summary>
        /// <returns>A collection of all roles.</returns>
        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            try
            {
                var query = "SELECT * FROM Role ORDER BY RoleName";
                var result = await _clientRepository.ExecuteQueryAsync(query);
                return result.Select(MapToRole);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting all roles: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all active roles.
        /// </summary>
        /// <returns>A collection of active roles.</returns>
        public async Task<IEnumerable<Role>> GetActiveRolesAsync()
        {
            try
            {
                var query = "SELECT * FROM Role WHERE IsActive = 1 ORDER BY RoleName";
                var result = await _clientRepository.ExecuteQueryAsync(query);
                return result.Select(MapToRole);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting active roles: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Creates a new role.
        /// </summary>
        /// <param name="role">The role entity to create.</param>
        /// <returns>The created role entity.</returns>
        public async Task<Role> CreateAsync(Role role)
        {
            try
            {
                var query = @"
                    INSERT INTO Role (RoleName, RoleDescription, RoleStatus, CreatedDate, IsActive)
                    VALUES (@RoleName, @RoleDescription, @RoleStatus, @CreatedDate, @IsActive);
                    SELECT SCOPE_IDENTITY();";

                var parameters = new
                {
                    role.RoleName,
                    role.RoleDescription,
                    role.RoleStatus,
                    role.CreatedDate,
                    role.IsActive
                };

                var result = await _clientRepository.ExecuteQueryAsync(query, parameters);
                var newId = Convert.ToInt32(result.FirstOrDefault());

                return await GetByIdAsync(newId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating role: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing role.
        /// </summary>
        /// <param name="role">The role entity to update.</param>
        /// <returns>The updated role entity.</returns>
        public async Task<Role> UpdateAsync(Role role)
        {
            try
            {
                var query = @"
                    UPDATE Role 
                    SET RoleName = @RoleName, RoleDescription = @RoleDescription, 
                        RoleStatus = @RoleStatus
                    WHERE Id = @Id";

                var parameters = new
                {
                    role.RoleName,
                    role.RoleDescription,
                    role.RoleStatus,
                    role.Id
                };

                await _clientRepository.ExecuteCommandAsync(query, parameters);
                return await GetByIdAsync(role.Id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating role: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deletes a role by setting IsActive to false.
        /// </summary>
        /// <param name="id">The role ID.</param>
        /// <returns>True if the role was deleted; otherwise, false.</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                // Check if any users are assigned to this role
                var userCount = await GetUserCountByRoleAsync(id);
                if (userCount > 0)
                {
                    throw new InvalidOperationException($"Cannot delete role. {userCount} users are assigned to this role.");
                }

                var query = "UPDATE Role SET IsActive = 0 WHERE Id = @Id";
                var parameters = new { Id = id };

                var result = await _clientRepository.ExecuteCommandAsync(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting role: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Activates a role.
        /// </summary>
        /// <param name="id">The role ID.</param>
        /// <returns>True if the role was activated; otherwise, false.</returns>
        public async Task<bool> ActivateRoleAsync(int id)
        {
            try
            {
                var query = "UPDATE Role SET IsActive = 1, RoleStatus = 'Active' WHERE Id = @Id";
                var parameters = new { Id = id };
                var result = await _clientRepository.ExecuteCommandAsync(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error activating role: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deactivates a role.
        /// </summary>
        /// <param name="id">The role ID.</param>
        /// <returns>True if the role was deactivated; otherwise, false.</returns>
        public async Task<bool> DeactivateRoleAsync(int id)
        {
            try
            {
                var query = "UPDATE Role SET IsActive = 0, RoleStatus = 'Inactive' WHERE Id = @Id";
                var parameters = new { Id = id };
                var result = await _clientRepository.ExecuteCommandAsync(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deactivating role: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Checks if a role name already exists.
        /// </summary>
        /// <param name="roleName">The role name to check.</param>
        /// <returns>True if the role name exists; otherwise, false.</returns>
        public async Task<bool> RoleNameExistsAsync(string roleName)
        {
            try
            {
                var query = "SELECT COUNT(1) FROM Role WHERE RoleName = @RoleName";
                var parameters = new { RoleName = roleName };

                var result = await _clientRepository.ExecuteQueryAsync(query, parameters);
                var count = Convert.ToInt32(result.FirstOrDefault());
                return count > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking role name existence: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets the count of users assigned to a specific role.
        /// </summary>
        /// <param name="roleId">The role ID.</param>
        /// <returns>The number of users assigned to the role.</returns>
        public async Task<int> GetUserCountByRoleAsync(int roleId)
        {
            try
            {
                var query = "SELECT COUNT(1) FROM [User] WHERE UserRoleCode = @RoleId AND IsActive = 1";
                var parameters = new { RoleId = roleId };

                var result = await _clientRepository.ExecuteQueryAsync(query, parameters);
                return Convert.ToInt32(result.FirstOrDefault());
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting user count by role: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Maps dynamic object to Role entity.
        /// </summary>
        /// <param name="data">Dynamic data from database.</param>
        /// <returns>Role entity.</returns>
        private Role MapToRole(dynamic data)
        {
            return new Role
            {
                Id = data.Id,
                RoleName = data.RoleName,
                RoleDescription = data.RoleDescription,
                RoleStatus = data.RoleStatus,
                CreatedDate = data.CreatedDate,
                IsActive = data.IsActive
            };
        }
    }
}
