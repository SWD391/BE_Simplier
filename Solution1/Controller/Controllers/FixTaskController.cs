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
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Services.JwtService;
using Services.AssetService;
using Services.FixTaskService;
using Microsoft.Identity.Client;
using static BusinessObjects.Enums.Status;
using AutoMapper;

namespace Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FixTaskController : ControllerBase
    {
        private readonly IFixTaskService _fixTaskService;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public FixTaskController(IFixTaskService fixTaskService, IJwtService jwtService, IMapper mapper)
        {
            _fixTaskService = fixTaskService;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        [Authorize(Roles =  "AdministratorStaff,AdministratorManager")]
        [HttpPost]
        public async Task<IActionResult> PostFixTask(FixTaskRequest request)
        {
            try
            {
                var accountId = _jwtService.GetAccountId(HttpContext) ?? throw new Exception("Account not found");
                var fixTask = new FixTask()
                {
                    Title = request.Title,
                    Description = request.Description,
                    AuthorId = accountId,
                    FeedbackId = request.FeedbackId,
                    Deadline = request.Deadline,
                };

                await _fixTaskService.CreateFixTaskService(fixTask, request.EmployeeIds);
                return Ok("A fix task has been successfully created");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "AdministratorStaff,AdministratorManager")]
        [HttpGet("FixTaskPagination")]
        public async Task<ActionResult> FixTaskPagination(int pageNumber, int pageSize)
        {
            try
            {
                var list = await _fixTaskService.GetFixTaskPaginationService(pageNumber, pageSize);
                var _list = _mapper.Map<List<FixTask>, List<FixTaskDTO>>(list);
                return Ok(_list);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "AdministratorStaff,AdministratorManager")]
        [HttpGet("NumFixTasks")]
        public async Task<ActionResult> NumFixTasks()
        {
            try
            {
                var numTasks = await _fixTaskService.GetNumFixTasksService();
                return Ok(numTasks);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "AdministratorStaff,AdministratorManager")]
        [HttpGet("FixTaskPaginationWithSearchKey")]
        public async Task<ActionResult> FixTaskPaginationWithSearchKey(string searchKey, int pageNumber, int pageSize)
        {
            try
            {
                var list = await _fixTaskService.GetFixTaskPaginationWithSearchKeyService(searchKey, pageNumber, pageSize);
                var _list = _mapper.Map<List<FixTask>, List<FixTaskDTO>>(list);
                return Ok(_list);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("FixTaskPaginationWithStatus")]
        public async Task<ActionResult> FixTaskPaginationWithStatus(FixTaskStatus status, int pageNumber, int pageSize)
        {
            try
            {
                var list = await _fixTaskService.GetFixTaskPaginationService(status, pageNumber, pageSize);
                var _list = _mapper.Map<List<FixTask>, List<FixTaskDTO>>(list);
                return Ok(_list);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("NumFixTasksWithStatus")]
        public async Task<ActionResult> NumFixTasksWithStatus(FixTaskStatus status)
        {
            try
            {
                var numTasks = await _fixTaskService.GetNumFixTasksService(status);
                return Ok(numTasks);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("FixTaskPaginationWithSearchKeyWithStatus")]
        public async Task<ActionResult> FixTaskPaginationWithSearchKeyWithStatus(FixTaskStatus status, string searchKey, int pageNumber, int pageSize)
        {
            try
            {
                var list = await _fixTaskService.GetFixTaskPaginationWithSearchKeyService(status, searchKey, pageNumber, pageSize);
                var _list = _mapper.Map<List<FixTask>, List<FixTaskDTO>>(list);
                return Ok(_list);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }



        [Authorize(Roles = "Employee")]
        [HttpGet("FixTaskPaginationBelongUser")]
        public async Task<ActionResult> FixTaskPaginationBelongUser(int pageNumber, int pageSize)
        {
            try
            {
                var accountId = _jwtService.GetAccountId(HttpContext) ?? throw new Exception("Account not found");
                var list = await _fixTaskService.GetFixTaskPaginationBelongUserService(accountId, pageNumber, pageSize);
                var _list = _mapper.Map<List<FixTask>, List<FixTaskDTO>>(list);
                return Ok(_list);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("NumFixTasksBelongUser")]
        public async Task<ActionResult> NumFixTasksBelongUser()
        {
            try
            {
                var accountId = _jwtService.GetAccountId(HttpContext) ?? throw new Exception("Account not found");
                var numTasks = await _fixTaskService.GetNumFixTasksBelongUserService(accountId);
                return Ok(numTasks);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("FixTaskPaginationWithSearchKeyBelongUser")]
        public async Task<ActionResult> FixTaskPaginationWithSearchKeyBelongUser(string searchKey, int pageNumber, int pageSize)
        {
            try
            {
                var accountId = _jwtService.GetAccountId(HttpContext) ?? throw new Exception("Account not found");
                var list = await _fixTaskService.GetFixTaskPaginationWithSearchKeyBelongUserService(accountId, searchKey, pageNumber, pageSize);
                var _list = _mapper.Map<List<FixTask>, List<FixTaskDTO>>(list);
                return Ok(_list);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }














        [Authorize(Roles = "Employee")]
        [HttpPut("ReceiveFixTask")]
        public async Task<ActionResult> ReceiveFixTask(HandleFixTaskRequest request)
        {
            try
            {
                var accountId = _jwtService.GetAccountId(HttpContext) ?? throw new Exception("Account not found");
                await _fixTaskService.ReceiveFixTaskService(accountId, request.FixTaskId, request.Handle);
                return Ok("Fix task has been received");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "Employee")]
        [HttpPut("ProcessFixTask")]
        public async Task<ActionResult> ProcessFixTask(HandleFixTaskRequest request)
        {
            try
            {
                var accountId = _jwtService.GetAccountId(HttpContext) ?? throw new Exception("Account not found");
                await _fixTaskService.ProcessFixTaskService(accountId, request.FixTaskId, request.Handle);
                return Ok("Fix task has been processed");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [Authorize(Roles = "Employee,AdministratorStaff,AdministratorManager")]
        [HttpGet]
        public async Task<ActionResult> FixTaskDetails(string taskId)
        {
            try
            {
                var fixTask = await _fixTaskService.GetFixTaskDetailsService(taskId);
                var _fixTask = _mapper.Map<FixTask, FixTaskDTO>(fixTask);
                return Ok(_fixTask);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "AdministratorStaff,AdministratorManager")]
        [HttpPut]
        public async Task<ActionResult> PutFixTask(FixTaskRequest request)
        {
            try
            {
                var fixTask = new FixTask()
                {
                    Title = request.Title,
                    Description = request.Description,
                    FeedbackId = request.FeedbackId,
                    Deadline = request.Deadline
                };

                await _fixTaskService.UpdateFixTaskService(fixTask);
                var _feedback = await _fixTaskService.GetFixTaskDetailsService(fixTask.TaskId);
                return Ok(_feedback);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "AdministratorStaff,AdministratorManager")]
        [HttpDelete]
        public async Task<ActionResult> DeleteFeedback(string fixTaskId)
        {
            try
            {
                await _fixTaskService.DeleteFixTaskService(fixTaskId);
                return Ok("Fix task has been deleted successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }



    }
}
public class FixTaskRequest
{
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime Deadline { get; set; }
    public string FeedbackId { get; set; } = null!;
    public List<string> EmployeeIds { get; set; } = new List<string>();
}

public class FixTaskDTO
{
    public string TaskId { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string AuthorId { get; set; } = null!;

    public FixTaskStatus Status { get; set; }

    public DateTime? ReceivedDate { get; set; }

    public DateTime? ProcessedDate { get; set; }

    public string FeedbackId { get; set; } = null!;

    public DateTime Deadline { get; set; }

    public List<AssignedDetailDTO> AssignedDetails { get; set; } = new List<AssignedDetailDTO>();
}

public class AssignedDetailDTO
{
    public string EmployeeId { get; set; } = null!;

    public string TaskId { get; set; } = null!;

    public string AssignedDetailsId { get; set; } = null!;
}

public class HandleFixTaskRequest
{
    public string FixTaskId { get; set; } = null!;
    public bool Handle { get; set; }
}