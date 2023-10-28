using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObjects.Enums.Status;

namespace Repository.FixTaskRepository
{
    public interface IFixTaskRepository : IRepository<FixTask>
    {
        public Task<FixTask?> GetFixTaskIncludeDetails(string fixTaskId);
        public Task<List<FixTask>> GetPaginationAsync(FixTaskStatus status, int pageNumber, int pageSize);
        public Task<List<FixTask>> GetPaginationWithSearchKeyAsync(FixTaskStatus status, string searchKey, int pageNumber, int pageSize);
        public Task<int> Count(FixTaskStatus status);
        public Task<List<FixTask>> GetPaginationBelongUserAsync(string accountId, int pageNumber, int pageSize);
        public Task<List<FixTask>> GetPaginationWithSearchKeyBelongUserAsync(string accountId, string searchKey, int pageNumber, int pageSize);
        public Task<int> CountBelongUser(string accountId);
    }
}
