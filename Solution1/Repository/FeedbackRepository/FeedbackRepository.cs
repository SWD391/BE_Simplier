using BusinessObjects.Enums;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Repository.AccountRepository;
using System.Linq;
using System.Threading.Tasks;
using static BusinessObjects.Enums.Status;

namespace Repository
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly FacifixContext _context;

        public FeedbackRepository()
        {
            _context = new FacifixContext();
        }

        public async Task CreateAsync(Feedback entity)
        {
            try
            {
                await _context.Feedbacks.AddAsync(entity);
                await _context.SaveChangesAsync();
            } catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }
        }

        public async Task DeleteAsync(string id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback != null)
            {
                _context.Feedbacks.Remove(feedback);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Feedback>> GetAllAsync()
        {
            return await _context.Feedbacks.ToListAsync();
        }

        public async Task<Feedback?> GetByIdAsync(string id)
        {
            return await _context.Feedbacks.FindAsync(id);
        }

        public async Task<List<Feedback>> GetPaginationWithSearchKeyAsync(string searchKey, int pageNumber, int pageSize)
        {
            return (await GetPaginationAsync(pageNumber, pageSize))
                .Where(feedback => feedback.Description.Contains(searchKey) || feedback.Title.Contains(searchKey))
                .ToList();
        }

        public async Task<List<Feedback>> GetPaginationAsync(int pageNumber, int pageSize)
        {
            return await _context.Feedbacks
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task UpdateAsync(Feedback entity)
        {
                var existingEntity = await _context.Feedbacks.FindAsync(entity.FeedbackId);
                if (existingEntity == null) return;

                existingEntity.UpdateNonNullProperties(entity);
                await _context.SaveChangesAsync();
        }

        public async Task<int> Count()
        {
            return await _context.Feedbacks.CountAsync();
        }

        public async Task<List<Feedback>> GetPaginationBelongUserAsync(string accountId, int pageNumber, int pageSize)
        {
            return await _context.Feedbacks.Where(row => row.CreatorId == accountId)
      .Skip((pageNumber - 1) * pageSize)
      .Take(pageSize)
      .ToListAsync();
        }

        public async Task<List<Feedback>> GetPaginationWithSearchKeyBelongUserAsync(string accountId, string searchKey, int pageNumber, int pageSize)
        {
            return (await GetPaginationBelongUserAsync(accountId, pageNumber, pageSize))
              .Where(feedback => feedback.Description.Contains(searchKey) || feedback.Title.Contains(searchKey))
              .ToList();
        }

        public async Task<int> CountBelongUser(string accountId)
        {
            return await _context.Feedbacks.Where(row => row.CreatorId == accountId).CountAsync();
        }

        public async Task<List<Feedback>> GetPaginationAsync(Status.FeedbackStatus status, int pageNumber, int pageSize)
        {
            return await _context.Feedbacks
            .Where(predicate => predicate.Status == status)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        }

        public async Task<List<Feedback>> GetPaginationWithSearchKeyAsync(Status.FeedbackStatus status, string searchKey, int pageNumber, int pageSize)
        {
            return (await GetPaginationAsync(pageNumber, pageSize))
                .Where(feedback => feedback.Description.Contains(searchKey) || feedback.Title.Contains(searchKey))
                .ToList();
        }

        public async Task<int> Count(FeedbackStatus status)
        {
            return await _context.Feedbacks.Where(predicate => predicate.Status == status).CountAsync();
        }
    }
}