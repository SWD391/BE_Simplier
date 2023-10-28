using BusinessObjects.Enums;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Repository.FixTaskRepository;
using System.Diagnostics;
using static BusinessObjects.Enums.Status;

namespace Services.FixTaskService
{
    public class FixTaskService : IFixTaskService
    {
        private readonly IFixTaskRepository _fixTaskRepository;
        public FixTaskService()
        {
            _fixTaskRepository = new FixTaskRepository(); // Initialize your repository here
        }

        public async Task CreateFixTaskService(FixTask fixTask, List<string> employeeIds)
        {
            var id = Guid.NewGuid().ToString();
            fixTask.TaskId = id;
            fixTask.CreatedDate = DateTime.Now;

            var details = employeeIds.Select(_id => new AssignedDetail()
            {
                EmployeeId = _id,
                TaskId = id,
                AssignedDetailsId = Guid.NewGuid().ToString()
            }).ToList();

            fixTask.AssignedDetails = details;

            await _fixTaskRepository.CreateAsync(fixTask);
        }

        public async Task DeleteFixTaskService(string fixTaskId)
        {
            await _fixTaskRepository.DeleteAsync(fixTaskId);
        }

        public async Task<FixTask> GetFixTaskDetailsService(string fixTaskId)
        {
            return await _fixTaskRepository.GetByIdAsync(fixTaskId) ?? throw new Exception("NotFound");
        }

        public async Task<List<FixTask>> GetFixTaskPaginationService(int pageNumber, int pageSize)
        {
            return await _fixTaskRepository.GetPaginationAsync(pageNumber, pageSize);
        }

        public async Task ProcessFixTaskService(string employeeId, string fixTaskId, bool process)
        {
            var fixTask = await _fixTaskRepository.GetFixTaskIncludeDetails(fixTaskId) ?? throw new Exception("Task not found");
            var details = fixTask.AssignedDetails;

            var exist = details.FirstOrDefault(d => d.EmployeeId == employeeId) ?? throw new Exception("Only employee can process task");
            
            fixTask.Status = process ? FixTaskStatus.Completed : FixTaskStatus.Uncompleted;
            fixTask.ProcessedDate = DateTime.Now;
            await _fixTaskRepository.UpdateAsync(fixTask);
        }

        public async Task ReceiveFixTaskService(string employeeId, string fixTaskId, bool receive)
        {
            var fixTask = await _fixTaskRepository.GetFixTaskIncludeDetails(fixTaskId) ?? throw new Exception("Task not found");
            var details = fixTask.AssignedDetails;

            var exist = details.FirstOrDefault(d => d.EmployeeId == employeeId) ?? throw new Exception("Only employee can receive task");

      
            fixTask.Status = receive ? FixTaskStatus.Accepted : FixTaskStatus.Rejected;
            fixTask.ReceivedDate = DateTime.Now;
            await _fixTaskRepository.UpdateAsync(fixTask);
        }

        public async Task<FixTask?> UpdateFixTaskService(FixTask fixTask)
        {
            await _fixTaskRepository.UpdateAsync(fixTask);
            return await _fixTaskRepository.GetByIdAsync(fixTask.TaskId);
        }

        public async Task<FixTask?> UpdateFixTaskStatusService(string fixTaskId, FixTaskStatus status)
        {
            var fixTask = await _fixTaskRepository.GetByIdAsync(fixTaskId) ?? throw new Exception("NotFound");
            fixTask.Status = status;

            await _fixTaskRepository.UpdateAsync(fixTask);
            return fixTask;
        }

        public async Task<List<FixTask>> GetFixTaskPaginationWithSearchKeyService(string searchKey, int pageNumber, int pageSize)
        {
            return await _fixTaskRepository.GetPaginationWithSearchKeyAsync(searchKey, pageNumber, pageSize);
        }

        public async Task<int> GetNumFixTasksService()
        {
            return await _fixTaskRepository.Count();
        }

        public async Task<List<FixTask>> GetFixTaskPaginationService(FixTaskStatus status, int pageNumber, int pageSize)
        {
            return await _fixTaskRepository.GetPaginationAsync(status, pageNumber, pageSize);
        }

        public async Task<List<FixTask>> GetFixTaskPaginationWithSearchKeyService(FixTaskStatus status, string searchKey, int pageNumber, int pageSize)
        {
            return await _fixTaskRepository.GetPaginationWithSearchKeyAsync(status, searchKey, pageNumber, pageSize);
        }

        public async Task<int> GetNumFixTasksService(FixTaskStatus status)
        {
            return await _fixTaskRepository.Count(status);
        }

        public async Task<List<FixTask>> GetFixTaskPaginationBelongUserService(string accountId, int pageNumber, int pageSize)
        {
            return await _fixTaskRepository.GetPaginationBelongUserAsync(accountId, pageNumber, pageSize);
        }

        public async Task<List<FixTask>> GetFixTaskPaginationWithSearchKeyBelongUserService(string accountId, string searchKey, int pageNumber, int pageSize)
        {
            return await _fixTaskRepository.GetPaginationWithSearchKeyBelongUserAsync(accountId, searchKey, pageNumber, pageSize);
        }

        public async Task<int> GetNumFixTasksBelongUserService(string accountId)
        {
            return await _fixTaskRepository.CountBelongUser(accountId);
        }
    }
}
