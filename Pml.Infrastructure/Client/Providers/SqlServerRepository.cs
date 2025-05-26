using Dapper;
using Microsoft.Data.SqlClient;
using Pml.Domain.IRepositories.Client;

namespace MasterTenantAuthApi.Data.Client.Providers
{
    /// <summary>
    /// Provides SQL Server data access using Dapper for client-related operations.
    /// </summary>
    public class SqlServerRepository : IClientRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The SQL Server connection string.</param>
        public SqlServerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Executes a SQL query and returns the result as a collection of dynamic objects.
        /// </summary>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="parameters">The parameters for the query.</param>
        /// <returns>A collection of dynamic objects representing the query result.</returns>
        public async Task<IEnumerable<dynamic>> ExecuteQueryAsync(string query, object parameters = null)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.QueryAsync(query, parameters);
        }

        /// <summary>
        /// Executes a SQL command and returns the number of affected rows.
        /// </summary>
        /// <param name="command">The SQL command to execute.</param>
        /// <param name="parameters">The parameters for the command.</param>
        /// <returns>The number of rows affected.</returns>
        public async Task<int> ExecuteCommandAsync(string command, object parameters = null)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.ExecuteAsync(command, parameters);
        }

        /// <summary>
        /// Retrieves a single entity by its ID from the specified table.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>The entity if found; otherwise, null.</returns>
        public async Task<T> GetByIdAsync<T>(string tableName, object id) where T : class
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.QueryFirstOrDefaultAsync<T>($"SELECT * FROM {tableName} WHERE Id = @Id", new { Id = id });
        }

        /// <summary>
        /// Retrieves all entities from the specified table.
        /// </summary>
        /// <typeparam name="T">The type of the entities.</typeparam>
        /// <param name="tableName">The name of the table.</param>
        /// <returns>A collection of entities.</returns>
        public async Task<IEnumerable<T>> GetAllAsync<T>(string tableName) where T : class
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.QueryAsync<T>($"SELECT * FROM {tableName}");
        }
    }
}
