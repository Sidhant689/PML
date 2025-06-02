using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pml.Application.IServices;
using Pml.Shared.DTOs.Client;
using Pml.Shared.Entities.Models.Client;

namespace Pml.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> GetRoleById(int id)
        {
            var role = await _roleService.GetByIdAsync(id);
            if (role == null)
                return NotFound();

            return Ok(MapToDto(role));
        }

        [HttpGet("by-name/{roleName}")]
        public async Task<ActionResult<RoleDto>> GetRoleByName(string roleName)
        {
            var role = await _roleService.GetByNameAsync(roleName);
            if (role == null)
                return NotFound();

            return Ok(MapToDto(role));
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetAllRoles()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles.Select(MapToDto));
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetActiveRoles()
        {
            var roles = await _roleService.GetActiveRolesAsync();
            return Ok(roles.Select(MapToDto));
        }

        [HttpPost]
        public async Task<ActionResult<RoleDto>> CreateRole([FromBody] RoleDto dto)
        {
            var role = new Role
            {
                RoleName = dto.RoleName,
                RoleDescription = dto.RoleDescription,
                RoleStatus = dto.RoleStatus,
                IsActive = dto.IsActive,
                CreatedDate = DateTime.UtcNow
            };

            var created = await _roleService.CreateAsync(role);
            return CreatedAtAction(nameof(GetRoleById), new { id = created.Id }, MapToDto(created));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RoleDto>> UpdateRole(int id, [FromBody] RoleDto dto)
        {
            var existing = await _roleService.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            existing.RoleName = dto.RoleName;
            existing.RoleDescription = dto.RoleDescription;
            existing.RoleStatus = dto.RoleStatus;
            existing.IsActive = dto.IsActive;

            var updated = await _roleService.UpdateAsync(existing);
            return Ok(MapToDto(updated));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var success = await _roleService.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }

        [HttpPost("activate/{id}")]
        public async Task<IActionResult> ActivateRole(int id)
        {
            var result = await _roleService.ActivateRoleAsync(id);
            return result ? Ok() : NotFound();
        }

        [HttpPost("deactivate/{id}")]
        public async Task<IActionResult> DeactivateRole(int id)
        {
            var result = await _roleService.DeactivateRoleAsync(id);
            return result ? Ok() : NotFound();
        }

        [HttpGet("exists/{roleName}")]
        public async Task<ActionResult<bool>> RoleNameExists(string roleName)
        {
            var exists = await _roleService.RoleNameExistsAsync(roleName);
            return Ok(exists);
        }

        [HttpGet("user-count/{roleId}")]
        public async Task<ActionResult<int>> GetUserCountByRole(int roleId)
        {
            var count = await _roleService.GetUserCountByRoleAsync(roleId);
            return Ok(count);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<string>>> GetRolesByUserId(int userId)
        {
            var roles = await _roleService.GetRolesByUserIdAsync(userId);
            return Ok(roles);
        }

        // Helper method to convert Entity to DTO
        private RoleDto MapToDto(Role role)
        {
            return new RoleDto
            {
                Id = role.Id,
                RoleName = role.RoleName,
                RoleDescription = role.RoleDescription,
                RoleStatus = role.RoleStatus,
                IsActive = role.IsActive
            };
        }
    }
}
