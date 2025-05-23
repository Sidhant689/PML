using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using Pml.Shared.DTOs.Master.Authentication;
using Pml.Application.IServices;
using Pml.Domain.Authentication;
using Pml.Domain.IRepositories.Master;
using Microsoft.Extensions.Logging;

namespace Pml.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly ISystemAdminRepository _systemAdminRepository;
        private readonly ISystemAdminRoleRepository _systemAdminRoleRepository;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            ITokenRepository tokenRepository,
            ISystemAdminRepository systemAdminRepository,
            ISystemAdminRoleRepository systemAdminRoleRepository,
            ILogger<AuthService> logger)
        {
            _tokenRepository = tokenRepository;
            _systemAdminRepository = systemAdminRepository;
            _systemAdminRoleRepository = systemAdminRoleRepository;
            _logger = logger;
        }

        public async Task<AuthResponse> LoginAsync(AuthRequest request)
        {
            // Validate credentials
            var user = await _systemAdminRepository.GetByUsernameAsync(request.Username);

            if (user == null)
            {
                _logger.LogWarning("Login attempt with invalid username: {Username}", request.Username);
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Invalid username or password"
                };
            }

            // Verify password
            if (!VerifyPasswordHash(request.Password, user.Password))
            {
                _logger.LogWarning("Login attempt with invalid password for user: {Username}", request.Username);
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Invalid username or password"
                };
            }

            // Get roles
            var roles = await _systemAdminRoleRepository.GetRolesByAdminIdAsync(user.Id);

            // Log authentication success (don't log sensitive role details in production)
            _logger.LogInformation("User {Username} authenticated successfully", user.UserName);

            // Generate tokens
            var accessToken = _tokenRepository.GenerateAccessToken(user.Id, user.UserName, user.UserEmail, roles);
            var refreshToken = _tokenRepository.GenerateRefreshToken();
            var refreshTokenExpiry = _tokenRepository.CalculateRefreshTokenExpiry();

            // Update refresh token in database
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = refreshTokenExpiry;
            await _systemAdminRepository.UpdateAdminAsync(user);

            return new AuthResponse
            {
                IsSuccess = true,
                Message = "Authentication successful",
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                RefreshTokenExpiry = refreshTokenExpiry,
                UserId = user.Id,
                Username = user.UserName,
                Email = user.UserEmail,
                Roles = roles
            };
        }

        public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            try
            {
                // Get principal from expired token
                var principal = _tokenRepository.GetPrincipalFromExpiredToken(request.AccessToken);
                var username = principal.Identity.Name;

                // Get user
                var user = await _systemAdminRepository.GetByUsernameAsync(username);
                if (user == null)
                {
                    return new AuthResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid token"
                    };
                }

                // Validate refresh token
                if (user.RefreshToken != request.RefreshToken ||
                    user.RefreshTokenExpiry <= DateTime.UtcNow)
                {
                    return new AuthResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid or expired refresh token"
                    };
                }

                // Get current roles (important for security)
                var roles = await _systemAdminRoleRepository.GetRolesByAdminIdAsync(user.Id);

                // Generate new tokens WITH roles
                var newAccessToken = _tokenRepository.GenerateAccessToken(user.Id, user.UserName, user.UserEmail, roles);
                var newRefreshToken = _tokenRepository.GenerateRefreshToken();
                var refreshTokenExpiry = _tokenRepository.CalculateRefreshTokenExpiry();

                // Update refresh token in database
                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiry = refreshTokenExpiry;
                await _systemAdminRepository.UpdateAdminAsync(user);

                return new AuthResponse
                {
                    IsSuccess = true,
                    Message = "Token refresh successful",
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    RefreshTokenExpiry = refreshTokenExpiry,
                    UserId = user.Id,
                    Username = user.UserName,
                    Email = user.UserEmail,
                    Roles = roles // Include roles in response
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token refresh failed for user");
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Token refresh failed"
                };
            }
        }

        public async Task<bool> RevokeTokenAsync(string username)
        {
            var user = await _systemAdminRepository.GetByUsernameAsync(username);
            if (user == null)
            {
                return false;
            }

            // Revoke refresh token
            user.RefreshToken = null;
            user.RefreshTokenExpiry = DateTime.UtcNow; // Set to past date
            await _systemAdminRepository.UpdateAdminAsync(user);

            _logger.LogInformation("Token revoked for user: {Username}", username);
            return true;
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }
}