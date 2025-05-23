using System.Data.OleDb;
using Dapper;
using Pml.Domain.IRepositories.Client;

namespace Pml.Infrastructure.Client
{
    // AccessRepository.cs (Using OLEDB)
    public class AccessRepository: IClientRepository
    {
        private readonly string _connectionString;

        public AccessRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<dynamic>> ExecuteQueryAsync(string query, object parameters = null)
        {
            using var connection = new OleDbConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.QueryAsync(query, parameters);
        }

        public async Task<int> ExecuteCommandAsync(string command, object parameters = null)
        {
            using var connection = new OleDbConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.ExecuteAsync(command, parameters);
        }

        public async Task<T> GetByIdAsync<T>(string tableName, object id) where T : class
        {
            using var connection = new OleDbConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.QueryFirstOrDefaultAsync<T>($"SELECT * FROM {tableName} WHERE Id = @Id", new { Id = id });
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(string tableName) where T : class
        {
            using var connection = new OleDbConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.QueryAsync<T>($"SELECT * FROM {tableName}");
        }
    }
}
