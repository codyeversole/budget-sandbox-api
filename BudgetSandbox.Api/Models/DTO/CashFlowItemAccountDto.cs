using System.ComponentModel.DataAnnotations;

namespace BudgetSandbox.Api.Models.DTO
{
    public class CashFlowItemAccountDto : IValidatableObject
    {
        public int? CashFlowItemAccountId { get; set; }
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
                    $"Cash flow item account must have an amount or percent.",
                    new[] { nameof(Amount), nameof(Percent) });
            }

            if (Amount != null && Percent != null)
            {
                yield return new ValidationResult(
                    $"Cash flow item account must have an amount or percent. Not both.",
                    new[] { nameof(Amount), nameof(Percent) });
            }
        }
    }
}
