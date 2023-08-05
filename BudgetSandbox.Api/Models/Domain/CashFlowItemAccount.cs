using System.ComponentModel.DataAnnotations;

namespace BudgetSandbox.Api.Models.Domain
{
    public class CashFlowItemAccount
    {
        public int CashFlowItemAccountId { get; set; }

        public int CashFlowItemId { get; set; }
        public CashFlowItem CashFlowItem { get; set; } = null!;

        public int AccountId { get; set; }
        public Account Account { get; set; } = null!;

        public decimal? Amount { get; set; }
        public decimal? Percent { get; set; }
    }
}
