using BusinessObjects.Enums;
using BusinessObjects.Models;
using Repository.AccountRepository;
using Services.JwtService;
using static BusinessObjects.Enums.Status;

namespace Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IJwtService _jwtService;
        private readonly Sha256Service _sha256Service;

        public AccountService()
        {
            _accountRepository = new AccountRepository();
            _jwtService = new JwtService.JwtService();
            _sha256Service = new Sha256Service();
        }

        public async Task<SignInResponse> SignInService(string email, string password)
        {
            var account = await _accountRepository.GetByEmailAsync(email) ?? throw new Exception("Account not found");
            if (!_sha256Service.VerifyPassword(password, account.Password)) throw new Exception("Incorrect password");

            var accessToken = _jwtService.GenerateToken(account);
            if (accessToken == null) throw new Exception("Cannot generate access token");

            return new SignInResponse()
            {
                AccessToken = accessToken,
                Account = account
            };
        }
        public async Task SignUpService(Account account)
        {
            var id = Guid.NewGuid().ToString();
            account.AccountId = id;

            var existingAccount = await _accountRepository.GetByEmailAsync(account.Email);

            if (existingAccount != null)
            {
                throw new Exception("Email already exists");
            }
            string hashedPassword = _sha256Service.HashPassword(account.Password);
            account.Password = hashedPassword;
            await _accountRepository.CreateAsync(account);
        }

        public async Task<Account?> GetAccountService(string accountId)
        {
            return await _accountRepository.GetByIdAsync(accountId);
        }

        public async Task<Account?> UpdateAccountService(Account account)
        {
            await _accountRepository.UpdateAsync(account);
            return await _accountRepository.GetByIdAsync(account.AccountId);
        }

        public async Task CreateAccountService(Account account)
        {   
            account.AccountId = Guid.NewGuid().ToString();
            await _accountRepository.CreateAsync(account);
        }

        public async Task DeleteAccountService(string accountId)
        {
            await _accountRepository.DeleteAsync(accountId);
        }

        public async Task<List<Account>> GetAccountPaginationService(int pageNumber, int pageSize)
        {
            return await _accountRepository.GetPaginationAsync(pageNumber, pageSize);
        }

        public async Task<List<Account>> GetAccountPaginationWithSearchKeyService(string searchKey, int pageNumber, int pageSize)
        {
            return await _accountRepository.GetPaginationWithSearchKeyAsync(searchKey, pageNumber, pageSize);
        }

        public async Task<int> GetNumAccountService()
        {
            return await _accountRepository.Count();
        }

        public async Task<List<Account>> GetAccountPaginationService(AccountRole role, int pageNumber, int pageSize)
        {
            return await _accountRepository.GetPaginationAsync(role, pageNumber, pageSize);
        }

        public async Task<List<Account>> GetAccountPaginationWithSearchKeyService(AccountRole role, string searchKey, int pageNumber, int pageSize)
        {
            return await _accountRepository.GetPaginationWithSearchKeyAsync(role, searchKey, pageNumber, pageSize);
        }

        public async Task<int> GetNumAccountService(AccountRole role)
        {
            return await _accountRepository.Count(role);
        }
    }
}

public class SignInResponse
{
    public string AccessToken { get; set; }
    public Account Account { get; set; }
}
