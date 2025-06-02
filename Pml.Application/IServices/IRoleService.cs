using Pml.Shared.Entities.Models.Client;

namespace Pml.Application.IServices
{
    public interface IRoleService
    {
        Task<Role> GetByIdAsync(int id);
        Task<Role> GetByNameAsync(string roleName);
        Task<IEnumerable<Role>> GetAllAsync();
        Task<IEnumerable<Role>> GetActiveRolesAsync();
        Task<Role> CreateAsync(Role role);
        Task<Role> UpdateAsync(Role role);
        Task<bool> DeleteAsync(int id);
        Task<bool> ActivateRoleAsync(int id);
        Task<bool> DeactivateRoleAsync(int id);
        Task<bool> RoleNameExistsAsync(string roleName);
        Task<int> GetUserCountByRoleAsync(int roleId);
        Task<IEnumerable<string>> GetRolesByUserIdAsync(int userId);
    }
}
