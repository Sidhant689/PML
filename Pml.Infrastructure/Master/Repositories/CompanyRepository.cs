using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Pml.Domain.IRepositories.Master;
using Pml.Shared.Entities.Models.Master;

namespace Pml.Infrastructure.Master.Repositories
{
    /// <summary>
    /// Repository for managing Company and CompanyDatabase entities using Dapper and PostgreSQL.
    /// </summary>
    public class CompanyRepository : ICompanyRepository
    {
        private readonly MasterDbContext _context;
        private readonly SqliteConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyRepository"/> class.
        /// </summary>
        /// <param name="context">The master database context.</param>
        /// <param name="connection">The PostgreSQL connection.</param>
        public CompanyRepository(MasterDbContext context, SqliteConnection connection)
        {
            _context = context;
            _connection = connection;
        }

        /// <summary>
        /// Gets a companies by its unique identifier.
        /// </summary>
        /// <param name="id">The companies ID.</param>
        /// <returns>The companies entity or null if not found.</returns>
        public async Task<Company> GetByIdAsync(int id)
        {
            try
            {
                using (var connection = new SqliteConnection(_connection.ConnectionString))
                {
                    connection.Open();
                    var query = "SELECT * FROM companies WHERE Id = @Id";
                    var data = await connection.QueryFirstOrDefaultAsync<Company>(query, new { Id = id });
                    connection.Close();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching companies by ID: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets a companies by its unique code.
        /// </summary>
        /// <param name="code">The companies code.</param>
        /// <returns>The companies entity or null if not found.</returns>
        public async Task<Company> GetByCodeAsync(string code)
        {
            try
            {
                using (var connection = new SqliteConnection(_connection.ConnectionString))
                {
                    connection.Open();
                    var query = "SELECT * FROM companies WHERE Code = @Code";
                    var data = await connection.QueryFirstOrDefaultAsync<Company>(query, new { Code = code });
                    connection.Close();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching companies by code: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all companies.
        /// </summary>
        /// <returns>A collection of all companies.</returns>
        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            try
            {
                using (var connection = new SqliteConnection(_connection.ConnectionString))
                {
                    connection.Open();
                    var query = "SELECT * FROM Companies";
                    var data = await connection.QueryAsync<Company>(query);
                    connection.Close();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching all companies: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all databases associated with a companies.
        /// </summary>
        /// <param name="companyId">The companies ID.</param>
        /// <returns>A collection of companies databases.</returns>
        public async Task<IEnumerable<CompanyDatabase>> GetCompanyDatabasesAsync(int companyId)
        {
            try
            {
                using (var connection = new SqliteConnection(_connection.ConnectionString))
                {
                    connection.Open();
                    var query = "SELECT * FROM companydatabase WHERE CompanyId = @CompanyId";
                    var data = await connection.QueryAsync<CompanyDatabase>(query, new { CompanyId = companyId });
                    connection.Close();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching databases for companies ID {companyId}: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<Company>> GetAllActiveCompaniesAsync()
        {
            try
            {
                using (var connection = new SqliteConnection(_connection.ConnectionString))
                {
                    connection.Open();
                    var query = "SELECT * FROM companies WHERE IsActive = true";
                    var data = await connection.QueryAsync<Company>(query);
                    connection.Close();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching all active companies: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets the default database for a companies.
        /// </summary>
        /// <param name="companyId">The companies ID.</param>
        /// <returns>The default companies database or null if not found.</returns>
        public async Task<CompanyDatabase> GetDefaultDatabaseAsync(int companyId)
        {
            try
            {
                using (var connection = new SqliteConnection(_connection.ConnectionString))
                {
                    connection.Open();
                    var query = "SELECT * FROM companydatabases WHERE CompanyId = @CompanyId AND IsDefault = true";
                    var data = await connection.QueryFirstOrDefaultAsync<CompanyDatabase>(query, new { CompanyId = companyId });
                    connection.Close();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching default database for companies ID {companyId}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Creates a new companies.
        /// </summary>
        /// <param name="companies">The companies entity to create.</param>
        /// <returns>The created companies entity.</returns>
        public async Task<Company> CreateCompanyAsync(Company companies)
        {
            try
            {
                using (var connection = new SqliteConnection(_connection.ConnectionString))
                {
                    connection.Open();
                    var query = "INSERT INTO companies (Name, Address, Phone, Email, Website, Logo, TaxId, RegistrationNumber) " +
                                "VALUES (@Name, @Address, @Phone, @Email, @Website, @Logo, @TaxId, @RegistrationNumber) " +
                                "RETURNING *";
                    var data = await connection.QueryFirstOrDefaultAsync<Company>(query, companies);
                    connection.Close();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating companies: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing companies.
        /// </summary>
        /// <param name="companies">The companies entity to update.</param>
        /// <returns>The updated companies entity.</returns>
        public async Task<Company> UpdateCompanyAsync(Company companies)
        {
            try
            {
                using (var connection = new SqliteConnection(_connection.ConnectionString))
                {
                    connection.Open();
                    var query = "UPDATE companies SET Name = @Name, Address = @Address, Phone = @Phone, Email = @Email, " +
                                "Website = @Website, Logo = @Logo, TaxId = @TaxId, RegistrationNumber = @RegistrationNumber " +
                                "WHERE Id = @Id RETURNING *";
                    var data = await connection.QueryFirstOrDefaultAsync<Company>(query, companies);
                    connection.Close();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating companies: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deletes a companies by its ID.
        /// </summary>
        /// <param name="id">The companies ID.</param>
        /// <returns>True if the companies was deleted; otherwise, false.</returns>
        public async Task<bool> DeleteCompanyAsync(int id)
        {
            try
            {
                using (var connection = new SqliteConnection(_connection.ConnectionString))
                {
                    connection.Open();
                    var query = "DELETE FROM companies WHERE Id = @Id";
                    var result = await connection.ExecuteAsync(query, new { Id = id });
                    connection.Close();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting companies: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Adds a new database for a companies.
        /// </summary>
        /// <param name="database">The companies database entity to add.</param>
        /// <returns>The created companies database entity.</returns>
        public async Task<CompanyDatabase> AddDatabaseAsync(CompanyDatabase database)
        {
            try
            {
                using (var connection = new SqliteConnection(_connection.ConnectionString))
                {
                    connection.Open();
                    var query = "INSERT INTO companydatabase (CompanyId, Name, Type, ConnectionString, IsDefault) " +
                                "VALUES (@CompanyId, @Name, @Type, @ConnectionString, @IsDefault) " +
                                "RETURNING *";
                    var data = await connection.QueryFirstOrDefaultAsync<CompanyDatabase>(query, database);
                    connection.Close();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding database: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing companies database.
        /// </summary>
        /// <param name="database">The companies database entity to update.</param>
        /// <returns>The updated companies database entity.</returns>
        public async Task<CompanyDatabase> UpdateDatabaseAsync(CompanyDatabase database)
        {
            try
            {
                using (var connection = new SqliteConnection(_connection.ConnectionString))
                {
                    connection.Open();
                    var query = "UPDATE companydatabase SET Name = @Name, Type = @Type, ConnectionString = @ConnectionString, " +
                                "IsDefault = @IsDefault WHERE Id = @Id RETURNING *";
                    var data = await connection.QueryFirstOrDefaultAsync<CompanyDatabase>(query, database);
                    connection.Close();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating database: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deletes a companies database by its ID.
        /// </summary>
        /// <param name="id">The database ID.</param>
        /// <returns>True if the database was deleted; otherwise, false.</returns>
        public async Task<bool> DeleteDatabaseAsync(int id)
        {
            try
            {
                using (var connection = new SqliteConnection(_connection.ConnectionString))
                {
                    connection.Open();
                    var query = "DELETE FROM companydatabase WHERE Id = @Id";
                    var result = await connection.ExecuteAsync(query, new { Id = id });
                    connection.Close();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting database: {ex.Message}", ex);
            }
        }
    }
}
