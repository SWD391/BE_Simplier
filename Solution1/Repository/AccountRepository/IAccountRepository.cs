using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObjects.Enums.Status;

namespace Repository.AccountRepository
{ 
    public interface IAccountRepository : IRepository<Account>
    {
        public Task<Account?> GetByEmailAsync(string email);
        public Task<List<Account>> GetPaginationAsync(AccountRole role, int pageNumber, int pageSize);
        public Task<List<Account>> GetPaginationWithSearchKeyAsync(AccountRole role, string searchKey, int pageNumber, int pageSize);
        public Task<int> Count(AccountRole role);
    }
}
