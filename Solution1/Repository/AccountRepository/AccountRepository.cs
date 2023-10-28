using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Repository.AccountRepository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly FacifixContext _context;

        public AccountRepository()
        {
            _context = new FacifixContext();
        }

        public async Task CreateAsync(Account entity)
        {
            await _context.Accounts.AddAsync(entity);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAsync(string id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Account>> GetAllAsync()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task<Account?> GetByEmailAsync(string email)
        {
            return await _context.Accounts.FirstOrDefaultAsync(row => row.Email == email);
        }

        public async Task<Account?> GetByIdAsync(string id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task<List<Account>> GetPaginationWithSearchKeyAsync(string searchKey, int pageNumber, int pageSize)
        {
            return (await GetPaginationAsync(pageNumber, pageSize))
                .Where(account => account.Email.Contains(searchKey))
                .ToList();
        }

        public async Task<List<Account>> GetPaginationAsync(int pageNumber, int pageSize)
        {
            return await _context.Accounts
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task UpdateAsync(Account entity)
        {
                var existingEntity = await _context.Accounts.FindAsync(entity.AccountId);
                if (existingEntity == null) return;

                existingEntity.UpdateNonNullProperties(entity);
                await _context.SaveChangesAsync();
        }

        public async Task<int> Count()
        {
            return await _context.Accounts.CountAsync();
        }

        public async Task<List<Account>> GetPaginationAsync(Status.AccountRole role, int pageNumber, int pageSize)
        {
            return await _context.Accounts
                .Where(value => value.Role == role)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Account>> GetPaginationWithSearchKeyAsync(Status.AccountRole role, string searchKey, int pageNumber, int pageSize)
        {
            return (await GetPaginationAsync(role, pageNumber, pageSize))
                .Where(account => account.Email.Contains(searchKey))
                .ToList();
        }

        public async Task<int> Count(Status.AccountRole role)
        {
            return await _context.Accounts.Where(value => value.Role == role).CountAsync();
        }
    }
}
