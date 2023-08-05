using BudgetSandbox.Api.Models.DTO;

namespace BudgetSandbox.Api.Models.Domain
{
    public class Asset
    {
        public Asset() { }
        public Asset(AssetDto assetDto)
        {
            AssetId = assetDto.AssetId.GetValueOrDefault();
            Description = assetDto.Description;
            AmountValue = assetDto.AmountValue;
            SandboxId = assetDto.SandboxId;
        }

        public int AssetId { get; set; }
        public string Description { get; set; } = null!;
        public decimal AmountValue { get; set; }

        public int SandboxId { get; set; }
        public Sandbox Sandbox { get; set; } = null!;

        public ICollection<CashFlowItem> CashFlowItems { get; } = new List<CashFlowItem>();
    }
}
