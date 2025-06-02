using Pml.Application.IServices;
using Pml.Domain.IRepositories.Client;
using Pml.Shared.Entities.Models.Client;

namespace Pml.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new ApplicationException($"Error fetching user by ID {id}", ex);
            }
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            try
            {
                return await _repository.GetByUsernameAsync(username);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error fetching user by username {username}", ex);
            }
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            try
            {
                return await _repository.GetByEmailAsync(email);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error fetching user by email {email}", ex);
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error fetching all users", ex);
            }
        }

        public async Task<IEnumerable<User>> GetByCompanyIdAsync(int companyId)
        {
            try
            {
                return await _repository.GetByCompanyIdAsync(companyId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error fetching users by company ID {companyId}", ex);
            }
        }

        //public async Task<IEnumerable<User>> GetByRoleAsync(int roleId)
        //{
        //    try
        //    {
        //        //return await _repository.GetByRoleAsync(roleId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ApplicationException($"Error fetching users by role ID {roleId}", ex);
        //    }
        //}

        public async Task<User> CreateAsync(User user)
        {
            try
            {
                return await _repository.CreateAsync(user);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error creating user", ex);
            }
        }

        public async Task<User> UpdateAsync(User user)
        {
            try
            {
                return await _repository.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error updating user with ID {user.Id}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error deleting user with ID {id}", ex);
            }
        }

        //public async Task<bool> ActivateUserAsync(int id)
        //{
        //    try
        //    {
        //        //return await _repository.ActivateUserAsync(id);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ApplicationException($"Error activating user with ID {id}", ex);
        //    }
        //}

        //public async Task<bool> DeactivateUserAsync(int id)
        //{
        //    try
        //    {
        //        //return await _repository.DeactivateUserAsync(id);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ApplicationException($"Error deactivating user with ID {id}", ex);
        //    }
        //}

        public async Task<bool> ChangePasswordAsync(int userId, string newPasswordHash)
        {
            try
            {
                return await _repository.ChangePasswordAsync(userId, newPasswordHash);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error changing password for user ID {userId}", ex);
            }
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            try
            {
                return await _repository.UsernameExistsAsync(username);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error checking if username exists: {username}", ex);
            }
        }

        //public async Task<bool> EmailExistsAsync(string email)
        //{
        //    try
        //    {
        //        return await _repository.EmailExistsAsync(email);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ApplicationException($"Error checking if email exists: {email}", ex);
        //    }
        //}
    }
}
