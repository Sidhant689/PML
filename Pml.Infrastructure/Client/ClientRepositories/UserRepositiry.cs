using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pml.Domain.IRepositories.Client;
using Pml.Shared.Entities.Models.Client;

namespace Pml.Infrastructure.Client.ClientRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IClientRepository _clientRepository;

        public UserRepository(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        /// <summary>
        /// Gets a user by their unique identifier.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>The user entity or null if not found.</returns>
        public async Task<User> GetByIdAsync(int id)
        {
            try
            {
                var query = @"
                    SELECT u.*, r.RoleName, r.RoleDescription 
                    FROM [Users] u 
                    LEFT JOIN Roles r ON u.UserRoleCode = r.Id 
                    WHERE u.Id = @Id AND u.IsActive = 1";

                var parameters = new { Id = id };
                var result = await _clientRepository.ExecuteQueryAsync(query, parameters);
                var userData = result.FirstOrDefault();

                if (userData == null)
                    return null;

                return MapToUser(userData);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting user by ID {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets a user by their username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>The user entity or null if not found.</returns>
        public async Task<User> GetByUsernameAsync(string username)
        {
            try
            {
                var query = @"
                    SELECT u.*, r.RoleName, r.RoleDescription 
                    FROM [Users] u 
                    LEFT JOIN Roles r ON u.UserRoleCode = r.Id 
                    WHERE u.UserName = @Username AND u.IsActive = 1";

                var parameters = new { Username = username };
                var result = await _clientRepository.ExecuteQueryAsync(query, parameters);
                var userData = result.FirstOrDefault();

                if (userData == null)
                    return null;

                return MapToUser(userData);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting user by username {username}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets a user by their email address.
        /// </summary>
        /// <param name="email">The email address.</param>
        /// <returns>The user entity or null if not found.</returns>
        public async Task<User> GetByEmailAsync(string email)
        {
            try
            {
                var query = @"
                    SELECT u.*, r.RoleName, r.RoleDescription 
                    FROM [Users] u 
                    LEFT JOIN Roles r ON u.UserRoleCode = r.Id 
                    WHERE u.UserEmail = @Email AND u.IsActive = 1";

                var parameters = new { Email = email };
                var result = await _clientRepository.ExecuteQueryAsync(query, parameters);
                var userData = result.FirstOrDefault();

                if (userData == null)
                    return null;

                return MapToUser(userData);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting user by email {email}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all active users.
        /// </summary>
        /// <returns>A collection of all active users.</returns>
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                var query = @"
                    SELECT u.*, r.RoleName, r.RoleDescription 
                    FROM [Users] u 
                    LEFT JOIN Roles r ON u.UserRoleCode = r.Id 
                    WHERE u.IsActive = 1
                    ORDER BY u.Name";

                var result = await _clientRepository.ExecuteQueryAsync(query);
                return result.Select(MapToUser);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting all users: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all users for a specific company.
        /// </summary>
        /// <param name="companyId">The company ID.</param>
        /// <returns>A collection of users for the company.</returns>
        public async Task<IEnumerable<User>> GetByCompanyIdAsync(int companyId)
        {
            try
            {
                var query = @"
                    SELECT u.*, r.RoleName, r.RoleDescription 
                    FROM [Users] u 
                    LEFT JOIN Roles r ON u.UserRoleCode = r.Id 
                    WHERE u.CompanyId = @CompanyId AND u.IsActive = 1
                    ORDER BY u.Name";

                var parameters = new { CompanyId = companyId };
                var result = await _clientRepository.ExecuteQueryAsync(query, parameters);
                return result.Select(MapToUser);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting users by company ID {companyId}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all users with a specific role.
        /// </summary>
        /// <param name="roleId">The role ID.</param>
        /// <returns>A collection of users with the specified role.</returns>
        public async Task<IEnumerable<User>> GetByRoleAsync(int roleId)
        {
            try
            {
                var query = @"
                    SELECT u.*, r.RoleName, r.RoleDescription 
                    FROM [Users] u 
                    LEFT JOIN Roles r ON u.UserRoleCode = r.Id 
                    WHERE u.UserRoleCode = @RoleId AND u.IsActive = 1
                    ORDER BY u.Name";

                var parameters = new { RoleId = roleId };
                var result = await _clientRepository.ExecuteQueryAsync(query, parameters);
                return result.Select(MapToUser);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting users by role ID {roleId}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user">The user entity to create.</param>
        /// <returns>The created user entity.</returns>
        public async Task<User> CreateAsync(User user)
        {
            try
            {
                var query = @"
                    INSERT INTO [Users] (Name, UserName, Password, UserStatus, UserEmail, UserPhone, 
                                       UserAddress, UserRoleCode, CompanyId, CreatedDate, IsActive)
                    VALUES (@Name, @UserName, @Password, @UserStatus, @UserEmail, @UserPhone, 
                            @UserAddress, @UserRoleCode, @CompanyId, @CreatedDate, @IsActive);
                    SELECT SCOPE_IDENTITY();";

                var parameters = new
                {
                    user.Name,
                    user.UserName,
                    user.Password,
                    user.UserStatus,
                    user.UserEmail,
                    user.UserPhone,
                    user.UserAddress,
                    user.UserRoleCode,
                    user.CompanyId,
                    user.CreatedDate,
                    user.IsActive
                };

                var result = await _clientRepository.ExecuteQueryAsync(query, parameters);
                var newId = Convert.ToInt32(result.FirstOrDefault());

                return await GetByIdAsync(newId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating user: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="user">The user entity to update.</param>
        /// <returns>The updated user entity.</returns>
        public async Task<User> UpdateAsync(User user)
        {
            try
            {
                var query = @"
                    UPDATE [Users] 
                    SET Name = @Name, UserName = @UserName, UserStatus = @UserStatus, 
                        UserEmail = @UserEmail, UserPhone = @UserPhone, UserAddress = @UserAddress,
                        UserRoleCode = @UserRoleCode, ModifiedDate = @ModifiedDate
                    WHERE Id = @Id";

                var parameters = new
                {
                    user.Name,
                    user.UserName,
                    user.UserStatus,
                    user.UserEmail,
                    user.UserPhone,
                    user.UserAddress,
                    user.UserRoleCode,
                    ModifiedDate = DateTime.UtcNow,
                    user.Id
                };

                await _clientRepository.ExecuteCommandAsync(query, parameters);
                return await GetByIdAsync(user.Id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating user: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deletes a user by setting IsActive to false.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>True if the user was deleted; otherwise, false.</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var query = "UPDATE [Users] SET IsActive = 0, ModifiedDate = @ModifiedDate WHERE Id = @Id";
                var parameters = new { Id = id, ModifiedDate = DateTime.UtcNow };

                var result = await _clientRepository.ExecuteCommandAsync(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting user: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Activates a user account.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>True if the user was activated; otherwise, false.</returns>
        public async Task<bool> ActivateUserAsync(int id)
        {
            try
            {
                var query = @"
                    UPDATE [Users] 
                    SET IsActive = 1, UserStatus = 'Active', ModifiedDate = @ModifiedDate 
                    WHERE Id = @Id";

                var parameters = new { Id = id, ModifiedDate = DateTime.UtcNow };
                var result = await _clientRepository.ExecuteCommandAsync(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error activating user: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deactivates a user account.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>True if the user was deactivated; otherwise, false.</returns>
        public async Task<bool> DeactivateUserAsync(int id)
        {
            try
            {
                var query = @"
                    UPDATE [Users] 
                    SET IsActive = 0, UserStatus = 'Inactive', ModifiedDate = @ModifiedDate 
                    WHERE Id = @Id";

                var parameters = new { Id = id, ModifiedDate = DateTime.UtcNow };
                var result = await _clientRepository.ExecuteCommandAsync(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deactivating user: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Changes a user's password.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="newPasswordHash">The new password hash.</param>
        /// <returns>True if the password was changed; otherwise, false.</returns>
        public async Task<bool> ChangePasswordAsync(int userId, string newPasswordHash)
        {
            try
            {
                var query = @"
                    UPDATE [Users] 
                    SET Password = @Password, ModifiedDate = @ModifiedDate 
                    WHERE Id = @Id";

                var parameters = new
                {
                    Password = newPasswordHash,
                    Id = userId,
                    ModifiedDate = DateTime.UtcNow
                };

                var result = await _clientRepository.ExecuteCommandAsync(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error changing password: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Checks if a username already exists.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <returns>True if the username exists; otherwise, false.</returns>
        public async Task<bool> UsernameExistsAsync(string username)
        {
            try
            {
                var query = "SELECT COUNT(1) FROM [Users] WHERE UserName = @Username";
                var parameters = new { Username = username };

                var result = await _clientRepository.ExecuteQueryAsync(query, parameters);
                var count = Convert.ToInt32(result.FirstOrDefault());
                return count > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking username existence: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Checks if an email already exists.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>True if the email exists; otherwise, false.</returns>
        public async Task<bool> EmailExistsAsync(string email)
        {
            try
            {
                var query = "SELECT COUNT(1) FROM [Users] WHERE UserEmail = @Email";
                var parameters = new { Email = email };

                var result = await _clientRepository.ExecuteQueryAsync(query, parameters);
                var count = Convert.ToInt32(result.FirstOrDefault());
                return count > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking email existence: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Maps dynamic object to User entity.
        /// </summary>
        /// <param name="data">Dynamic data from database.</param>
        /// <returns>User entity.</returns>
        private User MapToUser(dynamic data)
        {

            return new User
            {
                Id = (int)data.Id,
                Name = data.Name,
                UserName = data.UserName,
                Password = data.Password,
                UserStatus = data.UserStatus,
                UserEmail = data.UserEmail,
                UserPhone = data.UserPhone,
                UserAddress = data.UserAddress,
                UserRoleCode = (int)data.UserRoleCode,
                CompanyId = (int)data.CompanyId,
                CreatedDate = data.CreatedDate is DateTime ? data.CreatedDate : DateTime.Parse(data.CreatedDate.ToString()),
                ModifiedDate = data.ModifiedDate is DateTime ? data.ModifiedDate : (data.ModifiedDate == null ? null : DateTime.Parse(data.ModifiedDate.ToString())),
                IsActive = data.IsActive is bool b ? b : Convert.ToInt32(data.IsActive) == 1,
                Role = data.RoleName != null ? new Role
                {
                    Id = (int)data.UserRoleCode,
                    RoleName = data.RoleName,
                    RoleDescription = data.RoleDescription
                } : null
            };
        }
    }
}
