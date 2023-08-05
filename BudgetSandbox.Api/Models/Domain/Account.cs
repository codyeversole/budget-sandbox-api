using BudgetSandbox.Api.Models.DTO;

namespace BudgetSandbox.Api.Models.Domain
{
    public class Account
    {
        public Account() { }
        public Account(AccountDto accountDto)
        {
            AccountId = accountDto.AccountId.GetValueOrDefault();
            Description = accountDto.Description;
            Balance = accountDto.Balance;
            SandboxId = accountDto.SandboxId;
            Positive = accountDto.Positive;
        }

        public int AccountId { get; set; }
        public string Description { get; set; } = null!;
        public decimal Balance { get; set; }
        public bool Positive { get; set; }

        public int SandboxId { get; set; }
        public Sandbox Sandbox { get; set; } = null!;

        public ICollection<AccountBucket> AccountBuckets { get; } = new List<AccountBucket>();
        public ICollection<CashFlowItem> CashFlowItems { get; } = new List<CashFlowItem>();
    }
}
