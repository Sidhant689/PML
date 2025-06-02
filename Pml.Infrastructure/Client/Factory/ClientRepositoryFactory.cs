using MasterTenantAuthApi.Data.Client.Providers;
using Microsoft.AspNetCore.Http;
using Pml.Shared.Entities.Models.Master;
using Pml.Domain.IRepositories.Client;
using Pml.Domain.IRepositories.Master;
using Pml.Infrastructure.Client.Providers;

namespace Pml.Infrastructure.Client.Factory
{
    /// <summary>
    /// Factory class for creating client repository instances based on the company's default database.
    /// </summary>
    public class ClientRepositoryFactory : IClientRepositoryFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICompanyRepository _companyRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientRepositoryFactory"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">HTTP context accessor for accessing user claims.</param>
        /// <param name="companyRepository">Repository for company and database information.</param>
        public ClientRepositoryFactory(IHttpContextAccessor httpContextAccessor, ICompanyRepository companyRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _companyRepository = companyRepository;
        }

        /// <summary>
        /// Creates an appropriate <see cref="IClientRepository"/> instance for the current user's company.
        /// </summary>
        /// <returns>An implementation of <see cref="IClientRepository"/> based on the company's default database type.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown if company information is missing or invalid.</exception>
        /// <exception cref="InvalidOperationException">Thrown if no default database is configured for the company.</exception>
        /// <exception cref="NotSupportedException">Thrown if the database type is not supported.</exception>
        public async Task<IClientRepository> CreateClientRepositoryAsync(CompanyDatabase database)
        {
            if (database == null)
                throw new InvalidOperationException("Default database not configured");

            return database.Type switch
            {
                DatabaseType.SqlServer => new SqlServerRepository(database.ConnectionString),
                DatabaseType.Oracle => new OracleRepository(database.ConnectionString),
                DatabaseType.MsAccess => new AccessRepository(database.ConnectionString),
                DatabaseType.FoxPro => new FoxProRepository(database.ConnectionString),
                DatabaseType.SQLite => new SQLiteRepository(database.ConnectionString),
                DatabaseType.PostgreSQL => new PostgreSqlRepository(database.ConnectionString),
                _ => throw new NotSupportedException("Unsupported database type")
            };
        }
    }
}
