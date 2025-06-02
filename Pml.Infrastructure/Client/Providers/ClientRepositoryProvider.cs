using Microsoft.AspNetCore.Http;
using Pml.Domain.IRepositories.Client;
using Pml.Domain.IRepositories.Master;
using Pml.Infrastructure.Client.Factory;

namespace Pml.Infrastructure.Client
{
    public class ClientRepositoryProvider : IClientRepositoryProvider
    {
        private readonly IClientRepositoryFactory _clientRepositoryFactory;
        private readonly ICompanyRepository _companyRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Dictionary<int, IClientRepository> _cachedRepositories;

        public ClientRepositoryProvider(
            IClientRepositoryFactory clientRepositoryFactory,
            ICompanyRepository companyRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _clientRepositoryFactory = clientRepositoryFactory;
            _companyRepository = companyRepository;
            _httpContextAccessor = httpContextAccessor;
            _cachedRepositories = new Dictionary<int, IClientRepository>();
        }

        public async Task<IClientRepository> GetRepositoryAsync()
        {
            // Get company ID from user claims
            var companyIdClaim = _httpContextAccessor.HttpContext.User?.FindFirst("CompanyId")?.Value;
            if (string.IsNullOrEmpty(companyIdClaim) || !int.TryParse(companyIdClaim, out int companyId))
            {
                throw new UnauthorizedAccessException("Company ID not found in user claims");
            }

            return await GetRepositoryForCompanyAsync(companyId);
        }

        public async Task<IClientRepository> GetRepositoryForCompanyAsync(int companyId)
        {
            // Check if repository is already cached for this company
            if (_cachedRepositories.ContainsKey(companyId))
                return _cachedRepositories[companyId];

            // Get company database configuration
            var companyDatabase = await _companyRepository.GetDefaultDatabaseAsync(companyId);
            if (companyDatabase == null)
            {
                throw new InvalidOperationException($"No database configuration found for company {companyId}");
            }

            // Create and cache the repository
            var repository = await _clientRepositoryFactory.CreateClientRepositoryAsync(companyDatabase);
            _cachedRepositories[companyId] = repository;

            return repository;
        }
    }
}
