using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Pml.Shared.DTOs.Client;
using PMLERP.Helpers;
using PMLERP.IServices.Master;

namespace PMLERP.Services.Master
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        //private readonly ISecureStorage _secureStorage;
        private readonly AuthenticationStateProvider _authStateProvider;
        private const string TokenKey = "authenticationToken";
        private const string ExpiryKey = "tokenExpiryTime";

        public AuthService(HttpClient httpClient, 
            AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            //_secureStorage = secureStorage;
            _authStateProvider = authStateProvider;
        }

        public async Task<bool> LoginAsync(string username, string password, string Passcode, int CompanyId, int LanguageId)
        {
            try
            {
                var loginModel = new AuthRequest { UserName = username, Password = password, Passcode = Passcode, CompanyId = CompanyId, LanguageId = LanguageId};
                var response = await _httpClient.PostAsJsonAsync("Auth/login", loginModel);

                if (!response.IsSuccessStatusCode)
                    return false;

                var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

                if (result == null || string.IsNullOrEmpty(result.AccessToken))
                    return false;

                // Store token and expiry in secure storage
                await SecureStorage.Default.SetAsync(TokenKey, result.AccessToken);
                await SecureStorage.Default.SetAsync(ExpiryKey, result.RefreshTokenExpiry.ToString("o"));

                // Update authentication state
                ((CustomAuthStateProvider)_authStateProvider).SetAuthenticated(result.Username, result.AccessToken);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                // Remove token from secure storage
                SecureStorage.Default.Remove(TokenKey);
                SecureStorage.Default.Remove(ExpiryKey);

                // Update authentication state
                ((CustomAuthStateProvider)_authStateProvider).SetLoggedOut();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logout error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> IsUserAuthenticatedAsync()
        {
            try
            {
                // Check if token exists
                var token = await GetTokenAsync();

                if (string.IsNullOrEmpty(token))
                    return false;

                // Check if token is expired
                var expiryStr = await SecureStorage.Default.GetAsync(ExpiryKey);

                if (string.IsNullOrEmpty(expiryStr) || !DateTime.TryParse(expiryStr, out var expiry))
                    return false;

                if (expiry <= DateTime.UtcNow)
                {
                    // Token expired, logout
                    await LogoutAsync();
                    return false;
                }

                // Validate token with server
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync("Auth/validate");

                if (!response.IsSuccessStatusCode)
                {
                    await LogoutAsync();
                    return false;
                }

                var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

                if (result == null)
                {
                    await LogoutAsync();
                    return false;
                }

                // THIS IS THE KEY FIX - Restore authentication state
                ((CustomAuthStateProvider)_authStateProvider).SetAuthenticated(result.Username, token);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Authentication check error: {ex.Message}");
                await LogoutAsync();
                return false;
            }
        }

        public async Task<string> GetTokenAsync()
        {
            try
            {
                return await SecureStorage.Default.GetAsync(TokenKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetTokenAsync error: {ex.Message}");
                return null;
            }
        }
    }
}