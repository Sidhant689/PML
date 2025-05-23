using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pml.Domain.Entities.Models;

namespace Pml.Domain.IRepositories.Master
{
    public interface ISystemAdminRepository
    {
        Task<SystemAdminUser> GetByIdAsync(int id);
        Task<SystemAdminUser> GetByUsernameAsync(string username);
        Task<SystemAdminUser> GetByRefreshTokenAsync(string refreshToken);
        Task<SystemAdminUser> CreateAdminAsync(SystemAdminUser admin);
        Task<SystemAdminUser> UpdateAdminAsync(SystemAdminUser admin);
        Task<bool> DeleteAdminAsync(int id);
        Task<IEnumerable<SystemAdminUser>> GetAllAdminsAsync();
    }
}
