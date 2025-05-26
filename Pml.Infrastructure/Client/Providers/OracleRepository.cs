using Dapper;
using Oracle.ManagedDataAccess.Client;
using Pml.Domain.IRepositories.Client;

namespace Pml.Infrastructure.Client.Providers
{
    /// <summary>
    /// Provides methods for interacting with an Oracle database using Dapper.
    /// Implements the IClientRepository interface.
    /// </summary>
    public class OracleRepository : IClientRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="OracleRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The Oracle database connection string.</param>
        public OracleRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Executes a query asynchronously and returns the result as a collection of dynamic objects.
        /// </summary>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="parameters">The parameters for the query (optional).</param>
        /// <returns>A task representing the asynchronous operation, with a result of the query result set.</returns>
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

        /// <summary>
        /// Executes a command asynchronously and returns the number of affected rows.
        /// </summary>
        /// <param name="command">The SQL command to execute.</param>
        /// <param name="parameters">The parameters for the command (optional).</param>
        /// <returns>A task representing the asynchronous operation, with a result of the number of affected rows.</returns>
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

        /// <summary>
        /// Retrieves a record by its Id from the specified table asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the entity to return.</typeparam>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="id">The Id of the record to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, with a result of the entity found or null.</returns>
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

        /// <summary>
        /// Retrieves all records from the specified table asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the entities to return.</typeparam>
        /// <param name="tableName">The name of the table.</param>
        /// <returns>A task representing the asynchronous operation, with a result of the collection of entities.</returns>
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
