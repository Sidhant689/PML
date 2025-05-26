using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pml.Application.IServices;
using Pml.Domain.IRepositories.Master;
using Pml.Shared.Entities.Models.Master;

namespace Pml.Application.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repository;

        public CompanyService(ICompanyRepository repository)
        {
            _repository = repository;
        }

        public async Task<Company> GetByIdAsync(int id)
        {
            try
            {
                var company = await _repository.GetByIdAsync(id);
                if (company == null)
                {
                    throw new Exception("Company not found.");
                }
                return company;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Company> GetByCodeAsync(string code)
        {
            try
            {
                var company = await _repository.GetByCodeAsync(code);
                if (company == null)
                {
                    throw new Exception("Company not found.");
                }
                return company;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<CompanyDatabase>> GetCompanyDatabasesAsync(int companyId)
        {
            try
            {
                return await _repository.GetCompanyDatabasesAsync(companyId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CompanyDatabase> GetDefaultDatabaseAsync(int companyId)
        {
            try
            {
                return await _repository.GetDefaultDatabaseAsync(companyId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Company> CreateCompanyAsync(Company company)
        {
            try
            {
                return await _repository.CreateCompanyAsync(company);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Company> UpdateCompanyAsync(Company company)
        {
            try
            {
                return await _repository.UpdateCompanyAsync(company);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteCompanyAsync(int id)
        {
            try
            {
                return await _repository.DeleteCompanyAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CompanyDatabase> AddDatabaseAsync(CompanyDatabase database)
        {
            try
            {
                return await _repository.AddDatabaseAsync(database);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CompanyDatabase> UpdateDatabaseAsync(CompanyDatabase database)
        {
            try
            {
                return await _repository.UpdateDatabaseAsync(database);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteDatabaseAsync(int id)
        {
            try
            {
                return await _repository.DeleteDatabaseAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
