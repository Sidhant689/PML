using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pml.Shared.ToastMessage;

namespace PMLERP.IServices.Master
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string userName, string Password, string Passcode, int CompanyId, int LanguageId);
        Task<bool> LogoutAsync();
        Task<bool> IsUserAuthenticatedAsync();
        Task<string> GetTokenAsync();

    }
}
