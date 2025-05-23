using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace PMLERP.Helpers
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        private ClaimsPrincipal _authenticated;
        private string _token;

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(_authenticated ?? _anonymous));
        }

        public void SetAuthenticated(string username, string token)
        {
            _token = token;

            // Decode JWT token to get claims
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
            _authenticated = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void SetLoggedOut()
        {
            _token = null;
            _authenticated = null;

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public bool IsAuthenticated()
        {
            return _authenticated != null;
        }

        public string GetToken()
        {
            return _token;
        }
    }
}