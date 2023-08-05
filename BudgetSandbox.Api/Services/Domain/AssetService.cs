using Microsoft.EntityFrameworkCore;
using BudgetSandbox.Api.Data;
using BudgetSandbox.Api.Models.Domain;
using BudgetSandbox.Api.Services.Data;

namespace BudgetSandbox.Api.Services.Domain
{
    public interface IAssetService
    {
        Task<List<Asset>> GetAllAsync(int sandboxId, bool tracking = false);
        Task<Asset?> GetAsync(int assetId, bool tracking = false);
        Task SaveAsync(Asset asset);
        Task DeleteAsync(Asset asset);
    }

    public class AssetService : IAssetService
    {
        private readonly IRepositoryService<Asset> repositoryService;

        public AssetService(IRepositoryService<Asset> repositoryService)
        {
            this.repositoryService = repositoryService;
        }

        public async Task DeleteAsync(Asset asset) => await repositoryService.DeleteAsync(asset);

        public async Task<List<Asset>> GetAllAsync(int sandboxId, bool tracking = false)
        {
            return await repositoryService.GetMultipleAsync(a => a.SandboxId == sandboxId, tracking);
        }

        public async Task<Asset?> GetAsync(int assetId, bool tracking = false)
        {
            return await repositoryService.GetAsync(a => a.AssetId == assetId, tracking);
        }

        public async Task SaveAsync(Asset asset) => await repositoryService.AddOrUpdateAsync(asset);
    }
}
