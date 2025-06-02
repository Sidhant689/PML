using Pml.Application.IServices;
using Pml.Domain.IRepositories.Client;
using Pml.Shared.Entities.Models.Client;

namespace Pml.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;
        public RoleService(IRoleRepository repository)
        {
            _repository = repository;
        }
        public async Task<Role> GetByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error fetching role by ID {id}", ex);
            }
        }
        public async Task<Role> GetByNameAsync(string roleName)
        {
            try
            {
                return await _repository.GetByNameAsync(roleName);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error fetching role by name {roleName}", ex);
            }
        }
        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error fetching all roles", ex);
            }
        }
        public async Task<IEnumerable<Role>> GetActiveRolesAsync()
        {
            try
            {
                return await _repository.GetActiveRolesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error fetching active roles", ex);
            }
        }
        public async Task<Role> CreateAsync(Role role)
        {
            try
            {
                return await _repository.CreateAsync(role);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error creating role", ex);
            }
        }
        public async Task<Role> UpdateAsync(Role role)
        {
            try
            {
                return await _repository.UpdateAsync(role);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error updating role with ID {role.Id}", ex);
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
                throw new ApplicationException($"Error deleting role with ID {id}", ex);
            }
        }
        public async Task<bool> ActivateRoleAsync(int id)
        {
            try
            {
                return await _repository.ActivateRoleAsync(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error activating role with ID {id}", ex);
            }
        }
        public async Task<bool> DeactivateRoleAsync(int id)
        {
            try
            {
                return await _repository.DeactivateRoleAsync(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error deactivating role with ID {id}", ex);
            }
        }
        public async Task<bool> RoleNameExistsAsync(string roleName)
        {
            try
            {
                return await _repository.RoleNameExistsAsync(roleName);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error checking if role name exists: {roleName}", ex);
            }
        }
        public async Task<int> GetUserCountByRoleAsync(int roleId)
        {
            try
            {
                return await _repository.GetUserCountByRoleAsync(roleId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error fetching user count for role ID {roleId}", ex);
            }
        }
        public async Task<IEnumerable<string>> GetRolesByUserIdAsync(int userId)
        {
            try
            {
                return await _repository.GetRolesByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error fetching roles for user ID {userId}", ex);
            }
        }
        public void DeleteRoleAsync(int roleId)
        {
            try
            {
                _repository.DeleteAsync(roleId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error deleting role with ID {roleId}", ex);
            }
        }


    }
}
