using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pml.Domain.IRepositories.Master
{
    public interface ISystemAdminRoleRepository
    {
        Task<IEnumerable<string>> GetRolesByAdminIdAsync(int adminId);
    }
}
