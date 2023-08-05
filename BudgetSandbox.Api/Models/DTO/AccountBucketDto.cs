using BudgetSandbox.Api.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace BudgetSandbox.Api.Models.DTO
{
    public class AccountBucketDto : IValidatableObject
    {
        public int? AccountBucketId { get; set; }
        public int AccountId { get; set; }
        [Range(0, int.MaxValue)]
        public decimal? Amount { get; set; }
        [Range(0, 1)]
        public decimal? Percent { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Amount == null && Percent == null)
            {
                yield return new ValidationResult(
                    $"Account bucket must have an amount or percent.",
                    new[] { nameof(Amount), nameof(Percent) });
            }

            if (Amount != null && Percent != null)
            {
                yield return new ValidationResult(
                    $"Account bucket must have an amount or percent. Not both.",
                    new[] { nameof(Amount), nameof(Percent) });
            }
        }
    }
}
