using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Services.AccountService;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Services.JwtService;
using static BusinessObjects.Enums.Status;
using Newtonsoft.Json;

namespace Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IJwtService _jwtService;

        public AccountsController(IAccountService accountService, IJwtService jwtService)
        {
            _accountService = accountService;
            _jwtService = jwtService;
        }

        // POST: api/Accounts/SignIn
        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInRequest request)
        {
            try
            {
                var response = await _accountService.SignInService(request.Email, request.Password);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: api/Accounts/SignUp
        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (request.Role > 1)
                {
                    throw new Exception("You cannot sign up for adminstrator");
                }

                var account = new Account()
                {
                    Address = request.Address,
                    Birthdate = request.Birthdate,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Role = (AccountRole) request.Role,
                    PhoneNumber = request.PhoneNumber,
                    Password = request.Password,
                };

                await _accountService.SignUpService(account);

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("GetAccount")]
        public async Task<ActionResult> GetAccount()
        {
            try
            {
                var accountId = _jwtService.GetAccountId(HttpContext);
                return accountId == null ? throw new Exception("Account not found") : Ok(await _accountService.GetAccountService(accountId));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

        [Authorize]
        [HttpPut("SelfUpdateAccount")]
        public async Task<ActionResult> SelfUpdateAccount(SelfUpdateAccountRequest request)
        {
            try
            {
                var accountId = _jwtService.GetAccountId(HttpContext) ?? throw new Exception("Account not found");

                var account = new Account()
                {   
                    AccountId = accountId,
                    Address = request.Address,
                    Birthdate = request.Birthdate,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber,
                    ImageUrl = request.ImageUrl,
                };

                return accountId == null ? throw new Exception("Account not found") : Ok(await _accountService.UpdateAccountService(account));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "AdministratorStaff,AdministratorManager")]
        [HttpPost]
        public async Task<IActionResult> PostAccount(AccountRequest request)
        {   
            Console.WriteLine(JsonConvert.SerializeObject(request));
            try
            {
                var account = new Account()
                {
                    Address = request.Address,
                    Birthdate = request.Birthdate,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Role = (AccountRole)request.Role,
                    PhoneNumber = request.PhoneNumber,
                    Password = request.Password,
                };

                await _accountService.CreateAccountService(account);
                return Ok("An account has been successfully created");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [Authorize(Roles = "AdministratorStaff,AdministratorManager")]
        [HttpGet("AccountPagination")]
        public async Task<ActionResult> AccountPagination(int pageNumber, int pageSize)
        {
            try
            {
                var list = await _accountService.GetAccountPaginationService(pageNumber, pageSize);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "AdministratorStaff,AdministratorManager")]
        [HttpGet("NumAccounts")]
        public async Task<ActionResult> NumAccounts()
        {
            try
            {
                var list = await _accountService.GetNumAccountService();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "AdminstratorStaff,AdministratorManager")]
        [HttpGet("AccountPaginationWithSearchKey")]
        public async Task<ActionResult> AccountPaginationWithSearchKey(string searchKey, int pageNumber, int pageSize)
        {
            try
            {
                var list = await _accountService.GetAccountPaginationWithSearchKeyService(searchKey, pageNumber, pageSize);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "AdministratorStaff,AdministratorManager")]
        [HttpGet("AccountPaginationWithRole")]
        public async Task<ActionResult> AccountPaginationWithRole(AccountRole role, int pageNumber, int pageSize)
        {
            try
            {
                var list = await _accountService.GetAccountPaginationService(role, pageNumber, pageSize);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "AdministratorStaff,AdministratorManager")]
        [HttpGet("NumAccountsWithRole")]
        public async Task<ActionResult> NumAccountsWithRole(AccountRole role)
        {
            try
            {
                var list = await _accountService.GetNumAccountService(role);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "AdminstratorStaff,AdministratorManager")]
        [HttpGet("AccountPaginationWithSearchKeyWithRole")]
        public async Task<ActionResult> AccountPaginationWithSearchKeyWithRole(AccountRole role, string searchKey, int pageNumber, int pageSize)
        {
            try
            {
                var list = await _accountService.GetAccountPaginationWithSearchKeyService(role, searchKey, pageNumber, pageSize);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "AdministratorStaff,AdministratorManager")]
        [HttpGet]
        public async Task<ActionResult> AccountDetails(string accountId)
        {
            try
            {
                var asset = await _accountService.GetAccountService(accountId);
                return Ok(asset);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "AdministratorStaff,AdministratorManager")]
        [HttpDelete]
        public async Task<ActionResult> DeleteAccount(string accountId)
        {
            try
            {
                await _accountService.DeleteAccountService(accountId);
                return Ok("Account has been deleted successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "AdministratorStaff,AdministratorManager")]
        [HttpPut]
        public async Task<ActionResult> PutAccount(AccountRequest request)
        {
            try
            {
                var account = new Account()
                {
                    Address = request.Address,
                    Birthdate = request.Birthdate,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Role = (AccountRole)request.Role,
                    PhoneNumber = request.PhoneNumber,
                    Password = request.Password,
                };

                await _accountService.UpdateAccountService(account);
                return Ok("Account has been updated successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

public class SignInRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; } = null!;
}

public class SignUpRequest
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime Birthdate { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int Role { get; set; }

    public string Address { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;
}

public class AccountRequest
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime Birthdate { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int Role { get; set; }

    public string Address { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;
}

public class SelfUpdateAccountRequest
{
    public string? ImageUrl { get; set; } = null!;
    public DateTime Birthdate { get; set; }
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;
}

