using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pml.Application.IServices;
using Pml.Shared.DTOs.Client;
using Pml.Shared.Entities.Models.Client;

namespace Pml.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();

            return Ok(MapToDto(user));
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users.Select(MapToDto));
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto dto)
        {
            var user = new User
            {
                Name = dto.Name,
                UserName = dto.UserName,
                Password = dto.Password,
                UserStatus = dto.UserStatus,
                UserEmail = dto.UserEmail,
                UserPhone = dto.UserPhone,
                UserAddress = dto.UserAddress,
                UserRoleCode = dto.UserRoleCode,
                CompanyId = dto.CompanyId,
            };

            var created = await _userService.CreateAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = created.Id }, MapToDto(created));
        }

        [HttpPut]
        public async Task<ActionResult<UserDto>> UpdateUser([FromBody] UpdateUserDto dto)
        {
            var existing = await _userService.GetByIdAsync(dto.Id);
            if (existing == null) return NotFound();

            existing.Name = dto.Name;
            existing.UserName = dto.UserName;
            existing.UserEmail = dto.UserEmail;
            existing.UserPhone = dto.UserPhone;
            existing.UserAddress = dto.UserAddress;
            existing.UserStatus = dto.UserStatus;
            existing.UserRoleCode = dto.UserRoleCode;
            existing.CompanyId = dto.CompanyId;
            existing.IsActive = dto.IsActive;
            existing.ModifiedDate = DateTime.UtcNow;

            var updated = await _userService.UpdateAsync(existing);
            return Ok(MapToDto(updated));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var success = await _userService.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }

        //[HttpPost("activate/{id}")]
        //public async Task<IActionResult> ActivateUser(int id)
        //{
        //    var result = await _userService.ActivateUserAsync(id);
        //    return result ? Ok() : NotFound();
        //}

        //[HttpPost("deactivate/{id}")]
        //public async Task<IActionResult> DeactivateUser(int id)
        //{
        //    var result = await _userService.DeactivateUserAsync(id);
        //    return result ? Ok() : NotFound();
        //}

        private UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.UserName,
                UserStatus = user.UserStatus,
                UserEmail = user.UserEmail,
                UserPhone = user.UserPhone,
                UserAddress = user.UserAddress,
                UserRoleCode = user.UserRoleCode,
                CompanyId = user.CompanyId,
                IsActive = user.IsActive,

            };
        }
    }
}
