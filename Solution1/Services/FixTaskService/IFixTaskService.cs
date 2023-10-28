using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObjects.Enums.Status;

namespace Services.FixTaskService
{
    public interface IFixTaskService : IService<FixTask>
    {
        public Task CreateFixTaskService(FixTask fixTask, List<string> employeeIds);
        public Task<FixTask?> UpdateFixTaskStatusService(string fixTaskId, FixTaskStatus status);
        public Task<FixTask?> UpdateFixTaskService(FixTask fixTask);
        public Task ReceiveFixTaskService(string employeeId, string fixTaskId, bool receive);
        public Task ProcessFixTaskService(string employeeId, string fixTaskId, bool process);
        public Task DeleteFixTaskService(string feedbackId);
        public Task<List<FixTask>> GetFixTaskPaginationService(int pageNumber, int pageSize);
        public Task<FixTask> GetFixTaskDetailsService(string fixTaskId);
        public Task<List<FixTask>> GetFixTaskPaginationWithSearchKeyService(string searchKey, int pageNumber, int pageSize);
        public Task<int> GetNumFixTasksService();
        public Task<List<FixTask>> GetFixTaskPaginationService(FixTaskStatus status, int pageNumber, int pageSize);
        public Task<List<FixTask>> GetFixTaskPaginationWithSearchKeyService(FixTaskStatus status, string searchKey, int pageNumber, int pageSize);
        public Task<int> GetNumFixTasksService(FixTaskStatus status);
        public Task<List<FixTask>> GetFixTaskPaginationBelongUserService(string accountId, int pageNumber, int pageSize);
        public Task<List<FixTask>> GetFixTaskPaginationWithSearchKeyBelongUserService(string accountId, string searchKey, int pageNumber, int pageSize);
        public Task<int> GetNumFixTasksBelongUserService(string accountId);
    }
}
