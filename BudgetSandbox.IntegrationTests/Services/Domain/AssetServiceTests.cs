using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using BudgetSandbox.Api.Data;
using BudgetSandbox.IntegrationTests.Common;
using BudgetSandbox.Api.Models.Domain;
using BudgetSandbox.Api.Services.Data;
using BudgetSandbox.Api.Services.Domain;
using System.Security.Principal;

namespace BudgetSandbox.IntegrationTests.Services.Domain
{
    [Collection("Database collection")]
    public class AssetServiceTests : IDisposable
    {
        DatabaseFixture databaseFixture;
        BudgetSandboxContext context;
        AssetService assetService;

        public AssetServiceTests(DatabaseFixture databaseFixture)
        {
            this.databaseFixture = databaseFixture;
            context = this.databaseFixture.CreateContext();
            context.Database.BeginTransaction();

            RepositoryService<Asset> repositoryService = new(context);
            assetService = new(repositoryService);
        }

        public void Dispose()
        {
            context.ChangeTracker.Clear();
            context.Dispose();
        }

        [Fact]
        public async Task Should_get_assets_related_to_adget()
        {
            //Arrange
            var expectedAssets = await context.Assets
                .Where(a => a.SandboxId == databaseFixture.Sandbox.SandboxId)
                .ToListAsync();

            //Act
            List<Asset>? assets = await assetService.GetAllAsync(databaseFixture.Sandbox.SandboxId);

            //Assert
            Assert.NotNull(assets);
            Assert.Equal(2, assets.Count);
            assets.Should().BeEquivalentTo(expectedAssets, options =>
                options.IgnoringCyclicReferences());
        }

        [Fact]
        public async Task Should_get_asset()
        {
            //Arrange
            var expectedAsset = await context.Assets
                .Where(a => a.SandboxId == databaseFixture.Sandbox.SandboxId)
                .FirstOrDefaultAsync();

            //Act
            Asset? assetResult = await assetService.GetAsync(expectedAsset.AssetId);

            //Assert
            Assert.NotNull(assetResult);
            assetResult.Should().BeEquivalentTo(expectedAsset, options =>
                options.IgnoringCyclicReferences());
        }

        [Fact]
        public async Task Should_create_asset()
        {
            //Arrange
            Asset asset = CreateAsset();

            //Act
            await assetService.SaveAsync(asset);

            //Assert
            var assetResult = await context.Assets
                .Where(a => a.AssetId == asset.AssetId)
                .FirstOrDefaultAsync();

            Assert.NotNull(assetResult);
            assetResult.Should().BeEquivalentTo(asset, options => options.IgnoringCyclicReferences());
        }

        [Fact]
        public async Task Should_update_asset()
        {
            //Arrange
            Asset asset = CreateAsset();

            await assetService.SaveAsync(asset);

            asset.Description = "New Description";

            //Act
            await assetService.SaveAsync(asset);

            //Assert
            var assetResult = await context.Assets
                .Where(a => a.AssetId == asset.AssetId)
                .FirstOrDefaultAsync();

            Assert.NotNull(assetResult);
            assetResult.Should().BeEquivalentTo(asset, options => options.IgnoringCyclicReferences());
        }

        [Fact]
        public async Task Should_delete_asset()
        {
            //Arrange
            Asset asset = CreateAsset();

            await assetService.SaveAsync(asset);

            //Act
            await assetService.DeleteAsync(asset);

            //Assert
            var assetResult = await context.Assets
                .Where(a => a.AssetId == asset.AssetId)
                .FirstOrDefaultAsync();

            Assert.Null(assetResult);
        }

        private Asset CreateAsset()
        {
            return new Asset
            {
                SandboxId = databaseFixture.Sandbox.SandboxId,
                Description = "Rental Property",
                AmountValue = 200_000
            };
        }
    }
}
