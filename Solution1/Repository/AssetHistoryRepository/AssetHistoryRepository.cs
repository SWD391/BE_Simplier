using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.AssetHistoryRepository
{
    public class AssetHistoryRepository : IAssetHistoryRepository
    {
        private readonly FacifixContext _context;

        public AssetHistoryRepository()
        {
            _context = new FacifixContext();
        }

        public async Task CreateAsync(AssetHistory entity)
        {
            _context.AssetHistories.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var assetHistory = await _context.AssetHistories.FindAsync(id);
            if (assetHistory != null)
            {
                _context.AssetHistories.Remove(assetHistory);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<AssetHistory>> GetAllAsync()
        {
            return await _context.AssetHistories.ToListAsync();
        }

        public async Task<AssetHistory?> GetByIdAsync(string id)
        {
            return await _context.AssetHistories.FindAsync(id);
        }

        public Task<List<AssetHistory>> GetPaginationWithSearchKeyAsync(string searchKey, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AssetHistory>> GetPaginationAsync(int pageNumber, int pageSize)
        {
            return await _context.AssetHistories
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task UpdateAsync(AssetHistory entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<int> Count()
        {
            return await _context.AssetHistories.CountAsync();
        }
    }
}
