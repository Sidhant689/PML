using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pml.Shared.DTOs.Master.Authentication;
using Pml.Shared.ToastMessage;

namespace PMLERP.IServices.Master
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string userName, string Password);
        Task<bool> LogoutAsync();
        Task<bool> IsUserAuthenticatedAsync();
        Task<string> GetTokenAsync();

    }
}
