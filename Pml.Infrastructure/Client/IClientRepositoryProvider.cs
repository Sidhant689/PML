using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pml.Domain.IRepositories.Client;

namespace Pml.Infrastructure.Client
{
    public interface IClientRepositoryProvider
    {
        Task<IClientRepository> GetRepositoryAsync();
        Task<IClientRepository> GetRepositoryForCompanyAsync(int companyId);
    }
}
