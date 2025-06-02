// Solution 1: Modify UserRepository to use the Factory Pattern
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pml.Domain.IRepositories.Client;
using Pml.Shared.Entities.Models.Client;
using Pml.Shared.Entities.Models.Master;
using Pml.Domain.IRepositories.Master;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Pml.Infrastructure.Client.Factory;
using Pml.Infrastructure.Client;
using Pml.Infrastructure.Master.Repositories;

namespace Pml.Infrastructure.Client
{
    public class UserRepository : IUserRepository
    {
        private readonly IClientRepositoryFactory _clientRepositoryFactory;
        private readonly ICompanyRepository _companyRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRepository(
            IClientRepositoryFactory clientRepositoryFactory,
            ICompanyRepository companyRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _clientRepositoryFactory = clientRepositoryFactory;
            _companyRepository = companyRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        // Helper method to get the appropriate client repository
        private async Task<IClientRepository> GetClientRepositoryAsync()
        {
            // Get company ID from user claims
            var companyIdClaim = 1.ToString();
            if (string.IsNullOrEmpty(companyIdClaim) || !int.TryParse(companyIdClaim, out int companyId))
            {
                throw new UnauthorizedAccessException("Company ID not found in user claims");
            }

            // Get company database configuration
            var companyDatabase = await _companyRepository.GetDefaultDatabaseAsync(companyId);
            if (companyDatabase == null)
            {
                throw new InvalidOperationException($"No database configuration found for company {companyId}");
            }

            return await _clientRepositoryFactory.CreateClientRepositoryAsync(companyDatabase);
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
                var clientRepository = await GetClientRepositoryAsync();

                var query = @"
                    SELECT u.*, r.RoleName, r.RoleDescription 
                    FROM [User] u 
                    LEFT JOIN Role r ON u.UserRoleCode = r.Id 
                    WHERE u.Id = @Id AND u.IsActive = 1";

                var parameters = new { Id = id };
                var result = await clientRepository.ExecuteQueryAsync(query, parameters);
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
                var clientRepository = await GetClientRepositoryAsync();

                var query = @"
                    SELECT u.*, r.RoleName, r.RoleDescription 
                    FROM [User] u 
                    LEFT JOIN Role r ON u.UserRoleCode = r.Id 
                    WHERE u.UserName = @Username AND u.IsActive = 1";

                var parameters = new { Username = username };
                var result = await clientRepository.ExecuteQueryAsync(query, parameters);
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
                var clientRepository = await GetClientRepositoryAsync();

                var query = @"
                    SELECT u.*, r.RoleName, r.RoleDescription 
                    FROM [User] u 
                    LEFT JOIN Role r ON u.UserRoleCode = r.Id 
                    WHERE u.UserEmail = @Email AND u.IsActive = 1";

                var parameters = new { Email = email };
                var result = await clientRepository.ExecuteQueryAsync(query, parameters);
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
                var clientRepository = await GetClientRepositoryAsync();

                var query = @"
                    SELECT u.*, r.RoleName, r.RoleDescription 
                    FROM [Users] u 
                    LEFT JOIN Roles r ON u.UserRoleCode = r.Id 
                    WHERE u.IsActive = 1
                    ORDER BY u.Name";

                var result = await clientRepository.ExecuteQueryAsync(query);
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
                var clientRepository = await GetClientRepositoryAsync();

                var query = @"
                    SELECT u.*, r.RoleName, r.RoleDescription 
                    FROM [User] u 
                    LEFT JOIN Role r ON u.UserRoleCode = r.Id 
                    WHERE u.CompanyId = @CompanyId AND u.IsActive = 1
                    ORDER BY u.Name";

                var parameters = new { CompanyId = companyId };
                var result = await clientRepository.ExecuteQueryAsync(query, parameters);
                return result.Select(MapToUser);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting users by company ID {companyId}: {ex.Message}", ex);
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
                var clientRepository = await GetClientRepositoryAsync();

                var query = @"
                    INSERT INTO [User] (Name, UserName, Password, UserStatus, UserEmail, UserPhone, 
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

                var result = await clientRepository.ExecuteQueryAsync(query, parameters);
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
                var clientRepository = await GetClientRepositoryAsync();

                var query = @"
                    UPDATE [User] 
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

                await clientRepository.ExecuteCommandAsync(query, parameters);
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
                var clientRepository = await GetClientRepositoryAsync();

                var query = "UPDATE [User] SET IsActive = 0, ModifiedDate = @ModifiedDate WHERE Id = @Id";
                var parameters = new { Id = id, ModifiedDate = DateTime.UtcNow };

                var result = await clientRepository.ExecuteCommandAsync(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting user: {ex.Message}", ex);
            }
        }

        // ... (Continue with remaining methods using the same pattern)
        // I'll show a few more key methods:

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
                var clientRepository = await GetClientRepositoryAsync();

                var query = @"
                    UPDATE [User] 
                    SET Password = @Password, ModifiedDate = @ModifiedDate 
                    WHERE Id = @Id";

                var parameters = new
                {
                    Password = newPasswordHash,
                    Id = userId,
                    ModifiedDate = DateTime.UtcNow
                };

                var result = await clientRepository.ExecuteCommandAsync(query, parameters);
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
                var clientRepository = await GetClientRepositoryAsync();

                var query = "SELECT COUNT(1) FROM [User] WHERE UserName = @Username";
                var parameters = new { Username = username };

                var result = await clientRepository.ExecuteQueryAsync(query, parameters);
                var count = Convert.ToInt32(result.FirstOrDefault());
                return count > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking username existence: {ex.Message}", ex);
            }
        }

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
                CreatedDate = ConvertToDateTime(data.CreatedDate),
                ModifiedDate = data.ModifiedDate != null ? ConvertToDateTime(data.ModifiedDate) : (DateTime?)null,
                IsActive = ConvertToBool(data.IsActive),
                Role = data.RoleName != null ? new Role
                {
                    Id = (int)data.UserRoleCode,
                    RoleName = data.RoleName,
                    RoleDescription = data.RoleDescription
                } : null
            };
        }

        private bool ConvertToBool(object value)
        {
            if (value == null || value is DBNull)
                return false;
            if (value is bool b)
                return b;
            if (value is int i)
                return i != 0;
            if (value is long l)
                return l != 0;
            if (bool.TryParse(value.ToString(), out var result))
                return result;
            if (long.TryParse(value.ToString(), out var l2))
                return l2 != 0;
            return false;
        }

        private DateTime ConvertToDateTime(object value)
        {
            if (value == null || value is DBNull)
                return DateTime.MinValue;
            if (value is DateTime dt)
                return dt;
            if (DateTime.TryParse(value.ToString(), out var result))
                return result;
            throw new InvalidCastException($"Cannot convert value '{value}' to DateTime.");
        }
    }
}

