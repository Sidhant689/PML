using MasterTenantAuthApi.Data.Client.Providers;
using Microsoft.AspNetCore.Http;
using Pml.Domain.Entities.Models.Master;
using Pml.Domain.IRepositories.Client;
using Pml.Domain.IRepositories.Master;
using Pml.Infrastructure.Client.Providers;

namespace Pml.Infrastructure.Client
{
    public class DatabaseFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICompanyRepository _companyRepository;

        public DatabaseFactory(IHttpContextAccessor httpContextAccessor, ICompanyRepository companyRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _companyRepository = companyRepository;
        }

        public async Task<IClientRepository> CreateClientRepositoryAsync()
        {
            // Use try catch properly
            // get the company id  from the jwt token claims

            var companyIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("companyId");
            if (companyIdClaim == null || !int.TryParse(companyIdClaim.Value, out int companyId))
            {
                throw new UnauthorizedAccessException("Invalid company information");
            }

            // get comapany databases from the master DB
            var companyDatabases = await _companyRepository.GetCompanyDatabasesAsync(companyId);
            var defaultDatabase = await _companyRepository.GetDefaultDatabaseAsync(companyId);

            if (defaultDatabase == null)
            {
                throw new InvalidOperationException("No default database configured for company");
            }

            // Create appropriate client repository based on the company database type

            return defaultDatabase.Type switch
            {
                DatabaseType.SqlServer => new SqlServerRepository(defaultDatabase.ConnectionString),
                DatabaseType.Oracle => new OracleRepository(defaultDatabase.ConnectionString),
                DatabaseType.MsAccess => new AccessRepository(defaultDatabase.ConnectionString),
                DatabaseType.FoxPro => new FoxProRepository(defaultDatabase.ConnectionString),
                _ => throw new NotSupportedException($"Database type {defaultDatabase.Type} is not supported")
            };
        }
    }
}
