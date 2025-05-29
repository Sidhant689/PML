using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Pml.Domain.IRepositories.Master;
using Pml.Shared.Entities.Models.Master;

namespace Pml.Infrastructure.Master.Repositories
{
    /// <summary>
    /// Repository for managing SystemAdminUser entities in the database.
    /// </summary>
    public class SystemAdminRepository : ISystemAdminRepository
    {
        private readonly MasterDbContext _context;
        private readonly SqlConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemAdminRepository"/> class.
        /// </summary>
        /// <param name="context">The master database context.</param>
        /// <param name="connection">The PostgreSQL connection.</param>
        public SystemAdminRepository(MasterDbContext context, SqlConnection connection)
        {
            _context = context;
            _connection = connection;
        }

        /// <summary>
        /// Creates a new system admin user in the database.
        /// </summary>
        /// <param name="admin">The admin user to create.</param>
        /// <returns>The created <see cref="SystemAdminUser"/>.</returns>
        public async Task<SystemAdminUser> CreateAdminAsync(SystemAdminUser admin)
        {
            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
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

        /// <summary>
        /// Deletes a system admin user by ID.
        /// </summary>
        /// <param name="id">The ID of the admin user to delete.</param>
        /// <returns>True if the user was deleted; otherwise, false.</returns>
        public async Task<bool> DeleteAdminAsync(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
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

        /// <summary>
        /// Retrieves all system admin users from the database.
        /// </summary>
        /// <returns>An enumerable of <see cref="SystemAdminUser"/>.</returns>
        public async Task<IEnumerable<SystemAdminUser>> GetAllAdminsAsync()
        {
            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
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

        /// <summary>
        /// Retrieves a system admin user by ID.
        /// </summary>
        /// <param name="id">The ID of the admin user.</param>
        /// <returns>The <see cref="SystemAdminUser"/> if found; otherwise, null.</returns>
        public async Task<SystemAdminUser> GetByIdAsync(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
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

        /// <summary>
        /// Retrieves a system admin user by refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <returns>The <see cref="SystemAdminUser"/> if found; otherwise, null.</returns>
        public async Task<SystemAdminUser> GetByRefreshTokenAsync(string refreshToken)
        {
            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
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

        /// <summary>
        /// Retrieves a system admin user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>The <see cref="SystemAdminUser"/> if found; otherwise, null.</returns>
        public async Task<SystemAdminUser> GetByUsernameAsync(string username)
        {
            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    connection.Open();
                    var query = "SELECT * FROM dbo.systemadminuser WHERE Username = @Username";
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

        /// <summary>
        /// Updates an existing system admin user in the database.
        /// </summary>
        /// <param name="admin">The admin user with updated information.</param>
        /// <returns>The updated <see cref="SystemAdminUser"/>.</returns>
        public async Task<SystemAdminUser> UpdateAdminAsync(SystemAdminUser admin)
        {
            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
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
