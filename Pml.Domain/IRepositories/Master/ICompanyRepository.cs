using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pml.Domain.Entities.Models;

namespace Pml.Domain.IRepositories.Master
{
    public interface ICompanyRepository
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
