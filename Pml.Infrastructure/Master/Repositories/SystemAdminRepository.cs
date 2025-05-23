using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using Pml.Domain.Entities.Models.Master;
using Pml.Domain.IRepositories.Master;

namespace Pml.Infrastructure.Master.Repositories
{
    public class SystemAdminRepository : ISystemAdminRepository
    {
        private readonly MasterDbContext _context;
        private readonly NpgsqlConnection _connection;
        public SystemAdminRepository(MasterDbContext context, NpgsqlConnection connection)
        {
            _context = context;
            _connection = connection;
        }

        public async Task<SystemAdminUser> CreateAdminAsync(SystemAdminUser admin)
        {
            try
            {
                
                using (var connection = new NpgsqlConnection(_connection.ConnectionString))
                {
                    connection.Open();
                    var query = "INSERT INTO systemadminuser (Username, Email, PasswordHash, RefreshToken, RefreshTokenExpiry, IsSuperAdmin) " +
                                "VALUES (@Username, @Email, @PasswordHash, @RefreshToken, @RefreshTokenExpiry, @IsSuperAdmin) " +
                                "RETURNING *";
                    var data = await connection.QueryFirstOrDefaultAsync<SystemAdminUser>(query, admin);
                    connection.Close();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteAdminAsync(int id)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connection.ConnectionString))
                {
                    connection.Open();
                    var query = "DELETE FROM systemadminuser WHERE Id = @Id";
                    var result = await connection.ExecuteAsync(query, new { Id = id });
                    connection.Close();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting admin: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<SystemAdminUser>> GetAllAdminsAsync()
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connection.ConnectionString))
                {
                    connection.Open();
                    var query = "SELECT * FROM systemadminuser";
                    var data = await connection.QueryAsync<SystemAdminUser>(query);
                    connection.Close();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching all admins: {ex.Message}", ex);
            }
        }

        public async Task<SystemAdminUser> GetByIdAsync(int id)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connection.ConnectionString))
                {
                    connection.Open();
                    var query = "SELECT * FROM systemadminuser WHERE Id = @Id";
                    var data = await connection.QueryFirstOrDefaultAsync<SystemAdminUser>(query, new { Id = id });
                    connection.Close();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching admin by ID: {ex.Message}", ex);
            }
        }

        public async Task<SystemAdminUser> GetByRefreshTokenAsync(string refreshToken)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connection.ConnectionString))
                {
                    connection.Open();
                    var query = "SELECT * FROM systemadminuser WHERE RefreshToken = @RefreshToken";
                    var data = await connection.QueryFirstOrDefaultAsync<SystemAdminUser>(query, new { RefreshToken = refreshToken });
                    connection.Close();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching admin by refresh token: {ex.Message}", ex);
            }
        }

        public async Task<SystemAdminUser> GetByUsernameAsync(string username)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connection.ConnectionString))
                {
                    connection.Open();
                    var query = "SELECT * FROM systemadminuser WHERE Username = @Username";
                    var data = await connection.QueryFirstOrDefaultAsync<SystemAdminUser>(query, new { Username = username });
                    connection.Close();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching admin by username: {ex.Message}", ex);
            }
        }

        public async Task<SystemAdminUser> UpdateAdminAsync(SystemAdminUser admin)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connection.ConnectionString))
                {
                    connection.Open();
                    const string query = @"
                        UPDATE SystemAdminUser
                        SET Name = @Name, 
                            UserName = @UserName,
                            Password = @Password,
                            UserStatus = @UserStatus,
                            UserEmail = @UserEmail,
                            UserPhone = @UserPhone,
                            UserAddress = @UserAddress,
                            RefreshToken = @RefreshToken,
                            RefreshTokenExpiry = @RefreshTokenExpiry
                        WHERE Id = @Id";
                    var data = await connection.QueryFirstOrDefaultAsync<SystemAdminUser>(query, admin);
                    connection.Close();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating admin: {ex.Message}", ex);
            }
        }
    }
}
