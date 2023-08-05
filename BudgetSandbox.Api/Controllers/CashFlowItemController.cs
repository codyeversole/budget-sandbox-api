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
    public class CashFlowItemController : ControllerBase
    {
        private readonly ISandboxService sandboxService;
        private readonly ICashFlowItemService cashFlowItemService;
        public CashFlowItemController(ICashFlowItemService cashFlowItemService, ISandboxService sandboxService)
        {
            this.cashFlowItemService = cashFlowItemService;
            this.sandboxService = sandboxService;
        }

        [HttpGet("all/{SandboxId}")]
        public async Task<ActionResult<IEnumerable<CashFlowItem>>> GetAll([FromRoute] int sandboxId)
        {
            if (await sandboxService.HasAccessAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), sandboxId))
            {
                return await cashFlowItemService.GetAllAsync(sandboxId);
            }
            else
            {
                return Unauthorized();
            }            
        }

        [HttpGet("{CashFlowItemId}")]
        public async Task<ActionResult<CashFlowItem?>> Get([FromRoute] int cashFlowItemId)
        {
            var cashFlowItem = await cashFlowItemService.GetAsync(cashFlowItemId);
            if (cashFlowItem != null && await sandboxService.HasAccessAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), cashFlowItem.SandboxId))
            {
                return cashFlowItem;
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CashFlowItemDto cashFlowItemDto)
        {
            if (cashFlowItemDto != null && ModelState.IsValid)
            {
                if (await sandboxService.HasAccessAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), cashFlowItemDto.SandboxId))
                {
                    CashFlowItem cashFlowItem = new CashFlowItem(cashFlowItemDto);
                    await cashFlowItemService.SaveAsync(cashFlowItem);
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

        [HttpDelete("{CashFlowItemId}")]
        public async Task<ActionResult> Delete([FromRoute] int cashFlowItemId)
        {
            var cashFlowItem = await cashFlowItemService.GetAsync(cashFlowItemId);
            if (cashFlowItem != null && await sandboxService.HasAccessAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), cashFlowItem.SandboxId))
            {
                await cashFlowItemService.DeleteAsync(cashFlowItem);
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
