using BudgetSandbox.Api.Models.Domain;

namespace BudgetSandbox.Api.Models.DTO
{
    public class AccountDto
    {
        public int? AccountId { get; set; }
        public string Description { get; set; } = null!;
        public decimal Balance { get; set; }
        public bool Positive { get; set; }

        public int SandboxId { get; set; }
    }
}
