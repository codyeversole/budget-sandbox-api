using Microsoft.EntityFrameworkCore;
using BudgetSandbox.Api.Data;
using BudgetSandbox.Api.Models.Domain;
using BudgetSandbox.Api.Services.Data;

namespace BudgetSandbox.Api.Services.Domain
{
    public interface IBucketService
    {
        Task<List<Bucket>> GetAllAsync(int sandboxId, bool tracking = false);
        Task<Bucket?> GetAsync(int bucketId, bool tracking = false);
        Task SaveAsync(Bucket bucket);
        Task DeleteAsync(Bucket bucket);
    }

    public class BucketService : IBucketService
    {
        private readonly IRepositoryService<Bucket> repositoryService;

        public BucketService(IRepositoryService<Bucket> repositoryService)
        {
            this.repositoryService = repositoryService;
        }

        public async Task DeleteAsync(Bucket bucket) => await repositoryService.DeleteAsync(bucket);

        public async Task<List<Bucket>> GetAllAsync(int sandboxId, bool tracking = false)
        {
            return await repositoryService.GetMultipleAsync(a => a.SandboxId == sandboxId, tracking, "AccountBuckets");
        }

        public async Task<Bucket?> GetAsync(int bucketId, bool tracking = false)
        {
            return await repositoryService.GetAsync(a => a.BucketId == bucketId, tracking, "AccountBuckets");
        }

        public async Task SaveAsync(Bucket bucket)
        {
            await repositoryService.AddOrUpdateAsync(bucket);

            List<int> accountBucketIds = bucket.AccountBuckets.Select(a => a.AccountBucketId).ToList();
            var existing = await GetAsync(bucket.BucketId);

            var removedAccounts = existing.AccountBuckets.Where(ab => accountBucketIds.Contains(ab.AccountBucketId) == false).ToList();
            if (removedAccounts.Any())
            {
                foreach (var account in removedAccounts)
                {
                    existing.AccountBuckets.Remove(account);
                }
            }

            await repositoryService.AddOrUpdateAsync(bucket);
        }
    }
}
