using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pml.Shared.Entities.Models.Client;

namespace Pml.Application.IServices
{
    public interface IUserService
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<IEnumerable<User>> GetByCompanyIdAsync(int companyId);
        //Task<IEnumerable<User>> GetByRoleAsync(int roleId);
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
        //Task<bool> ActivateUserAsync(int id);
        //Task<bool> DeactivateUserAsync(int id);
        Task<bool> ChangePasswordAsync(int userId, string newPasswordHash);
        Task<bool> UsernameExistsAsync(string username);
        //Task<bool> EmailExistsAsync(string email);
    }
}
