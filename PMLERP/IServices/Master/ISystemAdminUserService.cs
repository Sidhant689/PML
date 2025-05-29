using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pml.Shared.DTOs.Master.SystemAdminDTOs;

namespace PMLERP.IServices.Master
{
    public interface ISystemAdminUserService
    {
        /// <summary>
        /// Creates a new system admin user.
        /// </summary>
        /// <param name="dto">The DTO containing admin details.</param>
        /// <returns>The created admin user details.</returns>
        Task<SystemAdminUserDto> CreateAdminAsync(SystemAdminUserDto dto);
        /// <summary>
        /// Updates an existing system admin user.
        /// </summary>
        /// <param name="dto">The DTO containing updated admin details.</param>
        /// <returns>The updated admin user details.</returns>
        Task<SystemAdminUserDto> UpdateAdminAsync(SystemAdminUserDto dto);
        /// <summary>
        /// Deletes a system admin user by ID.
        /// </summary>
        /// <param name="id">The ID of the admin user to delete.</param>
        /// <returns>True if deletion was successful, otherwise false.</returns>
        Task<bool> DeleteAdminAsync(int id);
        Task<SystemAdminUserDto> GetAdminByIdAsync(int id);
        Task<List<SystemAdminUserDto>> GetAllAdminsAsync();
    }
}
