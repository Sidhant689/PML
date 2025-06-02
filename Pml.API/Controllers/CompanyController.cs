using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pml.Application.IServices;
using Pml.Shared.Entities.Models.Master;

namespace Pml.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        /// <summary>
        /// Constructor for CompanyController.
        /// </summary>
        /// <param name="companyService">Injected company service.</param>
        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        /// <summary>
        /// Gets a company by its ID.
        /// </summary>
        /// <param name="id">Company ID.</param>
        /// <returns>Company object.</returns>
        [HttpGet("GetByIdAsync")]
        public async Task<Company> GetByIdAsync(int id)
        {
            try
            {
                var company = await _companyService.GetByIdAsync(id);
                if (company == null)
                {
                    throw new KeyNotFoundException($"Company with ID {id} not found.");
                }
                return company;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Gets a company by its code.
        /// </summary>
        /// <param name="code">Company code.</param>
        /// <returns>Company object.</returns>
        [HttpGet("GetByCodeAsync")]
        public async Task<Company> GetByCodeAsync(string code)
        {
            try
            {
                var company = await _companyService.GetByCodeAsync(code);
                if (company == null)
                {
                    throw new KeyNotFoundException($"Company with code {code} not found.");
                }
                return company;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Gets all companies.
        /// </summary>
        /// <returns>List of companies.</returns>
        [HttpGet("GetAllAsync")]
        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            try
            {
                return await _companyService.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Gets all databases for a company.
        /// </summary>
        /// <param name="companyId">Company ID.</param>
        /// <returns>List of company databases.</returns>
        [HttpGet("GetCompanyDatabasesAsync")]
        public async Task<IEnumerable<CompanyDatabase>> GetCompanyDatabasesAsync(int companyId)
        {
            try
            {
                return await _companyService.GetCompanyDatabasesAsync(companyId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Gets the default database for a company.
        /// </summary>
        /// <param name="companyId">Company ID.</param>
        /// <returns>Default company database.</returns>
        [HttpGet("GetDefaultDatabaseAsync")]
        public async Task<CompanyDatabase> GetDefaultDatabaseAsync(int companyId)
        {
            try
            {
                return await _companyService.GetDefaultDatabaseAsync(companyId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Creates a new company.
        /// </summary>
        /// <param name="company">Company object.</param>
        /// <returns>Created company.</returns>
        [HttpPost("CreateCompanyAsync")]
        public async Task<Company> CreateCompanyAsync([FromBody] Company company)
        {
            try
            {
                return await _companyService.CreateCompanyAsync(company);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing company.
        /// </summary>
        /// <param name="company">Company object.</param>
        /// <returns>Updated company.</returns>
        [HttpPut("UpdateCompanyAsync")]
        public async Task<Company> UpdateCompanyAsync([FromBody] Company company)
        {
            try
            {
                return await _companyService.UpdateCompanyAsync(company);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a company by its ID.
        /// </summary>
        /// <param name="id">Company ID.</param>
        /// <returns>True if deleted, otherwise false.</returns>
        [HttpDelete("DeleteCompanyAsync")]
        public async Task<bool> DeleteCompanyAsync(int id)
        {
            try
            {
                return await _companyService.DeleteCompanyAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Adds a new database to a company.
        /// </summary>
        /// <param name="database">CompanyDatabase object.</param>
        /// <returns>Created company database.</returns>
        [HttpPost("AddDatabaseAsync")]
        public async Task<CompanyDatabase> AddDatabaseAsync([FromBody] CompanyDatabase database)
        {
            try
            {
                return await _companyService.AddDatabaseAsync(database);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing company database.
        /// </summary>
        /// <param name="database">CompanyDatabase object.</param>
        /// <returns>Updated company database.</returns>
        [HttpPut("UpdateDatabaseAsync")]
        public async Task<CompanyDatabase> UpdateDatabaseAsync([FromBody] CompanyDatabase database)
        {
            try
            {
                return await _companyService.UpdateDatabaseAsync(database);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a company database by its ID.
        /// </summary>
        /// <param name="id">Database ID.</param>
        /// <returns>True if deleted, otherwise false.</returns>
        [HttpDelete("DeleteDatabaseAsync")]
        public async Task<bool> DeleteDatabaseAsync(int id)
        {
            try
            {
                return await _companyService.DeleteDatabaseAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
