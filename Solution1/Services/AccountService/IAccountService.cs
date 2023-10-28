using BusinessObjects.Models;
using Repository.AccountRepository;
using static BusinessObjects.Enums.Status;

namespace Services.AccountService
{
    public interface IAccountService : IService<Account>
    {
        public Task<SignInResponse> SignInService(string email, string password);
        public Task SignUpService(Account account);
        public Task<Account?> GetAccountService(string accountId);
        public Task CreateAccountService(Account account);
        public Task<Account?> UpdateAccountService(Account account);
        public Task DeleteAccountService(string accountId);
        public Task<List<Account>> GetAccountPaginationService(int pageNumber, int pageSize);
        public Task<List<Account>> GetAccountPaginationWithSearchKeyService(string searchKey, int pageNumber, int pageSize);
        public Task<int> GetNumAccountService();
        public Task<List<Account>> GetAccountPaginationService(AccountRole role, int pageNumber, int pageSize);
        public Task<List<Account>> GetAccountPaginationWithSearchKeyService(AccountRole role, string searchKey, int pageNumber, int pageSize);
        public Task<int> GetNumAccountService(AccountRole role);
    }
}
