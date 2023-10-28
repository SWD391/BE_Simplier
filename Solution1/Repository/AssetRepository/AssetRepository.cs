using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Repository.AssetHistoryRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.AssetRepository
{
    public class AssetRepository : IAssetRepository
    {
        private readonly FacifixContext _context;

        public AssetRepository()
        {
            _context = new FacifixContext();
        }

        public async Task CreateAsync(Asset entity)
        {
            _context.Assets.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset != null)
            {
                _context.Assets.Remove(asset);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Asset>> GetAllAsync()
        {
            return await _context.Assets.ToListAsync();
        }

        public async Task<Asset?> GetByIdAsync(string id)
        {
            return await _context.Assets.FindAsync(id);
        }

        public async Task<List<Asset>> GetPaginationWithSearchKeyAsync(string searchKey, int pageNumber, int pageSize)
        {
            return (await GetPaginationAsync(pageNumber, pageSize))
                .Where(asset => asset.AssetName.Contains(searchKey) || asset.Description.Contains(searchKey))
                .ToList();
        }

        public async Task<List<Asset>> GetPaginationAsync(int pageNumber, int pageSize)
        {
            return await _context.Assets
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task UpdateAsync(Asset entity)
        {
            var existingEntity = await _context.Assets.FindAsync(entity.AssetId);
            if (existingEntity == null) return;

            existingEntity.UpdateNonNullProperties(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<int> Count()
        {
            return await _context.Assets.CountAsync();
        }
    }
}
