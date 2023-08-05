using BudgetSandbox.Api.Models.DTO;
using System.ComponentModel.DataAnnotations;

namespace BudgetSandbox.Api.Models.Domain
{
    public class Bucket
    {
        public Bucket() { }
        public Bucket(BucketDto bucketDto)
        {
            BucketId = bucketDto.BucketId.GetValueOrDefault();
            Description = bucketDto.Description;
            Balance = bucketDto.Balance;
            GoalBalance = bucketDto.GoalBalance;
            GoalAchieved = bucketDto.GoalAchieved;
            Positive = bucketDto.Positive;
            Archived = bucketDto.Archived;
            SandboxId = bucketDto.SandboxId;
            AccountBuckets = bucketDto.AccountBuckets.Select(dto => new AccountBucket 
            { 
                AccountBucketId = dto.AccountBucketId.GetValueOrDefault(),
                AccountId = dto.AccountId, 
                Amount = dto.Amount,
                Percent = dto.Percent
            }).ToList();
        }
        public int BucketId { get; set; }
        public string Description { get; set; } = null!;
        public decimal Balance { get; set; }
        public decimal? GoalBalance { get; set; }
        public bool GoalAchieved { get; set; }
        public bool Positive { get; set; }
        public bool Archived { get; set; }

        public int SandboxId { get; set; }
        public Sandbox Sandbox { get; set; } = null!;

        public ICollection<AccountBucket> AccountBuckets { get; set; } = new List<AccountBucket>();
    }
}
