using System.ComponentModel.DataAnnotations;

namespace BudgetSandbox.Api.Models.DTO
{
    public class CashFlowItemDto : IValidatableObject
    {
        public int? CashFlowItemId { get; set; }
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        [Range(0, int.MaxValue)]
        public decimal Amount { get; set; }
        [Required]
        public string Frequency { get; set; } = null!;
        [Required]
        public bool Positive { get; set; }

        public int SandboxId { get; set; }

        /// <summary>
        /// The source of this cash flow item
        /// </summary>
        public int? AssetId { get; set; }

        public List<CashFlowItemAccountDto> CashFlowItemAccounts { get; set; } = new List<CashFlowItemAccountDto>();
        public List<CashFlowItemBucketDto> CashFlowItemBuckets { get; set; } = new List<CashFlowItemBucketDto>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            decimal total = 0;

            foreach (var account in CashFlowItemAccounts)
            {
                if (account.Amount != null)
                {
                    total += account.Amount.GetValueOrDefault();
                }
                else
                {
                    total += (account.Percent * Amount).GetValueOrDefault();
                }
            }

            foreach (var account in CashFlowItemBuckets)
            {
                if (account.Amount != null)
                {
                    total += account.Amount.GetValueOrDefault();
                }
                else
                {
                    total += (account.Percent * Amount).GetValueOrDefault();
                }
            }

            if (total != Amount)
            {
                yield return new ValidationResult(
                    $"Cash flow item must have accounts and/or buckets that equal amount.",
                    new[] { nameof(CashFlowItemAccounts), nameof(CashFlowItemBuckets) });
            }
        }
    }
}
