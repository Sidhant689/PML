using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pml.Domain.Entities.Models.Master;

namespace Pml.Application.IServices
{
    public interface ISystemAdminService
    {
        Task<SystemAdminUser> CreateAdminAsync(SystemAdminUser admin);
        Task<SystemAdminUser> UpdateAdminAsync(SystemAdminUser admin);
        Task<bool> DeleteAdminAsync(int id);
        Task<IEnumerable<SystemAdminUser>> GetAllAdminsAsync();
        Task<SystemAdminUser> GetByIdAsync(int id);
        Task<SystemAdminUser> GetByRefreshTokenAsync(string refreshToken);
        Task<SystemAdminUser> GetByUsernameAsync(string username);
    }
}
