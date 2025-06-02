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
using Microsoft.AspNetCore.Identity.Data;
using Pml.Shared.DTOs.Client;
using Pml.Domain.IRepositories.Client;

namespace Pml.Application.Services
{
    /// <summary>
    /// Service for handling authentication logic such as login, token refresh, and token revocation.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IUserRepository _userRepositiry;
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger<AuthService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="tokenRepository">Token repository for generating and validating tokens.</param>
        /// <param name="systemAdminRepository">Repository for system admin users.</param>
        /// <param name="systemAdminRoleRepository">Repository for system admin roles.</param>
        /// <param name="logger">Logger instance.</param>
        public AuthService(
            ITokenRepository tokenRepository,
            ILogger<AuthService> logger)
        {
            _tokenRepository = tokenRepository;
            _logger = logger;
        }

        /// <summary>
        /// Authenticates a user and generates access and refresh tokens.
        /// </summary>
        /// <param name="request">Authentication request containing username and password.</param>
        /// <returns>Authentication response with tokens and user info.</returns>
        public async Task<AuthResponse> LoginAsync(AuthRequest request)
        {
            // Validate credentials
            var user = await _userRepositiry.GetByUsernameAsync(request.UserName);

            if (user == null)
            {
                _logger.LogWarning("Login attempt with invalid username: {Username}", request.UserName);
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Invalid username or password"
                };
            }

            // Verify password
            if (!VerifyPasswordHash(request.Password, user.Password))
            {
                _logger.LogWarning("Login attempt with invalid password for user: {Username}", request.UserName);
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Invalid username or password"
                };
            }

            // Get roles for the authenticated user
            var roles = await _roleRepository.GetRolesByUserIdAsync(user.Id);

            // Log authentication success (avoid logging sensitive details in production)
            _logger.LogInformation("User {Username} authenticated successfully", user.UserName);

            // Generate access and refresh tokens
            var accessToken = _tokenRepository.GenerateAccessToken(user.Id, user.UserName, user.UserEmail, roles);
            var refreshToken = _tokenRepository.GenerateRefreshToken();
            var refreshTokenExpiry = _tokenRepository.CalculateRefreshTokenExpiry();

            // Update refresh token in database
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = refreshTokenExpiry;
            await _userRepositiry.UpdateAsync(user);

            // Return authentication response
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

        /// <summary>
        /// Refreshes the access token using a valid refresh token.
        /// </summary>
        /// <param name="request">Refresh token request containing expired access token and refresh token.</param>
        /// <returns>Authentication response with new tokens and user info.</returns>
        public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            try
            {
                // Get principal from expired access token
                var principal = _tokenRepository.GetPrincipalFromExpiredToken(request.AccessToken);
                var username = principal.Identity.Name;

                // Get user by username
                var user = await _userRepositiry.GetByUsernameAsync(username);
                if (user == null)
                {
                    return new AuthResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid token"
                    };
                }

                // Validate refresh token and its expiry
                if (user.RefreshToken != request.RefreshToken ||
                    user.RefreshTokenExpiry <= DateTime.UtcNow)
                {
                    return new AuthResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid or expired refresh token"
                    };
                }

                // Get current roles for the user
                var roles = await _roleRepository.GetRolesByUserIdAsync(user.Id);

                // Generate new access and refresh tokens
                var newAccessToken = _tokenRepository.GenerateAccessToken(user.Id, user.UserName, user.UserEmail, roles);
                var newRefreshToken = _tokenRepository.GenerateRefreshToken();
                var refreshTokenExpiry = _tokenRepository.CalculateRefreshTokenExpiry();

                // Update refresh token in database
                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiry = refreshTokenExpiry;
                await _userRepositiry.UpdateAsync(user);

                // Return authentication response
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

        /// <summary>
        /// Revokes the refresh token for a user, effectively logging them out.
        /// </summary>
        /// <param name="username">Username of the user whose token is to be revoked.</param>
        /// <returns>True if token was revoked, otherwise false.</returns>
        public async Task<bool> RevokeTokenAsync(string username)
        {
            var user = await _userRepositiry.GetByUsernameAsync(username);
            if (user == null)
            {
                return false;
            }

            // Revoke refresh token by clearing it and setting expiry to now
            user.RefreshToken = null;
            user.RefreshTokenExpiry = DateTime.UtcNow; // Set to past date
            await _userRepositiry.UpdateAsync(user);

            _logger.LogInformation("Token revoked for user: {Username}", username);
            return true;
        }

        /// <summary>
        /// Verifies the provided password against the stored hash.
        /// </summary>
        /// <param name="password">Plain text password.</param>
        /// <param name="storedHash">Hashed password from the database.</param>
        /// <returns>True if password matches the hash, otherwise false.</returns>
        private bool VerifyPasswordHash(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }
}