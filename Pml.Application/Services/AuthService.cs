using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using Pml.Application.IServices;
using Pml.Domain.Authentication;
using Pml.Domain.IRepositories.Master;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.Data;
using Pml.Shared.DTOs.Client;
using Pml.Domain.IRepositories.Client;
using Pml.Infrastructure.Client.ClientRepositories;
using Pml.Infrastructure.Client;
using Pml.Shared.Entities.Models.Client;

namespace Pml.Application.Services
{
    /// <summary>
    /// Service for handling authentication logic such as login, token refresh, and token revocation.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IClientRepositoryProvider _clientRepositoryProvider;
        private readonly ILogger<AuthService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthService"/> class.
        /// </summary>
        /// <param name="tokenRepository">Token repository for generating and validating tokens.</param>
        /// <param name="companyRepository">Repository for company operations.</param>
        /// <param name="clientRepositoryProvider">Provider for client-specific repositories.</param>
        /// <param name="logger">Logger instance.</param>
        public AuthService(
            ITokenRepository tokenRepository,
            ICompanyRepository companyRepository,
            IClientRepositoryProvider clientRepositoryProvider,
            ILogger<AuthService> logger)
        {
            _tokenRepository = tokenRepository;
            _companyRepository = companyRepository;
            _clientRepositoryProvider = clientRepositoryProvider;
            _logger = logger;
        }

        /// <summary>
        /// Authenticates a user and generates access and refresh tokens.
        /// </summary>
        /// <param name="request">Authentication request containing username and password.</param>
        /// <returns>Authentication response with tokens and user info.</returns>
        public async Task<AuthResponse> LoginAsync(AuthRequest request)
        {
            try
            {
                // Step 1: Find user across all company databases or in a specific company
                var (user, companyId) = await FindUserAsync(request.UserName, request.CompanyId);

                if (user == null)
                {
                    _logger.LogWarning("Login attempt with invalid username: {Username}", request.UserName);
                    return new AuthResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid username or password"
                    };
                }

                // Step 2: Verify password
                if (!VerifyPasswordHash(request.Password, user.Password))
                {
                    _logger.LogWarning("Login attempt with invalid password for user: {Username}", request.UserName);
                    return new AuthResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid username or password"
                    };
                }

                // Step 3: Check if user is active
                if (!user.IsActive || user.UserStatus != "Active")
                {
                    _logger.LogWarning("Login attempt for inactive user: {Username}", request.UserName);
                    return new AuthResponse
                    {
                        IsSuccess = false,
                        Message = "User account is not active"
                    };
                }

                // Step 4: Get user roles
                var roles = new List<string>();
                if (user.Role != null)
                {
                    roles.Add(user.Role.RoleName);
                }

                // Step 5: Log authentication success
                _logger.LogInformation("User {Username} from company {CompanyId} authenticated successfully",
                    user.UserName, companyId);

                // Step 6: Generate access and refresh tokens
                var accessToken = _tokenRepository.GenerateAccessToken(
                    user.Id,
                    user.UserName,
                    user.UserEmail,
                    roles,
                    companyId); // Include company ID in token

                var refreshToken = _tokenRepository.GenerateRefreshToken();
                var refreshTokenExpiry = _tokenRepository.CalculateRefreshTokenExpiry();

                // Step 7: Update refresh token in user's company database
                await UpdateUserRefreshTokenAsync(user.Id, companyId, refreshToken, refreshTokenExpiry);

                // Step 8: Return authentication response
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed for username: {Username}", request.UserName);
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Authentication failed due to an internal error"
                };
            }
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
                // Step 1: Get principal from expired access token
                var principal = _tokenRepository.GetPrincipalFromExpiredToken(request.AccessToken);
                var username = principal.Identity.Name;
                var companyIdClaim = principal.FindFirst("CompanyId")?.Value;

                if (string.IsNullOrEmpty(companyIdClaim) || !int.TryParse(companyIdClaim, out int companyId))
                {
                    return new AuthResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid token - company information missing"
                    };
                }

                // Step 2: Get user from the specific company database
                var user = await GetUserFromCompanyAsync(username, companyId);
                if (user == null)
                {
                    return new AuthResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid token - user not found"
                    };
                }

                // Step 3: Validate refresh token and its expiry
                if (user.RefreshToken != request.RefreshToken ||
                    user.RefreshTokenExpiry <= DateTime.UtcNow)
                {
                    return new AuthResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid or expired refresh token"
                    };
                }

                // Step 4: Get current roles for the user
                var roles = new List<string>();
                if (user.Role != null)
                {
                    roles.Add(user.Role.RoleName);
                }

                // Step 5: Generate new access and refresh tokens
                var newAccessToken = _tokenRepository.GenerateAccessToken(
                    user.Id,
                    user.UserName,
                    user.UserEmail,
                    roles,
                    companyId);

                var newRefreshToken = _tokenRepository.GenerateRefreshToken();
                var refreshTokenExpiry = _tokenRepository.CalculateRefreshTokenExpiry();

                // Step 6: Update refresh token in database
                await UpdateUserRefreshTokenAsync(user.Id, companyId, newRefreshToken, refreshTokenExpiry);

                // Step 7: Return authentication response
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
                    Roles = roles
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token refresh failed");
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
        /// <param name="companyId">Company ID where the user belongs.</param>
        /// <returns>True if token was revoked, otherwise false.</returns>
        public async Task<bool> RevokeTokenAsync(string username, int companyId)
        {
            try
            {
                var user = await GetUserFromCompanyAsync(username, companyId);
                if (user == null)
                {
                    return false;
                }

                // Revoke refresh token by clearing it and setting expiry to past date
                await UpdateUserRefreshTokenAsync(user.Id, companyId, null, DateTime.UtcNow);

                _logger.LogInformation("Token revoked for user: {Username} in company: {CompanyId}",
                    username, companyId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to revoke token for user: {Username}", username);
                return false;
            }
        }

        /// <summary>
        /// Finds a user by username, either in a specific company or across all companies.
        /// </summary>
        /// <param name="username">Username to search for.</param>
        /// <param name="companyId">Optional company ID to search in specific company.</param>
        /// <returns>Tuple containing the user and company ID where found.</returns>
        private async Task<(User user, int companyId)> FindUserAsync(string username, int? companyId = null)
        {
            if (companyId.HasValue)
            {
                // Search in specific company
                var user = await GetUserFromCompanyAsync(username, companyId.Value);
                return (user, companyId.Value);
            }
            else
            {
                // Search across all companies (this might be expensive - consider optimization)
                var companies = await _companyRepository.GetAllActiveCompaniesAsync();

                foreach (var company in companies)
                {
                    var user = await GetUserFromCompanyAsync(username, company.Id);
                    if (user != null)
                    {
                        return (user, company.Id);
                    }
                }

                return (null, 0);
            }
        }

        /// <summary>
        /// Gets a user from a specific company's database.
        /// </summary>
        /// <param name="username">Username to search for.</param>
        /// <param name="companyId">Company ID.</param>
        /// <returns>User entity or null if not found.</returns>
        private async Task<User> GetUserFromCompanyAsync(string username, int companyId)
        {
            try
            {
                // Get company database configuration
                var companyDatabase = await _companyRepository.GetDefaultDatabaseAsync(companyId);
                if (companyDatabase == null)
                {
                    return null;
                }

                // Create client repository for this company
                var clientRepository = await _clientRepositoryProvider
                    .GetRepositoryForCompanyAsync(companyId);

                // Create user repository and get user
                var userRepository = new UserRepository(clientRepository);
                return await userRepository.GetByUsernameAsync(username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user {Username} from company {CompanyId}",
                    username, companyId);
                return null;
            }
        }

        /// <summary>
        /// Updates user's refresh token in the company database.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <param name="companyId">Company ID.</param>
        /// <param name="refreshToken">New refresh token.</param>
        /// <param name="expiry">Token expiry date.</param>
        private async Task UpdateUserRefreshTokenAsync(int userId, int companyId,
            string refreshToken, DateTime expiry)
        {
            try
            {
                var clientRepository = await _clientRepositoryProvider
                    .GetRepositoryForCompanyAsync(companyId);

                var query = @"
                    UPDATE [Users] 
                    SET RefreshToken = @RefreshToken, 
                        RefreshTokenExpiry = @RefreshTokenExpiry,
                        ModifiedDate = @ModifiedDate
                    WHERE Id = @UserId";

                var parameters = new
                {
                    RefreshToken = refreshToken,
                    RefreshTokenExpiry = expiry,
                    ModifiedDate = DateTime.UtcNow,
                    UserId = userId
                };

                await clientRepository.ExecuteCommandAsync(query, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating refresh token for user {UserId} in company {CompanyId}",
                    userId, companyId);
                throw;
            }
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