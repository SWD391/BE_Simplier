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
using Microsoft.Identity.Client;

namespace Controllers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        private readonly IAssetService _assetService;
        private readonly IJwtService _jwtService;

        public AssetController(IAssetService assetService, IJwtService jwtService)
        {
            _assetService = assetService;
            _jwtService = jwtService;
        }

        [Authorize(Roles = "AdministratorStaff,AdministratorManager")]
        [HttpPost]
        public async Task<IActionResult> PostAsset(AssetRequest request)
        {
            try
            {
                var accountId = _jwtService.GetAccountId(HttpContext);
                if (accountId == null) throw new Exception("Account not found");
                var asset = new Asset()
                {
                    AssetName = request.AssetName,
                    Color = request.Color,
                    Description = request.Description,
                    ImageUrl = request.ImageUrl,
                    Price = request.Price,
                    Type = request.Type,
                    ImporterId = accountId,
                    Location = request.Location,
                };

                await _assetService.CreateAssetService(asset);
                return Ok("An asset has been successfully created");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [AllowAnonymous]
        [HttpGet("AssetPagination")]
        public async Task<ActionResult> AssetPagination(int pageNumber, int pageSize)
        {
            try
            {
                var list = await _assetService.GetAssetPaginationService(pageNumber, pageSize);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("NumAssets")]
        public async Task<ActionResult> NumAssets()
        {
            try
            {
                var list = await _assetService.GetNumAssetsService();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("AssetPaginationWithSearchKey")]
        public async Task<ActionResult> AssetPaginationWithSearchKey(string searchKey, int pageNumber, int pageSize)
        {
            try
            {
                var list = await _assetService.GetAssetPaginationWithSearchKeyService(searchKey, pageNumber, pageSize);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> AssetDetails(string assetId)
        {
            try
            {
                var asset = await _assetService.GetAssetDetailsService(assetId);
                return Ok(asset);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("All")]
        public async Task<ActionResult> AllAssets()
        {
            try
            {
                var list = await _assetService.GetAllAssetsService();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "AdministratorStaff,AdministratorManager")]
        [HttpDelete]
        public async Task<ActionResult> DeleteAsset(string assetId)
        {
            try
            {
                await _assetService.DeleteAssetService(assetId);
                return Ok("Feedback has been deleted successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "AdministratorStaff,AdministratorManager")]
        [HttpPut]
        public async Task<ActionResult> PutAsset(AssetRequest request)
        {
            try
            {
                var asset = new Asset()
                {
                    AssetName = request.AssetName,
                    Color = request.Color,
                    Description = request.Description,
                    ImageUrl = request.ImageUrl,
                    Price = request.Price,
                    Type = request.Type,
                    Location = request.Location
                };

                await _assetService.UpdateAssetService(asset);
                return Ok("Feedback has been deleted successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
public class AssetRequest
{
   public string AssetName { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public string Color { get; set; } = null!;

    public string Type { get; set; } = null!;

    public double Price { get; set; }

    public string Description { get; set; } = null!;

    public string Location { get; set; } = null!;
}