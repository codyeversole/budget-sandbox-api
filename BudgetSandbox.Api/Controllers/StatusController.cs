using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BudgetSandbox.Api.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BudgetSandbox.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Budget sandbox api working";
        }
    }
}
