using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using Pml.Domain.IRepositories.Master;

namespace Pml.Infrastructure.Master.Repositories
{
    public class SystemAdminRoleRepository : ISystemAdminRoleRepository
    {
        private readonly MasterDbContext _dbContext;
        private readonly NpgsqlConnection _connection;

        public SystemAdminRoleRepository(MasterDbContext dbContext, NpgsqlConnection connection)
        {
            _dbContext = dbContext;
            _connection = connection;
        }

        public async Task<IEnumerable<string>> GetRolesByAdminIdAsync(int adminId)
        {
            try
            {
                using(var connection = new NpgsqlConnection(_connection.ConnectionString))
                {
                    connection.OpenAsync();
                    var query = $"SELECT rolename FROM systemadminrole r " +
                                $"join systemadminuserrole ur on ur.systemadminroleid = r.id" +
                                $" WHERE ur.systemadminuserid = {adminId}";
                    var roles = await connection.QueryAsync<string>(query);
                    connection.Close();
                    return roles.ToList();
                }
            }
            catch (Exception ex )
            {
                throw new Exception($"Error Fetching Roles by AdminId : {ex.Message}",ex);
            }
        }
    }
}
