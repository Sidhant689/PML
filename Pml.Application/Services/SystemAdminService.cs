using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pml.Application.IServices;
using Pml.Domain.Entities.Models.Master;
using Pml.Domain.IRepositories.Master;

namespace Pml.Application.Services
{
    public class SystemAdminService : ISystemAdminService
    {
        private readonly ISystemAdminRepository _repository;

        public SystemAdminService(ISystemAdminRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Creates a new system admin user.
        /// </summary>
        /// <param name="admin">The admin user to create.</param>
        /// <returns>The created admin user.</returns>
        public async Task<SystemAdminUser> CreateAdminAsync(SystemAdminUser admin)
        {
            try
            {
                return await _repository.CreateAdminAsync(admin);
            }
            catch (Exception ex)
            {
                // Log exception here if logging is available
                throw new ApplicationException("An error occurred while creating the admin user.", ex);
            }
        }

        /// <summary>
        /// Deletes a system admin user by ID.
        /// </summary>
        /// <param name="id">The ID of the admin user to delete.</param>
        /// <returns>True if deleted, otherwise false.</returns>
        public async Task<bool> DeleteAdminAsync(int id)
        {
            try
            {
                return await _repository.DeleteAdminAsync(id);
            }
            catch (Exception ex)
            {
                // Log exception here if logging is available
                throw new ApplicationException($"An error occurred while deleting the admin user with ID {id}.", ex);
            }
        }

        /// <summary>
        /// Gets all system admin users.
        /// </summary>
        /// <returns>A collection of all admin users.</returns>
        public async Task<IEnumerable<SystemAdminUser>> GetAllAdminsAsync()
        {
            try
            {
                return await _repository.GetAllAdminsAsync();
            }
            catch (Exception ex)
            {
                // Log exception here if logging is available
                throw new ApplicationException("An error occurred while retrieving all admin users.", ex);
            }
        }

        /// <summary>
        /// Gets a system admin user by ID.
        /// </summary>
        /// <param name="id">The ID of the admin user.</param>
        /// <returns>The admin user if found; otherwise, null.</returns>
        public async Task<SystemAdminUser> GetByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                // Log exception here if logging is available
                throw new ApplicationException($"An error occurred while retrieving the admin user with ID {id}.", ex);
            }
        }

        /// <summary>
        /// Gets a system admin user by refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <returns>The admin user if found; otherwise, null.</returns>
        public async Task<SystemAdminUser> GetByRefreshTokenAsync(string refreshToken)
        {
            try
            {
                return await _repository.GetByRefreshTokenAsync(refreshToken);
            }
            catch (Exception ex)
            {
                // Log exception here if logging is available
                throw new ApplicationException("An error occurred while retrieving the admin user by refresh token.", ex);
            }
        }

        /// <summary>
        /// Gets a system admin user by username.
        /// </summary>
        /// <param name="username">The username of the admin user.</param>
        /// <returns>The admin user if found; otherwise, null.</returns>
        public async Task<SystemAdminUser> GetByUsernameAsync(string username)
        {
            try
            {
                return await _repository.GetByUsernameAsync(username);
            }
            catch (Exception ex)
            {
                // Log exception here if logging is available
                throw new ApplicationException($"An error occurred while retrieving the admin user with username '{username}'.", ex);
            }
        }

        /// <summary>
        /// Updates an existing system admin user.
        /// </summary>
        /// <param name="admin">The admin user to update.</param>
        /// <returns>The updated admin user.</returns>
        public async Task<SystemAdminUser> UpdateAdminAsync(SystemAdminUser admin)
        {
            try
            {
                return await _repository.UpdateAdminAsync(admin);
            }
            catch (Exception ex)
            {
                // Log exception here if logging is available
                throw new ApplicationException("An error occurred while updating the admin user.", ex);
            }
        }
    }
}
