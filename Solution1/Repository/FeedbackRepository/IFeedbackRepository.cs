using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObjects.Enums.Status;

namespace Repository.AccountRepository
{ 
    public interface IFeedbackRepository : IRepository<Feedback>
    {
        public Task<List<Feedback>> GetPaginationBelongUserAsync(string accountId, int pageNumber, int pageSize);
        public Task<List<Feedback>> GetPaginationWithSearchKeyBelongUserAsync(string accountId, string searchKey, int pageNumber, int pageSize);
        public Task<int> CountBelongUser(string accountId);
        public Task<List<Feedback>> GetPaginationAsync(FeedbackStatus status, int pageNumber, int pageSize);
        public Task<List<Feedback>> GetPaginationWithSearchKeyAsync(FeedbackStatus status, string searchKey, int pageNumber, int pageSize);
        public Task<int> Count(FeedbackStatus status);
    }
}
