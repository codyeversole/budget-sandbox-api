using BudgetSandbox.Api.Models.DTO;
using System.ComponentModel.DataAnnotations;

namespace BudgetSandbox.Api.Models.Domain
{
    public class CashFlowItem
    {
        public CashFlowItem() { }
        public CashFlowItem(CashFlowItemDto cashFlowItemDto)
        {
            CashFlowItemId = cashFlowItemDto.CashFlowItemId.GetValueOrDefault();
            Description = cashFlowItemDto.Description;
            Amount = cashFlowItemDto.Amount;
            Frequency = cashFlowItemDto.Frequency;
            Positive = cashFlowItemDto.Positive;
            SandboxId = cashFlowItemDto.SandboxId;
            AssetId = cashFlowItemDto.AssetId;
            CashFlowItemAccounts = cashFlowItemDto.CashFlowItemAccounts.Select(dto => new CashFlowItemAccount
            {
                CashFlowItemAccountId = dto.CashFlowItemAccountId.GetValueOrDefault(),
                AccountId = dto.AccountId,
                Amount = dto.Amount,
                Percent = dto.Percent
            }).ToList();
            CashFlowItemBuckets = cashFlowItemDto.CashFlowItemBuckets.Select(dto => new CashFlowItemBucket
            {
                CashFlowItemBucketId = dto.CashFlowItemBucketId.GetValueOrDefault(),
                BucketId = dto.BucketId,
                Amount = dto.Amount,
                Percent = dto.Percent
            }).ToList();
        }

        public int CashFlowItemId { get; set; }
        public string Description { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Frequency { get; set; } = null!;
        public bool Positive { get; set; }

        public int SandboxId { get; set; }
        public Sandbox Sandbox { get; set; } = null!;

        /// <summary>
        /// The source of this cash flow item
        /// </summary>
        public int? AssetId { get; set; }
        public Asset? Asset { get; set; }

        public ICollection<CashFlowItemAccount> CashFlowItemAccounts { get; set; } = new List<CashFlowItemAccount>();
        public ICollection<CashFlowItemBucket> CashFlowItemBuckets { get; set; } = new List<CashFlowItemBucket>();
    }
}
