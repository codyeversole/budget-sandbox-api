using Microsoft.EntityFrameworkCore;
using BudgetSandbox.Api.Data;
using BudgetSandbox.Api.Models.Domain;
using BudgetSandbox.Api.Services.Data;
using System.Security.Principal;

namespace BudgetSandbox.Api.Services.Domain
{
    public interface ICashFlowItemService
    {
        Task<List<CashFlowItem>> GetAllAsync(int sandboxId, bool tracking = false);
        Task<CashFlowItem?> GetAsync(int cashFlowItemId, bool tracking = false);
        Task SaveAsync(CashFlowItem cashFlowItem);
        Task DeleteAsync(CashFlowItem cashFlowItem);
    }

    public class CashFlowItemService : ICashFlowItemService
    {
        private readonly IRepositoryService<CashFlowItem> repositoryService;

        public CashFlowItemService(IRepositoryService<CashFlowItem> repositoryService)
        {
            this.repositoryService = repositoryService;
        }

        public async Task DeleteAsync(CashFlowItem cashFlowItem) => await repositoryService.DeleteAsync(cashFlowItem);

        public async Task<List<CashFlowItem>> GetAllAsync(int sandboxId, bool tracking = false)
        {
            return await repositoryService.GetMultipleAsync(a => a.SandboxId == sandboxId, tracking, "CashFlowItemAccounts", "CashFlowItemBuckets");
        }

        public async Task<CashFlowItem?> GetAsync(int cashFlowItemId, bool tracking = false)
        {
            return await repositoryService.GetAsync(a => a.CashFlowItemId == cashFlowItemId, tracking, "CashFlowItemAccounts", "CashFlowItemBuckets");
        }

        public async Task SaveAsync(CashFlowItem cashFlowItem)
        {
            await repositoryService.AddOrUpdateAsync(cashFlowItem);

            List<int> cashFlowItemAccountIds = cashFlowItem.CashFlowItemAccounts.Select(a => a.CashFlowItemAccountId).ToList();
            List<int> cashFlowItemBucketIds = cashFlowItem.CashFlowItemBuckets.Select(a => a.CashFlowItemBucketId).ToList();
            var existing = await GetAsync(cashFlowItem.CashFlowItemId, true);

            var removedAccounts = existing.CashFlowItemAccounts.Where(a => cashFlowItemAccountIds.Contains(a.CashFlowItemAccountId) == false).ToList();
            if (removedAccounts.Any())
            {
                foreach (var account in removedAccounts)
                {
                    existing.CashFlowItemAccounts.Remove(account);
                }
            }

            var removedBuckets = existing.CashFlowItemBuckets.Where(a => cashFlowItemBucketIds.Contains(a.CashFlowItemBucketId) == false).ToList();
            if (removedBuckets.Any())
            {
                foreach (var bucket in removedBuckets)
                {
                    existing.CashFlowItemBuckets.Remove(bucket);
                }
            }

            await repositoryService.AddOrUpdateAsync(cashFlowItem);
        }
    }
}
