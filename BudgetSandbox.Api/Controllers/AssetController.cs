using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BudgetSandbox.Api.Models.Domain;
using BudgetSandbox.Api.Models.DTO;
using BudgetSandbox.Api.Services.Domain;
using System.Security.Claims;
using System.Security.Principal;

namespace BudgetSandbox.Api.Controllers
{
    [Authorize(Policy = "NormalUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        private readonly ISandboxService sandboxService;
        private readonly IAssetService assetService;
        public AssetController(IAssetService assetService, ISandboxService sandboxService)
        {
            this.assetService = assetService;
            this.sandboxService = sandboxService;
        }

        [HttpGet("all/{SandboxId}")]
        public async Task<ActionResult<IEnumerable<Asset>>> GetAll([FromRoute] int sandboxId)
        {
            if (await sandboxService.HasAccessAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), sandboxId))
            {
                return await assetService.GetAllAsync(sandboxId);
            }
            else
            {
                return Unauthorized();
            }            
        }

        [HttpGet("{AssetId}")]
        public async Task<ActionResult<Asset?>> Get([FromRoute] int assetId)
        {
            var asset = await assetService.GetAsync(assetId);
            if (asset != null && await sandboxService.HasAccessAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), asset.SandboxId))
            {
                return asset;
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AssetDto assetDto)
        {
            if(assetDto != null && ModelState.IsValid)
            {
                if (await sandboxService.HasAccessAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), assetDto.SandboxId))
                {
                    Asset asset = new Asset(assetDto);
                    await assetService.SaveAsync(asset);
                    return Ok();
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("{AssetId}")]
        public async Task<ActionResult> Delete([FromRoute] int assetId)
        {
            var asset = await assetService.GetAsync(assetId);
            if (asset != null && await sandboxService.HasAccessAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), asset.SandboxId))
            {
                await assetService.DeleteAsync(asset);
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
