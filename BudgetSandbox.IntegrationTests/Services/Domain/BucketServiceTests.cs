using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using BudgetSandbox.Api.Data;
using BudgetSandbox.IntegrationTests.Common;
using BudgetSandbox.Api.Models.Domain;
using BudgetSandbox.Api.Services.Data;
using BudgetSandbox.Api.Services.Domain;

namespace BudgetSandbox.IntegrationTests.Services.Domain
{
    [Collection("Database collection")]
    public class BucketServiceTests : IDisposable
    {
        DatabaseFixture databaseFixture;
        BudgetSandboxContext context;
        BucketService bucketService;

        public BucketServiceTests(DatabaseFixture databaseFixture)
        {
            this.databaseFixture = databaseFixture;
            context = this.databaseFixture.CreateContext();
            context.Database.BeginTransaction();

            RepositoryService<Bucket> repositoryService = new(context);
            bucketService = new(repositoryService);
        }

        public void Dispose()
        {
            context.ChangeTracker.Clear();
            context.Dispose();
        }

        [Fact]
        public async Task Should_get_buckets_related_to_sandbox()
        {
            //Arrange
            var expectedBuckets = await context.Buckets
                .Where(bu => bu.SandboxId == databaseFixture.Sandbox.SandboxId)
                .Include(bu => bu.AccountBuckets)
                .ToListAsync();

            //Act
            List<Bucket>? buckets = await bucketService.GetAllAsync(databaseFixture.Sandbox.SandboxId);

            //Assert
            Assert.NotNull(buckets);
            Assert.Equal(2, buckets.Count);
            buckets.Should().BeEquivalentTo(expectedBuckets, options =>
                options.IgnoringCyclicReferences());
        }

        [Fact]
        public async Task Should_get_bucket()
        {
            //Arrange
            var expectedBucket = await context.Buckets
                .Where(bu => bu.SandboxId == databaseFixture.Sandbox.SandboxId)
                .Include(bu => bu.AccountBuckets)
                .FirstOrDefaultAsync();

            //Act
            Bucket? bucketResult = await bucketService.GetAsync(expectedBucket.BucketId);

            //Assert
            Assert.NotNull(bucketResult);
            bucketResult.Should().BeEquivalentTo(expectedBucket, options =>
                options.IgnoringCyclicReferences());
        }

        [Fact]
        public async Task Should_create_bucket()
        {
            //Arrange
            var account = await context.Accounts
                .Where(cfi => cfi.SandboxId == databaseFixture.Sandbox.SandboxId)
                .FirstOrDefaultAsync();

            Bucket bucket = CreateBucket(account);

            //Act
            await bucketService.SaveAsync(bucket);

            //Assert
            var bucketResult = await context.Buckets
                .Where(bu => bu.BucketId == bucket.BucketId)
                .Include(bu => bu.AccountBuckets)
                .FirstOrDefaultAsync();

            Assert.NotNull(bucketResult);
            bucketResult.Should().BeEquivalentTo(bucket, options => options.IgnoringCyclicReferences());
        }

        [Fact]
        public async Task Should_update_bucket()
        {
            //Arrange
            var account = await context.Accounts
                .Where(cfi => cfi.SandboxId == databaseFixture.Sandbox.SandboxId)
                .FirstOrDefaultAsync();

            Bucket bucket = CreateBucket(account);

            await bucketService.SaveAsync(bucket);

            bucket.Description = "New Description";
            bucket.AccountBuckets = new List<AccountBucket>();

            //Act
            await bucketService.SaveAsync(bucket);

            //Assert
            var bucketResult = await context.Buckets
                .Where(bu => bu.BucketId == bucket.BucketId)
                .Include(bu => bu.AccountBuckets)
                .FirstOrDefaultAsync();

            Assert.NotNull(bucketResult);
            Assert.Empty(bucketResult.AccountBuckets);
            bucketResult.Should().BeEquivalentTo(bucket, options => options.IgnoringCyclicReferences());
        }

        [Fact]
        public async Task Should_delete_bucket()
        {
            //Arrange
            var account = await context.Accounts
                .Where(cfi => cfi.SandboxId == databaseFixture.Sandbox.SandboxId)
                .FirstOrDefaultAsync();

            Bucket bucket = CreateBucket(account);

            await bucketService.SaveAsync(bucket);

            //Act
            await bucketService.DeleteAsync(bucket);

            //Assert
            var bucketResult = await context.Buckets
                .Where(bu => bu.BucketId == bucket.BucketId)
                .Include(bu => bu.AccountBuckets)
                .FirstOrDefaultAsync();

            Assert.Null(bucketResult);
        }

        private Bucket CreateBucket(Account account)
        {
            return new Bucket
            {
                SandboxId = databaseFixture.Sandbox.SandboxId,
                Description = "Bucket 1",
                Balance = 200m,
                GoalBalance = 1000m,
                GoalAchieved = false,
                AccountBuckets = new List<AccountBucket>
                {
                    new AccountBucket
                    {
                        AccountId = account.AccountId,
                        Percent = 1m
                    }
                }
            };
        }
    }
}
