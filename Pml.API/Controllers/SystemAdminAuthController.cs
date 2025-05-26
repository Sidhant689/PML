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

        /// <summary>
        /// Constructor for SystemAdminAuthController.
        /// </summary>
        /// <param name="authService">Authentication service dependency.</param>
        public SystemAdminAuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Authenticates a user and returns tokens if successful.
        /// </summary>
        /// <param name="request">Authentication request containing username and password.</param>
        /// <returns>Authentication response with tokens or unauthorized result.</returns>
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

        /// <summary>
        /// Refreshes the access token using a valid refresh token.
        /// </summary>
        /// <param name="request">Refresh token request containing access and refresh tokens.</param>
        /// <returns>Authentication response with new tokens or unauthorized result.</returns>
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

        /// <summary>
        /// Validates the current JWT token and returns user information if valid.
        /// </summary>
        /// <returns>Authentication response with user details or unauthorized result.</returns>
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
                // Return unauthorized if token validation fails
                return Unauthorized(new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Token validation failed"
                });
            }
        }

        /// <summary>
        /// Revokes the current user's token.
        /// </summary>
        /// <returns>Result indicating whether the token was successfully revoked.</returns>
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