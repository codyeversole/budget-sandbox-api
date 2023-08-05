using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BudgetSandbox.Api.Models.Domain;
using BudgetSandbox.Api.Models.DTO;
using BudgetSandbox.Api.Services.Domain;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Principal;

namespace BudgetSandbox.Api.Controllers
{
    [Authorize(Policy = "NormalUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class BucketController : ControllerBase
    {
        private readonly ISandboxService sandboxService;
        private readonly IBucketService bucketService;
        private readonly IAccountService accountService;
        public BucketController(IBucketService bucketService, ISandboxService sandboxService, IAccountService accountService)
        {
            this.bucketService = bucketService;
            this.sandboxService = sandboxService;
            this.accountService = accountService;
        }

        [HttpGet("all/{SandboxId}")]
        public async Task<ActionResult<IEnumerable<Bucket>>> GetAll([FromRoute] int sandboxId)
        {
            if (await sandboxService.HasAccessAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), sandboxId))
            {
                return await bucketService.GetAllAsync(sandboxId);
            }
            else
            {
                return Unauthorized();
            }            
        }

        [HttpGet("{BucketId}")]
        public async Task<ActionResult<Bucket?>> Get([FromRoute] int bucketId)
        {
            var bucket = await bucketService.GetAsync(bucketId);
            if (bucket != null && await sandboxService.HasAccessAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), bucket.SandboxId))
            {
                return bucket;
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] BucketDto bucketDto)
        {
            if(bucketDto == null)
            {
                return BadRequest();
            }
            else
            {
                var accounts = await accountService.GetAllAsync(bucketDto.SandboxId);
                if (bucketDto.AccountBuckets.Any(ab => accounts.FirstOrDefault(a => a.AccountId == ab.AccountId)?.Positive != bucketDto.Positive))
                {
                    ModelState.AddModelError("AccountBuckets", "Bucket must only be associated with accounts that match the buckets positive or negative property.");
                }

                if (ModelState.IsValid)
                {
                    if (await sandboxService.HasAccessAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), bucketDto.SandboxId))
                    {
                        Bucket bucket = new Bucket(bucketDto);
                        await bucketService.SaveAsync(bucket);
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
        }

        [HttpDelete("{BucketId}")]
        public async Task<ActionResult> Delete([FromRoute] int bucketId)
        {
            var bucket = await bucketService.GetAsync(bucketId);
            if (bucket != null && await sandboxService.HasAccessAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), bucket.SandboxId))
            {
                await bucketService.DeleteAsync(bucket);
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
