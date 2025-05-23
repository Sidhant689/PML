using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pml.Domain.IRepositories.Client
{
    public interface IClientRepository
    {
        Task<IEnumerable<dynamic>> ExecuteQueryAsync(string query, object parameters = null);
        Task<int> ExecuteCommandAsync(string command, object parameters = null);
        Task<T> GetByIdAsync<T>(string tableName, object id) where T : class;
        Task<IEnumerable<T>> GetAllAsync<T>(string tableName) where T : class;
    }
}
