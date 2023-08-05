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
    public class AccountController : ControllerBase
    {
        private readonly ISandboxService sandboxService;
        private readonly IAccountService accountService;
        public AccountController(IAccountService accountService, ISandboxService sandboxService)
        {
            this.accountService = accountService;
            this.sandboxService = sandboxService;
        }

        [HttpGet("all/{SandboxId}")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAll([FromRoute] int sandboxId)
        {
            if (await sandboxService.HasAccessAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), sandboxId))
            {
                return await accountService.GetAllAsync(sandboxId);
            }
            else
            {
                return Unauthorized();
            }            
        }

        [HttpGet("{AccountId}")]
        public async Task<ActionResult<Account?>> Get([FromRoute] int accountId)
        {
            var account = await accountService.GetAsync(accountId);
            if (account != null && await sandboxService.HasAccessAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), account.SandboxId))
            {
                return account;
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AccountDto accountDto)
        {
            if(accountDto != null && ModelState.IsValid)
            {
                if (await sandboxService.HasAccessAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), accountDto.SandboxId))
                {
                    Account account = new Account(accountDto);
                    await accountService.SaveAsync(account);
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

        [HttpDelete("{AccountId}")]
        public async Task<ActionResult> Delete([FromRoute] int accountId)
        {
            var account = await accountService.GetAsync(accountId);
            if (account != null && await sandboxService.HasAccessAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), account.SandboxId))
            {
                await accountService.DeleteAsync(account);
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
