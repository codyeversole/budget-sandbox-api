using BudgetSandbox.Api.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace BudgetSandbox.Api.Models.DTO
{
    public class BucketDto : IValidatableObject
    {
        public int? BucketId { get; set; }
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public decimal Balance { get; set; }
        public decimal? GoalBalance { get; set; }
        [Required]
        public bool GoalAchieved { get; set; }

        [Required]
        public int SandboxId { get; set; }
        [Required]
        public bool Positive { get; set; }
        [Required]
        public bool Archived { get; set; }

        public List<AccountBucketDto> AccountBuckets { get; set; } = new List<AccountBucketDto>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            decimal total = 0;

            foreach (var account in AccountBuckets)
            {
                if (account.Amount != null)
                {
                    total += account.Amount.GetValueOrDefault();
                }
                else
                {
                    total += (account.Percent * Balance).GetValueOrDefault();
                }
            }

            if (total != Balance)
            {
                yield return new ValidationResult(
                    $"Bucket must have account buckets that equal balance.",
                    new[] { nameof(AccountBuckets) });
            }
        }
    }
}
