using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using BudgetSandbox.Api.Data;
using BudgetSandbox.IntegrationTests.Common;
using BudgetSandbox.Api.Models.Constants;
using BudgetSandbox.Api.Models.Domain;
using BudgetSandbox.Api.Services.Data;
using BudgetSandbox.Api.Services.Domain;
using System.Security.Principal;

namespace BudgetSandbox.IntegrationTests.Services.Domain
{
    [Collection("Database collection")]
    public class CashFlowItemServiceTests : IDisposable
    {
        DatabaseFixture databaseFixture;
        BudgetSandboxContext context;
        CashFlowItemService cashFlowItemService;

        public CashFlowItemServiceTests(DatabaseFixture databaseFixture)
        {
            this.databaseFixture = databaseFixture;
            context = this.databaseFixture.CreateContext();
            context.Database.BeginTransaction();

            RepositoryService<CashFlowItem> repositoryService = new(context);
            cashFlowItemService = new(repositoryService);
        }

        public void Dispose()
        {
            context.ChangeTracker.Clear();
            context.Dispose();
        }

        [Fact]
        public async Task Should_get_cash_flow_items_related_to_sandbox()
        {
            //Arrange
            var expectedCashFlowItems = await context.CashFlowItems
                .Where(cfi => cfi.SandboxId == databaseFixture.Sandbox.SandboxId)
                .Include(cfi => cfi.CashFlowItemAccounts)
                .Include(cfi => cfi.CashFlowItemBuckets)
                .ToListAsync();

            //Act
            List<CashFlowItem>? cashFlowItems = await cashFlowItemService.GetAllAsync(databaseFixture.Sandbox.SandboxId);

            //Assert
            Assert.NotNull(cashFlowItems);
            Assert.Equal(2, cashFlowItems.Count);
            cashFlowItems.Should().BeEquivalentTo(expectedCashFlowItems, options =>
                options.IgnoringCyclicReferences());
        }

        [Fact]
        public async Task Should_get_cash_flow_item()
        {
            //Arrange
            var expectedCashFlowItem = await context.CashFlowItems
                .Where(cfi => cfi.SandboxId == databaseFixture.Sandbox.SandboxId)
                .Include(cfi => cfi.CashFlowItemAccounts)
                .Include(cfi => cfi.CashFlowItemBuckets)
                .FirstOrDefaultAsync();

            //Act
            CashFlowItem? cashFlowItemResult = await cashFlowItemService.GetAsync(expectedCashFlowItem.CashFlowItemId);

            //Assert
            Assert.NotNull(cashFlowItemResult);
            cashFlowItemResult.Should().BeEquivalentTo(expectedCashFlowItem, options =>
                options.IgnoringCyclicReferences());
        }

        [Fact]
        public async Task Should_create_cash_flow_item()
        {
            //Arrange
            var account = await context.Accounts
                .Where(cfi => cfi.SandboxId == databaseFixture.Sandbox.SandboxId)
                .FirstOrDefaultAsync();


            var bucket = await context.Buckets
                .Where(cfi => cfi.SandboxId == databaseFixture.Sandbox.SandboxId)
                .FirstOrDefaultAsync();

            CashFlowItem cashFlowItem = CreateCashFlowItem(account, bucket);

            //Act
            await cashFlowItemService.SaveAsync(cashFlowItem);

            //Assert
            var cashFlowItemResult = await context.CashFlowItems
                .Where(a => a.CashFlowItemId == cashFlowItem.CashFlowItemId)
                .Include(a => a.CashFlowItemAccounts)
                .Include(a => a.CashFlowItemBuckets)
                .FirstOrDefaultAsync();

            Assert.NotNull(cashFlowItemResult);
            cashFlowItemResult.Should().BeEquivalentTo(cashFlowItem, options => options.IgnoringCyclicReferences());
        }

        [Fact]
        public async Task Should_update_cash_flow_item()
        {
            //Arrange
            var account = await context.Accounts
                .Where(cfi => cfi.SandboxId == databaseFixture.Sandbox.SandboxId)
                .FirstOrDefaultAsync();


            var bucket = await context.Buckets
                .Where(cfi => cfi.SandboxId == databaseFixture.Sandbox.SandboxId)
                .FirstOrDefaultAsync();

            CashFlowItem cashFlowItem = CreateCashFlowItem(account, bucket);

            await cashFlowItemService.SaveAsync(cashFlowItem);

            cashFlowItem.Description = "New Description";
            cashFlowItem.CashFlowItemAccounts.First().Percent = 1;
            cashFlowItem.CashFlowItemBuckets = new List<CashFlowItemBucket>();

            //Act
            await cashFlowItemService.SaveAsync(cashFlowItem);

            //Assert
            var cashFlowItemResult = await context.CashFlowItems
                .Where(a => a.CashFlowItemId == cashFlowItem.CashFlowItemId)
                .Include(a => a.CashFlowItemAccounts)
                .Include(a => a.CashFlowItemBuckets)
                .FirstOrDefaultAsync();

            Assert.NotNull(cashFlowItemResult);
            Assert.Empty(cashFlowItemResult.CashFlowItemBuckets);
            Assert.Single(cashFlowItemResult.CashFlowItemAccounts);
            cashFlowItemResult.Should().BeEquivalentTo(cashFlowItem, options => options.IgnoringCyclicReferences());
        }

        [Fact]
        public async Task Should_delete_cash_flow_item()
        {
            //Arrange
            var account = await context.Accounts
                .Where(cfi => cfi.SandboxId == databaseFixture.Sandbox.SandboxId)
                .FirstOrDefaultAsync();

            var bucket = await context.Buckets
                .Where(cfi => cfi.SandboxId == databaseFixture.Sandbox.SandboxId)
                .FirstOrDefaultAsync();

            CashFlowItem cashFlowItem = CreateCashFlowItem(account, bucket);

            await cashFlowItemService.SaveAsync(cashFlowItem);

            //Act
            await cashFlowItemService.DeleteAsync(cashFlowItem);

            //Assert
            var cashFlowItemResult = await context.CashFlowItems
                .Where(a => a.CashFlowItemId == cashFlowItem.CashFlowItemId)
                .Include(a => a.CashFlowItemAccounts)
                .Include(a => a.CashFlowItemBuckets)
                .FirstOrDefaultAsync();

            Assert.Null(cashFlowItemResult);
        }

        private CashFlowItem CreateCashFlowItem(Account account, Bucket bucket)
        {
            return new CashFlowItem
            {
                SandboxId = databaseFixture.Sandbox.SandboxId,
                Description = "Rent Payment",
                Amount = 1_525.50m,
                Frequency = CashFlowFrequency.Monthly,
                CashFlowItemAccounts = new List<CashFlowItemAccount>
                {
                    new CashFlowItemAccount
                    {
                        AccountId = account.AccountId,
                        Percent = 0.5m
                    }
                },
                CashFlowItemBuckets = new List<CashFlowItemBucket>
                {
                    new CashFlowItemBucket
                    {
                        BucketId = bucket.BucketId,
                        Percent = 0.5m
                    }
                }
            };
        }
    }
}
