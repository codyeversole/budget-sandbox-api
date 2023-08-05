using BudgetSandbox.Api.Models.Domain;

namespace BudgetSandbox.Api.Models.DTO
{
    public class AssetDto
    {
        public int? AssetId { get; set; }
        public string Description { get; set; } = null!;
        public decimal AmountValue { get; set; }

        public int SandboxId { get; set; }
    }
}
