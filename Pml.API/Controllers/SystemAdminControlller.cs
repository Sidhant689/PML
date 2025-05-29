using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pml.Shared.DTOs.Master.SystemAdminDTOs;
using Pml.Application.IServices;
using Pml.Shared.Entities.Models.Master;

namespace Pml.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemAdminController : ControllerBase
    {
        private readonly ISystemAdminService _adminService;

        // Constructor to inject the system admin service dependency
        public SystemAdminController(ISystemAdminService adminService)
        {
            _adminService = adminService;
        }

        /// <summary>
        /// Creates a new system admin user.
        /// </summary>
        /// <param name="dto">The DTO containing admin details.</param>
        /// <returns>The created admin user details.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreateAdmin([FromBody] CreateSystemAdminDto dto)
        {
            try
            {
                // Map DTO to entity and hash the password
                var admin = new SystemAdminUser
                {
                    Name = dto.Name,
                    UserName = dto.UserName,
                    Password = BCrypt.Net.BCrypt.HashPassword(dto.Password), // hash it
                    UserStatus = dto.UserStatus,
                    UserEmail = dto.UserEmail,
                    UserPhone = dto.UserPhone,
                    UserAddress = dto.UserAddress
                };

                // Call service to create admin
                var created = await _adminService.CreateAdminAsync(admin);

                // Map entity to response DTO
                var response = new SystemAdminResponseDto
                {
                    Id = created.Id,
                    Name = created.Name,
                    UserName = created.UserName,
                    UserStatus = created.UserStatus,
                    UserEmail = created.UserEmail,
                    UserPhone = created.UserPhone,
                    UserAddress = created.UserAddress
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Return 500 if any error occurs
                return StatusCode(500, $"Error creating admin: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing system admin user.
        /// </summary>
        /// <param name="dto">The DTO containing updated admin details.</param>
        /// <returns>The updated admin user details.</returns>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateAdmin([FromBody] UpdateSystemAdminDto dto)
        {
            try
            {
                // Map DTO to entity
                var admin = new SystemAdminUser
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    UserName = dto.UserName,
                    UserStatus = dto.UserStatus,
                    UserEmail = dto.UserEmail,
                    UserPhone = dto.UserPhone,
                    UserAddress = dto.UserAddress
                };

                // Call service to update admin
                var updated = await _adminService.UpdateAdminAsync(admin);

                if (updated == null)
                    return NotFound("Admin not found.");

                // Map entity to response DTO
                var response = new SystemAdminResponseDto
                {
                    Id = updated.Id,
                    Name = updated.Name,
                    UserName = updated.UserName,
                    UserStatus = updated.UserStatus,
                    UserEmail = updated.UserEmail,
                    UserPhone = updated.UserPhone,
                    UserAddress = updated.UserAddress
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Return 500 if any error occurs
                return StatusCode(500, $"Error updating admin: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all system admin users.
        /// </summary>
        /// <returns>A list of all admin users.</returns>
        [HttpGet("all")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAllAdmins()
        {
            try
            {
                // Call service to get all admins
                var admins = await _adminService.GetAllAdminsAsync();

                // Map entities to response DTOs
                var response = admins.Select(admin => new SystemAdminResponseDto
                {
                    Id = admin.Id,
                    Name = admin.Name,
                    UserName = admin.UserName,
                    UserStatus = admin.UserStatus,
                    UserEmail = admin.UserEmail,
                    UserPhone = admin.UserPhone,
                    UserAddress = admin.UserAddress
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Return 500 if any error occurs
                return StatusCode(500, $"Error retrieving admins: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a system admin user by ID.
        /// </summary>
        /// <param name="id">The ID of the admin to delete.</param>
        /// <returns>Status of the delete operation.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            try
            {
                // Call service to delete admin
                var result = await _adminService.DeleteAdminAsync(id);
                if (!result)
                    return NotFound("Admin not found.");
                return Ok("Admin deleted successfully.");
            }
            catch (Exception ex)
            {
                // Return 500 if any error occurs
                return StatusCode(500, $"Error deleting admin: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a system admin user by ID.
        /// </summary>
        /// <param name="id">The ID of the admin to retrieve.</param>
        /// <returns>The admin user details.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                // Call service to get admin by ID
                var admin = await _adminService.GetByIdAsync(id);
                if (admin == null)
                    return NotFound("Admin not found.");

                // Map entity to response DTO
                var response = new SystemAdminResponseDto
                {
                    Id = admin.Id,
                    Name = admin.Name,
                    UserName = admin.UserName,
                    UserStatus = admin.UserStatus,
                    UserEmail = admin.UserEmail,
                    UserPhone = admin.UserPhone,
                    UserAddress = admin.UserAddress
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Return 500 if any error occurs
                return StatusCode(500, $"Error retrieving admin: {ex.Message}");
            }
        }
    }
}
