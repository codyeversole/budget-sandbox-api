using System.ComponentModel.DataAnnotations;

namespace BudgetSandbox.Api.Models.Domain
{
    public class CashFlowItemBucket
    {
        public int CashFlowItemBucketId { get; set; }

        public int CashFlowItemId { get; set; }
        public CashFlowItem CashFlowItem { get; set; } = null!;

        public int BucketId { get; set; }
        public Bucket Bucket { get; set; } = null!;

        public decimal? Amount { get; set; }
        public decimal? Percent { get; set; }
    }
}
