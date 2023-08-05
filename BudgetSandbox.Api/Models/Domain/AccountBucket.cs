using System.ComponentModel.DataAnnotations;

namespace BudgetSandbox.Api.Models.Domain
{
    public class AccountBucket
    {
        public int AccountBucketId { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; } = null!;

        public int BucketId { get; set; }
        public Bucket Bucket { get; set; } = null!;

        public decimal? Amount { get; set; }
        public decimal? Percent { get; set; }
    }
}
