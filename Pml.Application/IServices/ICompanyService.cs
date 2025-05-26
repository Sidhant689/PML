using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pml.Shared.Entities.Models.Master;

namespace Pml.Application.IServices
{
    public interface ICompanyService
    {
        Task<Company> GetByIdAsync(int id);
        Task<Company> GetByCodeAsync(string code);
        Task<IEnumerable<Company>> GetAllAsync();
        Task<IEnumerable<CompanyDatabase>> GetCompanyDatabasesAsync(int companyId);
        Task<CompanyDatabase> GetDefaultDatabaseAsync(int companyId);
        Task<Company> CreateCompanyAsync(Company company);
        Task<Company> UpdateCompanyAsync(Company company);
        Task<bool> DeleteCompanyAsync(int id);
        Task<CompanyDatabase> AddDatabaseAsync(CompanyDatabase database);
        Task<CompanyDatabase> UpdateDatabaseAsync(CompanyDatabase database);
        Task<bool> DeleteDatabaseAsync(int id);
    }
}
