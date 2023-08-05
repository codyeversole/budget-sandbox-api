using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BudgetSandbox.Api.Models.Domain;
using BudgetSandbox.Api.Models.DTO;
using BudgetSandbox.Api.Services.Domain;
using System.Security.Claims;

namespace BudgetSandbox.Api.Controllers
{
    [Authorize(Policy = "NormalUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class SandboxController : ControllerBase
    {
        private readonly ISandboxService sandboxService;
        public SandboxController(ISandboxService sandboxService)
        {
            this.sandboxService = sandboxService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sandbox>>> Get()
        {
            return await sandboxService.GetAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Sandbox sandbox)
        {
            if (sandbox != null && ModelState.IsValid)
            {
                if (sandbox.SandboxId == 0 || await sandboxService.HasAccessAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), sandbox.SandboxId))
                {
                    await sandboxService.SaveAsync(sandbox, User.FindFirstValue(ClaimTypes.NameIdentifier));
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

        [HttpDelete("{SandboxId}")]
        public async Task<ActionResult> Delete([FromRoute] int sandboxId)
        {
            if (await sandboxService.HasAccessAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), sandboxId))
            {
                await sandboxService.DeleteAsync(sandboxId);
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
