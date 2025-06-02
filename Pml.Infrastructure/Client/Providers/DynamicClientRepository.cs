using Pml.Domain.IRepositories.Client;

namespace Pml.Infrastructure.Client
{
    public class DynamicClientRepository : IClientRepository
    {
        private readonly IClientRepositoryProvider _repositoryProvider;

        public DynamicClientRepository(IClientRepositoryProvider repositoryProvider)
        {
            _repositoryProvider = repositoryProvider;
        }

        public async Task<IEnumerable<dynamic>> ExecuteQueryAsync(string query, object parameters = null)
        {
            var repository = await _repositoryProvider.GetRepositoryAsync();
            return await repository.ExecuteQueryAsync(query, parameters);
        }

        public async Task<int> ExecuteCommandAsync(string command, object parameters = null)
        {
            var repository = await _repositoryProvider.GetRepositoryAsync();
            return await repository.ExecuteCommandAsync(command, parameters);
        }

        public async Task<T> GetByIdAsync<T>(string tableName, object id) where T : class
        {
            var repository = await _repositoryProvider.GetRepositoryAsync();
            return await repository.GetByIdAsync<T>(tableName, id);
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(string tableName) where T : class
        {
            var repository = await _repositoryProvider.GetRepositoryAsync();
            return await repository.GetAllAsync<T>(tableName);
        }
    }
}
