using Pml.Domain.IRepositories.Client;
using Pml.Shared.Entities.Models.Master;

namespace Pml.Infrastructure.Client.Factory
{
    /// <summary>
    /// Interface for a factory that creates client repository instances based on the company's default database.
    /// </summary>
    public interface IClientRepositoryFactory
    {
        /// <summary>
        /// Creates an appropriate <see cref="IClientRepository"/> instance for the current user's company.
        /// </summary>
        /// <returns>
        /// An implementation of <see cref="IClientRepository"/> based on the company's default database type.
        /// </returns>
        Task<IClientRepository> CreateClientRepositoryAsync(CompanyDatabase database);
    }
}
