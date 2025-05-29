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
    /// Repository for managing Company and CompanyDatabase entities using Dapper and PostgreSQL.
    /// </summary>
    public class CompanyRepository : ICompanyRepository
    {
        private readonly MasterDbContext _context;
        private readonly SqlConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyRepository"/> class.
        /// </summary>
        /// <param name="context">The master database context.</param>
        /// <param name="connection">The PostgreSQL connection.</param>
        public CompanyRepository(MasterDbContext context, SqlConnection connection)
        {
            _context = context;
            _connection = connection;
        }

        /// <summary>
        /// Gets a company by its unique identifier.
        /// </summary>
        /// <param name="id">The company ID.</param>
        /// <returns>The company entity or null if not found.</returns>
        public async Task<Company> GetByIdAsync(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    connection.Open();
                    var query = "SELECT * FROM company WHERE Id = @Id";
                    var data = await connection.QueryFirstOrDefaultAsync<Company>(query, new { Id = id });
                    connection.Close();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching company by ID: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets a company by its unique code.
        /// </summary>
        /// <param name="code">The company code.</param>
        /// <returns>The company entity or null if not found.</returns>
        public async Task<Company> GetByCodeAsync(string code)
        {
            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    connection.Open();
                    var query = "SELECT * FROM company WHERE Code = @Code";
                    var data = await connection.QueryFirstOrDefaultAsync<Company>(query, new { Code = code });
                    connection.Close();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching company by code: {ex.Message}", ex);
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
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    connection.Open();
                    var query = "SELECT * FROM company";
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
        /// Gets all databases associated with a company.
        /// </summary>
        /// <param name="companyId">The company ID.</param>
        /// <returns>A collection of company databases.</returns>
        public async Task<IEnumerable<CompanyDatabase>> GetCompanyDatabasesAsync(int companyId)
        {
            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
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
                throw new Exception($"Error fetching databases for company ID {companyId}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets the default database for a company.
        /// </summary>
        /// <param name="companyId">The company ID.</param>
        /// <returns>The default company database or null if not found.</returns>
        public async Task<CompanyDatabase> GetDefaultDatabaseAsync(int companyId)
        {
            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    connection.Open();
                    var query = "SELECT * FROM companydatabase WHERE CompanyId = @CompanyId AND IsDefault = true";
                    var data = await connection.QueryFirstOrDefaultAsync<CompanyDatabase>(query, new { CompanyId = companyId });
                    connection.Close();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching default database for company ID {companyId}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Creates a new company.
        /// </summary>
        /// <param name="company">The company entity to create.</param>
        /// <returns>The created company entity.</returns>
        public async Task<Company> CreateCompanyAsync(Company company)
        {
            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    connection.Open();
                    var query = "INSERT INTO company (Name, Address, Phone, Email, Website, Logo, TaxId, RegistrationNumber) " +
                                "VALUES (@Name, @Address, @Phone, @Email, @Website, @Logo, @TaxId, @RegistrationNumber) " +
                                "RETURNING *";
                    var data = await connection.QueryFirstOrDefaultAsync<Company>(query, company);
                    connection.Close();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating company: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing company.
        /// </summary>
        /// <param name="company">The company entity to update.</param>
        /// <returns>The updated company entity.</returns>
        public async Task<Company> UpdateCompanyAsync(Company company)
        {
            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    connection.Open();
                    var query = "UPDATE company SET Name = @Name, Address = @Address, Phone = @Phone, Email = @Email, " +
                                "Website = @Website, Logo = @Logo, TaxId = @TaxId, RegistrationNumber = @RegistrationNumber " +
                                "WHERE Id = @Id RETURNING *";
                    var data = await connection.QueryFirstOrDefaultAsync<Company>(query, company);
                    connection.Close();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating company: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deletes a company by its ID.
        /// </summary>
        /// <param name="id">The company ID.</param>
        /// <returns>True if the company was deleted; otherwise, false.</returns>
        public async Task<bool> DeleteCompanyAsync(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
                {
                    connection.Open();
                    var query = "DELETE FROM company WHERE Id = @Id";
                    var result = await connection.ExecuteAsync(query, new { Id = id });
                    connection.Close();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting company: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Adds a new database for a company.
        /// </summary>
        /// <param name="database">The company database entity to add.</param>
        /// <returns>The created company database entity.</returns>
        public async Task<CompanyDatabase> AddDatabaseAsync(CompanyDatabase database)
        {
            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
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
        /// Updates an existing company database.
        /// </summary>
        /// <param name="database">The company database entity to update.</param>
        /// <returns>The updated company database entity.</returns>
        public async Task<CompanyDatabase> UpdateDatabaseAsync(CompanyDatabase database)
        {
            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
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
        /// Deletes a company database by its ID.
        /// </summary>
        /// <param name="id">The database ID.</param>
        /// <returns>True if the database was deleted; otherwise, false.</returns>
        public async Task<bool> DeleteDatabaseAsync(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connection.ConnectionString))
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
