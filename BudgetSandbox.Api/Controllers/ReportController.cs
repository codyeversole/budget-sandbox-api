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
    public class ReportController : ControllerBase
    {
        private readonly ISandboxService sandboxService;
        private readonly IReportService reportService;
        public ReportController(IReportService reportService, ISandboxService sandboxService)
        {
            this.reportService = reportService;
            this.sandboxService = sandboxService;
        }

        [HttpGet("{sandboxId}")]
        public async Task<ActionResult<ReportCashFlowDto>> GetAll([FromRoute] int sandboxId)
        {
            if (await sandboxService.HasAccessAsync(User.FindFirstValue(ClaimTypes.NameIdentifier), sandboxId))
            {
                return await reportService.ReportCashFlow(sandboxId);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
