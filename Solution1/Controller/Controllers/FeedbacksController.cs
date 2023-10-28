using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Services.AccountService;
using Services.FeedbackService;
using Microsoft.AspNetCore.Authorization;
using Services.JwtService;
using static BusinessObjects.Enums.Status;
using Microsoft.Identity.Client;

namespace Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IJwtService _jwtService;

        public FeedbacksController(IFeedbackService feedbackService, IJwtService jwtService)
        {
            _feedbackService = feedbackService;
            _jwtService = jwtService;
        }

        [Authorize]       
        [HttpPost]
        public async Task<ActionResult> PostFeedback(FeedbackRequest request)
        {
            try
            {
                var accountId = _jwtService.GetAccountId(HttpContext) ?? throw new Exception("NotFound");

                var feedback = new Feedback()
                {   
                    Title = request.Title,
                    Description = request.Description,
                    ImageUrl = request.ImageUrl,
                    AssetId = request.AssetId,
                    CreatorId = accountId
                };

                await _feedbackService.CreateFeedbackService(feedback);
                return Ok("Feedback has been created successfully");
            } catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

        [Authorize(Roles = "AdministratorStaff,AdministratorManager")]
        [HttpPost("SubmitFeedback")]
        public async Task<ActionResult> SubmitFeedback(SubmitFeedbackRequest request)
        {
            try
            {
                await _feedbackService.SubmitFeedbackService(request.FeedbackId, request.Approve);
                return Ok("Feedback has been submit successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "AdministratorStaff,AdministratorManager")]
        [HttpGet("FeedbackPagination")]
        public async Task<ActionResult> FeedbackPagination(int pageNumber, int pageSize)
        {
            try
            {
                var list = await _feedbackService.GetFeedbackPaginationService(pageNumber, pageSize);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "AdministratorStaff,AdministratorManager")]
        [HttpGet("NumFeedbacks")]
        public async Task<ActionResult> NumFeedbacks()
        {
            try
            {
                var list = await _feedbackService.GetNumFeedbacksService();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "AdministratorStaff,AdministratorManager")]
        [HttpGet("FeedbackPaginationWithSearchKey")]
        public async Task<ActionResult> FeedbackPaginationWithSearchKey(string searchKey, int pageNumber, int pageSize)
        {
            try
            {
                var list = await _feedbackService.GetFeedbackPaginationWithSearchKeyService(searchKey, pageNumber, pageSize);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [Authorize(Roles = "AdministratorStaff,AdministratorManager")]
        [HttpGet("FeedbackPaginationWithStatus")]
        public async Task<ActionResult> FeedbackPaginationWithStatus(int status, int pageNumber, int pageSize)
        {
            try
            {
                var list = await _feedbackService.GetFeedbackPaginationService((FeedbackStatus) status, pageNumber, pageSize);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "AdministratorStaff,AdministratorManager")]
        [HttpGet("NumFeedbacksWithStatus")]
        public async Task<ActionResult> NumFeedbacks(int status)
        {
            try
            {
                var list = await _feedbackService.GetNumFeedbacksService((FeedbackStatus) status);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "AdministratorStaff,AdministratorManager")]
        [HttpGet("FeedbackPaginationWithSearchKeyWithStatus")]
        public async Task<ActionResult> FeedbackPaginationWithSearchKeyWithStatus(int status, string searchKey, int pageNumber, int pageSize)
        {
            try
            {
                var list = await _feedbackService.GetFeedbackPaginationWithSearchKeyService((FeedbackStatus)status, searchKey, pageNumber, pageSize);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("FeedbackPaginationBelongUser")]
        public async Task<ActionResult> FeedbackPaginationBelongUser(int pageNumber, int pageSize)
        {
            try
            {
                var accountId = _jwtService.GetAccountId(HttpContext) ?? throw new Exception("NotFound");
                var list = await _feedbackService.GetFeedbackPaginationBelongUserService(accountId, pageNumber, pageSize);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("NumFeedbacksBelongUser")]
        public async Task<ActionResult> NumFeedbacksBelongUser()
        {
            try
            {
                var accountId = _jwtService.GetAccountId(HttpContext) ?? throw new Exception("NotFound");
                var list = await _feedbackService.GetNumFeedbacksBelongUserService(accountId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("FeedbackPaginationWithSearchKeyBelongUser")]
        public async Task<ActionResult> FeedbackPaginationWithSearchKeyBelongUser(string searchKey, int pageNumber, int pageSize)
        {
            try
            {
                var accountId = _jwtService.GetAccountId(HttpContext) ?? throw new Exception("NotFound");
                var list = await _feedbackService.GetFeedbackPaginationWithSearchKeyBelongUserService(accountId, searchKey, pageNumber, pageSize);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "AdministratorStaff,AdministratorManager")]
        [HttpGet]
        public async Task<ActionResult> FeedbackDetails(string feedbackId)
        {
            try
            {
                var feedback = await _feedbackService.GetFeedbackDetailsService(feedbackId);
                return Ok(feedback);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "AdministratorStaff,AdministratorManager")]
        [HttpPut]
        public async Task<ActionResult> PutFeedback(FeedbackRequest request)
        {
            try
            {
                var feedback = new Feedback()
                {
                    Title = request.Title,
                    Description = request.Description,
                    ImageUrl = request.ImageUrl,
                    AssetId = request.AssetId
                };

                await _feedbackService.UpdateFeedbackService(feedback);
                var _feedback = await _feedbackService.GetFeedbackDetailsService(feedback.FeedbackId);
                return Ok(_feedback);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "AdministratorStaff,AdministratorManager")]
        [HttpDelete]
        public async Task<ActionResult> DeleteFeedback(string feedbackId)
        {
            try
            {
                await _feedbackService.DeleteFeedbackService(feedbackId);
                return Ok("Feedback has been deleted successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

public class FeedbackRequest
{
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public string AssetId { get; set; } = null!;
}

public class SubmitFeedbackRequest
{
    public string FeedbackId { get; set; } = null!;
    public bool Approve { get; set; }
}