using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pml.Shared.DTOs.Client
{
    /// <summary>
    /// Authentication request model.
    /// </summary>
    public class AuthRequest
    {
        /// <summary>
        /// Username for authentication.
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Password for authentication.
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Optional company ID for authentication in specific company.
        /// If not provided, system will search across all companies.
        /// </summary>
        public int? CompanyId { get; set; }

        /// <summary>
        /// Optional language ID for localization.
        /// </summary>
        public int? LanguageId { get; set; }

        /// <summary>
        /// Optional passcode for additional security.
        /// </summary>
        public string? Passcode { get; set; }
    }

    /// <summary>
    /// Authentication response model.
    /// </summary>
    public class AuthResponse
    {
        /// <summary>
        /// Indicates if authentication was successful.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Response message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// JWT access token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Refresh token for getting new access tokens.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Refresh token expiry date.
        /// </summary>
        public DateTime RefreshTokenExpiry { get; set; }

        /// <summary>
        /// Authenticated user's ID.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Authenticated user's username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Authenticated user's email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User's roles.
        /// </summary>
        public IEnumerable<string> Roles { get; set; }
    }

    /// <summary>
    /// Refresh token request model.
    /// </summary>
    public class RefreshTokenRequest
    {
        /// <summary>
        /// Expired access token.
        /// </summary>
        [Required]
        public string AccessToken { get; set; }

        /// <summary>
        /// Refresh token.
        /// </summary>
        [Required]
        public string RefreshToken { get; set; }
    }
}
