using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pml.Shared.DTOs.Master.Authentication;
using Pml.Application.IServices;
using System.Security.Claims;

namespace Pml.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemAdminAuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public SystemAdminAuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest request)
        {
            var response = await _authService.LoginAsync(request);
            if (!response.IsSuccess)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var response = await _authService.RefreshTokenAsync(request);
            if (!response.IsSuccess)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }

        //[Authorize]
        [HttpGet("validate")]
        public async Task<IActionResult> ValidateToken()
        {
            try
            {
                // Extract user information from the JWT token claims
                var username = User.Identity?.Name;
                var userId = User.FindFirst("userId")?.Value;
                var email = User.FindFirst("email")?.Value;
                var roles = User.FindAll("role").Select(c => c.Value).ToList();

                if (string.IsNullOrEmpty(username))
                {
                    return Unauthorized(new AuthResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid token"
                    });
                }

                return Ok(new AuthResponse
                {
                    IsSuccess = true,
                    Message = "Token is valid",
                    Username = username,
                    UserId = !string.IsNullOrEmpty(userId) ? Convert.ToInt32(userId) : 0,
                    Email = email,
                    Roles = roles
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Token validation failed"
                });
            }
        }

        //[Authorize]
        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke()
        {
            var username = User.Identity.Name;
            var result = await _authService.RevokeTokenAsync(username);
            if (!result)
            {
                return BadRequest(new { message = "Failed to revoke token" });
            }
            return Ok(new { message = "Token revoked successfully" });
        }
    }
}