using Dapper;
using Oracle.ManagedDataAccess.Client;
using Pml.Domain.IRepositories.Client;

namespace Pml.Infrastructure.Client.Providers
{
    public class OracleRepository : IClientRepository
    {
        private readonly string _connectionString;

        public OracleRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<dynamic>> ExecuteQueryAsync(string query, object parameters = null)
        {
            try
            {
                using var connection = new OracleConnection(_connectionString);
                await connection.OpenAsync();
                return await connection.QueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                // Log the exception (logging mechanism not implemented here)
                throw new Exception("An error occurred while executing the query.", ex);
            }
        }

        public async Task<int> ExecuteCommandAsync(string command, object parameters = null)
        {
            try
            {
                using var connection = new OracleConnection(_connectionString);
                await connection.OpenAsync();
                return await connection.ExecuteAsync(command, parameters);
            }
            catch (Exception ex)
            {
                // Log the exception (logging mechanism not implemented here)
                throw new Exception("An error occurred while executing the command.", ex);
            }
        }

        public async Task<T> GetByIdAsync<T>(string tableName, object id) where T : class
        {
            try
            {
                using var connection = new OracleConnection(_connectionString);
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<T>($"SELECT * FROM {tableName} WHERE Id = :Id", new { Id = id });
            }
            catch (Exception ex)
            {
                // Log the exception (logging mechanism not implemented here)
                throw new Exception($"An error occurred while retrieving the record with Id: {id} from table: {tableName}.", ex);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(string tableName) where T : class
        {
            try
            {
                using var connection = new OracleConnection(_connectionString);
                await connection.OpenAsync();
                return await connection.QueryAsync<T>($"SELECT * FROM {tableName}");
            }
            catch (Exception ex)
            {
                // Log the exception (logging mechanism not implemented here)
                throw new Exception($"An error occurred while retrieving all records from table: {tableName}.", ex);
            }
        }
    }
}
