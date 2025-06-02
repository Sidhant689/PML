using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Pml.Domain.Authentication
{
    public interface ITokenRepository
    {
        string GenerateAccessToken(int userId, string username, string email, IEnumerable<string> roles, int companyId);
        string GenerateRefreshToken();
        DateTime CalculateRefreshTokenExpiry();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
