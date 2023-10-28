using BusinessObjects.Enums;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BusinessObjects.Enums.Status;

namespace Repository.FixTaskRepository
{
    public class FixTaskRepository : IFixTaskRepository
    {
        private readonly FacifixContext _context;

        public FixTaskRepository()
        {
            _context = new FacifixContext();
        }

        public async Task CreateAsync(FixTask entity)
        {
            try
            {
                _context.FixTasks.Add(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public async Task DeleteAsync(string id)
        {
            var task = await _context.FixTasks.FindAsync(id);
            if (task != null)
            {
                _context.FixTasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<FixTask>> GetAllAsync()
        {
            return await _context.FixTasks.ToListAsync();
        }

        public async Task<FixTask?> GetByIdAsync(string id)
        {
            return await _context.FixTasks.Include(task =>
            task.AssignedDetails).FirstOrDefaultAsync(row => row.TaskId == id);
        }

        public async Task<List<FixTask>> GetPaginationWithSearchKeyAsync(string searchKey, int pageNumber, int pageSize)
        {
            return (await GetPaginationAsync(pageNumber, pageSize))
                .Where(task => task.Description.Contains(searchKey) || task.Title.Contains(searchKey))
                .ToList();
        }

        public async Task<List<FixTask>> GetPaginationAsync(int pageNumber, int pageSize)
        {
            return await _context.FixTasks
                .Include(task =>
            task.AssignedDetails
            )
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task UpdateAsync(FixTask entity)
        {
            var existingEntity = await _context.FixTasks.FindAsync(entity.TaskId);
            if (existingEntity == null) return;

            existingEntity.UpdateNonNullProperties(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<FixTask?> GetFixTaskIncludeDetails(string fixTaskId)
        {
            return await _context.FixTasks.Include(f => f.AssignedDetails).FirstOrDefaultAsync(row => row.TaskId == fixTaskId);
        }

        public async Task<int> Count()
        {
            return await _context.FixTasks.CountAsync();
        }

        public async Task<List<FixTask>> GetPaginationAsync(FixTaskStatus status, int pageNumber, int pageSize)
        {
            return await _context.FixTasks
                .Where(task => task.Status == status)
              .Include(task =>
          task.AssignedDetails
          )
              .Skip((pageNumber - 1) * pageSize)
              .Take(pageSize)
              .ToListAsync();
        }

        public async Task<List<FixTask>> GetPaginationWithSearchKeyAsync(FixTaskStatus status, string searchKey, int pageNumber, int pageSize)
        {
            return (await GetPaginationAsync(status, pageNumber, pageSize))
              .Where(task => task.Description.Contains(searchKey) || task.Title.Contains(searchKey))
              .ToList();
        }

        public async Task<int> Count(FixTaskStatus status)
        {
            return await _context.FixTasks.Where(task => task.Status == status).CountAsync();
        }

        public async Task<List<FixTask>> GetPaginationBelongUserAsync(string accountId, int pageNumber, int pageSize)
        {
            return await _context.FixTasks
               .Where(task => task.AssignedDetails.Select(d => d.EmployeeId).Contains(accountId))
             .Include(task =>
         task.AssignedDetails
         )
             .Skip((pageNumber - 1) * pageSize)
             .Take(pageSize)
             .ToListAsync();
        }

        public async Task<List<FixTask>> GetPaginationWithSearchKeyBelongUserAsync(string accountId, string searchKey, int pageNumber, int pageSize)
        {
            return (await GetPaginationBelongUserAsync(accountId, pageNumber, pageSize))
            .Where(task => task.Description.Contains(searchKey) || task.Title.Contains(searchKey))
            .ToList();
        }

        public async Task<int> CountBelongUser(string accountId)
        {
            return await _context.FixTasks.Where(task => task.AssignedDetails.Select(d => d.EmployeeId).Contains(accountId)).CountAsync();
        }
    }
}

public class AssignedDetailsDTO
{

}
