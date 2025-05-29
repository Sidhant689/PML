using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Pml.Domain.IRepositories.Master;

namespace Pml.Infrastructure.Master.Repositories
{
    /// <summary>
    /// Repository for managing system admin roles.
    /// </summary>
    public class SystemAdminRoleRepository : ISystemAdminRoleRepository
    {
        private readonly MasterDbContext _dbContext;
        private readonly SqlConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemAdminRoleRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The master database context.</param>
        /// <param name="connection">The Npgsql database connection.</param>
        public SystemAdminRoleRepository(MasterDbContext dbContext, SqlConnection connection)
        {
            _dbContext = dbContext;
            _connection = connection;
        }

        /// <summary>
        /// Retrieves the list of role names assigned to a specific admin user by their ID.
        /// </summary>
        /// <param name="adminId">The ID of the admin user.</param>
        /// <returns>A collection of role names.</returns>
        public async Task<IEnumerable<string>> GetRolesByAdminIdAsync(int adminId)
        {
            try
            {
                // Create a new connection using the provided connection string
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    // Open the connection asynchronously
                    await connection.OpenAsync();

                    // Define the SQL query to fetch role names for the given admin user
                    var query = "SELECT rolename FROM systemadminrole r " +
                                "JOIN systemadminuserrole ur ON ur.systemadminroleid = r.id " +
                                "WHERE ur.systemadminuserid = @AdminId";

                    // Execute the query using Dapper and return the result as a list
                    var roles = await connection.QueryAsync<string>(query, new { AdminId = adminId });

                    // Close the connection
                    connection.Close();

                    return roles.ToList();
                }
            }
            catch (Exception ex)
            {
                // Wrap and rethrow any exceptions with additional context
                throw new Exception($"Error Fetching Roles by AdminId : {ex.Message}", ex);
            }
        }
    }
}
