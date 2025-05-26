using System.Data.OleDb;
using Dapper;
using Pml.Domain.IRepositories.Client;

namespace Pml.Infrastructure.Client
{
    // Repository implementation for Microsoft Access using OLEDB and Dapper
    public class AccessRepository : IClientRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The OLEDB connection string for the Access database.</param>
        public AccessRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Executes a SQL query asynchronously and returns the result as a collection of dynamic objects.
        /// </summary>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="parameters">The parameters for the query (optional).</param>
        /// <returns>A task representing the asynchronous operation, with a result of the query result set.</returns>
        public async Task<IEnumerable<dynamic>> ExecuteQueryAsync(string query, object parameters = null)
        {
            using var connection = new OleDbConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.QueryAsync(query, parameters);
        }

        /// <summary>
        /// Executes a SQL command asynchronously and returns the number of affected rows.
        /// </summary>
        /// <param name="command">The SQL command to execute.</param>
        /// <param name="parameters">The parameters for the command (optional).</param>
        /// <returns>A task representing the asynchronous operation, with a result of the number of affected rows.</returns>
        public async Task<int> ExecuteCommandAsync(string command, object parameters = null)
        {
            using var connection = new OleDbConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.ExecuteAsync(command, parameters);
        }

        /// <summary>
        /// Retrieves a single record by its ID from the specified table asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the entity to return.</typeparam>
        /// <param name="tableName">The name of the table to query.</param>
        /// <param name="id">The ID value to search for.</param>
        /// <returns>A task representing the asynchronous operation, with a result of the entity if found; otherwise, null.</returns>
        public async Task<T> GetByIdAsync<T>(string tableName, object id) where T : class
        {
            using var connection = new OleDbConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.QueryFirstOrDefaultAsync<T>($"SELECT * FROM {tableName} WHERE Id = @Id", new { Id = id });
        }

        /// <summary>
        /// Retrieves all records from the specified table asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the entities to return.</typeparam>
        /// <param name="tableName">The name of the table to query.</param>
        /// <returns>A task representing the asynchronous operation, with a result of the collection of entities.</returns>
        public async Task<IEnumerable<T>> GetAllAsync<T>(string tableName) where T : class
        {
            using var connection = new OleDbConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.QueryAsync<T>($"SELECT * FROM {tableName}");
        }
    }
}
