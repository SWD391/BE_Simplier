using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository.NotificationRepository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly FacifixContext _context;

        public NotificationRepository(FacifixContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Notification entity)
        {
            _context.Notifications.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Notification>> GetAllAsync()
        {
            return await _context.Notifications.ToListAsync();
        }

        public async Task<Notification?> GetByIdAsync(string id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task<List<Notification>> GetPaginationWithSearchKeyAsync(string searchKey, int pageNumber, int pageSize)
        {
            return (await GetPaginationAsync(pageNumber, pageSize))
                .Where(notification => notification.Title.Contains(searchKey) || notification.Message.Contains(searchKey))
                .ToList();
        }

        public async Task<List<Notification>> GetPaginationAsync(int pageNumber, int pageSize)
        {
            return await _context.Notifications
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task UpdateAsync(Notification entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<int> Count()
        {
            return await _context.Notifications.CountAsync();
        }
    }
}
