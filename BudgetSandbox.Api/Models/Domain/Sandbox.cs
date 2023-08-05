namespace BudgetSandbox.Api.Models.Domain
{
    public class Sandbox
    {
        public int SandboxId { get; set; }
        public string Description { get; set; } = null!;

        public ICollection<Account> Accounts { get; } = new List<Account>();
        public ICollection<Bucket> Buckets { get; } = new List<Bucket>();
        public ICollection<Asset> Assets { get; } = new List<Asset>();
        public ICollection<CashFlowItem> CashFlowItems { get; } = new List<CashFlowItem>();
    }
}
