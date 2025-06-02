using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pml.Application.IServices;
using Pml.Shared.DTOs.Client;

namespace Pml.API.Controllers
{
    /// <summary>
    /// Controller for handling authentication operations like login, token refresh, and logout.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="authService">Authentication service.</param>
        /// <param name="logger">Logger instance.</param>
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Authenticates a user and returns access and refresh tokens.
        /// </summary>
        /// <param name="request">Authentication request containing username and password.</param>
        /// <returns>Authentication response with tokens and user information.</returns>
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new AuthResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid request data"
                    });
                }

                var response = await _authService.LoginAsync(request);

                if (!response.IsSuccess)
                {
                    return BadRequest(response);
                }

                _logger.LogInformation("User {Username} logged in successfully", request.UserName);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed for user {Username}", request.UserName);
                return StatusCode(500, new AuthResponse
                {
                    IsSuccess = false,
                    Message = "An internal server error occurred"
                });
            }
        }

        /// <summary>
        /// Refreshes the access token using a valid refresh token.
        /// </summary>
        /// <param name="request">Refresh token request.</param>
        /// <returns>New access and refresh tokens.</returns>
        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new AuthResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid request data"
                    });
                }

                var response = await _authService.RefreshTokenAsync(request);

                if (!response.IsSuccess)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token refresh failed");
                return StatusCode(500, new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Token refresh failed"
                });
            }
        }

        /// <summary>
        /// Logs out a user by revoking their refresh token.
        /// </summary>
        /// <returns>Success status of logout operation.</returns>
        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            try
            {
                var username = User.Identity?.Name;
                var companyIdClaim = User.FindFirst("CompanyId")?.Value;

                if (string.IsNullOrEmpty(username) ||
                    string.IsNullOrEmpty(companyIdClaim) ||
                    !int.TryParse(companyIdClaim, out int companyId))
                {
                    return BadRequest(new { message = "Invalid user session" });
                }

                var result = await _authService.RevokeTokenAsync(username, companyId);

                if (result)
                {
                    _logger.LogInformation("User {Username} logged out successfully", username);
                    return Ok(new { message = "Logout successful" });
                }

                return BadRequest(new { message = "Logout failed" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Logout failed");
                return StatusCode(500, new { message = "Logout failed due to internal error" });
            }
        }

        /// <summary>
        /// Gets the current user's information from the JWT token.
        /// </summary>
        /// <returns>Current user information.</returns>
        [HttpGet("me")]
        [Authorize]
        public ActionResult GetCurrentUser()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var username = User.Identity?.Name;
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                var companyId = User.FindFirst("CompanyId")?.Value;
                var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

                return Ok(new
                {
                    UserId = userId,
                    Username = username,
                    Email = email,
                    CompanyId = companyId,
                    Roles = roles
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get current user information");
                return StatusCode(500, new { message = "Failed to retrieve user information" });
            }
        }

        /// <summary>
        /// Validates if the current token is still valid.
        /// </summary>
        /// <returns>Token validation status.</returns>
        [HttpGet("validate")]
        [Authorize]
        public ActionResult ValidateToken()
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
    }

}
