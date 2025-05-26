using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pml.Domain.Authentication;
using Pml.Domain.Entities.Settings;

namespace Pml.Infrastructure.Authentication
{
    /// <summary>
    /// Repository for handling JWT access and refresh token generation and validation.
    /// </summary>
    public class TokenRepository : ITokenRepository
    {
        private readonly JwtSettings _jwtSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenRepository"/> class.
        /// </summary>
        /// <param name="jwtSettings">JWT settings injected via IOptions.</param>
        public TokenRepository(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        /// <summary>
        /// Generates a JWT access token for the specified user and roles.
        /// </summary>
        /// <param name="userId">The user's unique identifier.</param>
        /// <param name="username">The user's username.</param>
        /// <param name="email">The user's email address.</param>
        /// <param name="roles">Optional list of user roles.</param>
        /// <returns>Signed JWT access token as a string.</returns>
        public string GenerateAccessToken(int userId, string username, string email, IEnumerable<string> roles = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret));
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Email, email ?? string.Empty)
                };

            // Add roles to claims if provided
            if (roles != null && roles.Any())
            {
                Console.WriteLine("Adding the following roles to token:");
                foreach (var role in roles)
                {
                    if (!string.IsNullOrWhiteSpace(role))
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.Trim()));
                        Console.WriteLine($"- Added role: '{role.Trim()}'");
                    }
                }
            }
            else
            {
                Console.WriteLine("No roles provided for token generation");
            }

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the JWT token
            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpiryMinutes),
                signingCredentials: creds
            );

            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Generates a secure random refresh token.
        /// </summary>
        /// <returns>Base64-encoded refresh token string.</returns>
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// Calculates the expiry date for a refresh token based on settings.
        /// </summary>
        /// <returns>UTC expiry date for the refresh token.</returns>
        public DateTime CalculateRefreshTokenExpiry()
        {
            return DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays);
        }

        /// <summary>
        /// Extracts the claims principal from an expired JWT token.
        /// </summary>
        /// <param name="token">The expired JWT token.</param>
        /// <returns>ClaimsPrincipal extracted from the token.</returns>
        /// <exception cref="SecurityTokenException">Thrown if the token is invalid.</exception>
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)),
                ValidateLifetime = false // Allow expired tokens for this operation
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

            // Ensure the token is a valid JWT and uses the expected algorithm
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}
