using MasterTenantAuthApi.Data.Client.Providers;
using Microsoft.AspNetCore.Http;
using Pml.Shared.Entities.Models.Master;
using Pml.Domain.IRepositories.Client;
using Pml.Domain.IRepositories.Master;
using Pml.Infrastructure.Client.Providers;

namespace Pml.Infrastructure.Client
{
    /// <summary>
    /// Factory class for creating client repository instances based on the company's default database.
    /// </summary>
    public class DatabaseFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICompanyRepository _companyRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseFactory"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">HTTP context accessor for accessing user claims.</param>
        /// <param name="companyRepository">Repository for company and database information.</param>
        public DatabaseFactory(IHttpContextAccessor httpContextAccessor, ICompanyRepository companyRepository)
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
        public async Task<IClientRepository> CreateClientRepositoryAsync()
        {
            // Try to get the company ID from the JWT token claims
            var companyIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("companyId");
            if (companyIdClaim == null || !int.TryParse(companyIdClaim.Value, out int companyId))
            {
                throw new UnauthorizedAccessException("Invalid company information");
            }

            // Retrieve all company databases (not used directly here, but could be useful for future logic)
            var companyDatabases = await _companyRepository.GetCompanyDatabasesAsync(companyId);

            // Retrieve the default database for the company
            var defaultDatabase = await _companyRepository.GetDefaultDatabaseAsync(companyId);
            if (defaultDatabase == null)
            {
                throw new InvalidOperationException("No default database configured for company");
            }

            // Create and return the appropriate client repository based on the database type
            return defaultDatabase.Type switch
            {
                DatabaseType.SqlServer => new SqlServerRepository(defaultDatabase.ConnectionString),
                DatabaseType.Oracle => new OracleRepository(defaultDatabase.ConnectionString),
                DatabaseType.MsAccess => new AccessRepository(defaultDatabase.ConnectionString),
                DatabaseType.FoxPro => new FoxProRepository(defaultDatabase.ConnectionString),
                DatabaseType.SQLite => new SQLiteRepository(defaultDatabase.ConnectionString),
                DatabaseType.PostgreSQL => new PostgreSqlRepository(defaultDatabase.ConnectionString),
                _ => throw new NotSupportedException($"Database type {defaultDatabase.Type} is not supported")
            };
        }
    }
}
