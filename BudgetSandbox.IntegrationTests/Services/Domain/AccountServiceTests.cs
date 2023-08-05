using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using BudgetSandbox.Api.Data;
using BudgetSandbox.Api.Models.Domain;
using BudgetSandbox.IntegrationTests.Common;
using BudgetSandbox.Api.Models.DTO;
using BudgetSandbox.Api.Services.Data;
using BudgetSandbox.Api.Services.Domain;

namespace BudgetSandbox.IntegrationTests.Services.Domain
{
    [Collection("Database collection")]
    public class AccountServiceTests : IDisposable
    {
        DatabaseFixture databaseFixture;
        BudgetSandboxContext context;
        AccountService accountService;

        public AccountServiceTests(DatabaseFixture databaseFixture)
        {
            this.databaseFixture = databaseFixture;
            context = this.databaseFixture.CreateContext();
            context.Database.BeginTransaction();

            RepositoryService<Account> repositoryService = new(context);
            accountService = new(repositoryService);
        }

        public void Dispose()
        {
            context.ChangeTracker.Clear();
            context.Dispose();
        }

        [Fact]
        public async Task Should_get_accounts_related_to_sandbox()
        {
            //Arrange
            var expectedAccount = await context.Accounts
                .Where(a => a.SandboxId == databaseFixture.Sandbox.SandboxId)
                .Include(a => a.AccountBuckets)
                .ToListAsync();

            //Act
            List<Account>? accounts = await accountService.GetAllAsync(databaseFixture.Sandbox.SandboxId);

            //Assert
            Assert.NotNull(accounts);
            Assert.Equal(2, accounts.Count());
            accounts.Should().BeEquivalentTo(expectedAccount, options =>
                options.IgnoringCyclicReferences());
        }

        [Fact]
        public async Task Should_get_account()
        {
            //Arrange
            var expectedAccount = await context.Accounts
                .Where(a => a.SandboxId == databaseFixture.Sandbox.SandboxId)
                .Include(a => a.AccountBuckets)
                .FirstOrDefaultAsync();

            //Act
            Account? accountResult = await accountService.GetAsync(expectedAccount.AccountId);

            //Assert
            Assert.NotNull(accountResult);
            accountResult.Should().BeEquivalentTo(expectedAccount, options =>
                options.IgnoringCyclicReferences());
        }

        [Fact]
        public async Task Should_create_account()
        {
            //Arrange
            Account account = CreateAccount();

            //Act
            await accountService.SaveAsync(account);

            //Assert
            var accountResult = await context.Accounts
                .Where(a => a.AccountId == account.AccountId)
                .Include(a => a.AccountBuckets)
                .FirstOrDefaultAsync();

            Assert.NotNull(accountResult);
            accountResult.Should().BeEquivalentTo(account, options =>
                options.IgnoringCyclicReferences());
        }

        [Fact]
        public async Task Should_update_account()
        {
            //Arrange
            Account account = CreateAccount();
            await accountService.SaveAsync(account);

            account.Description = "new description";
            account.Balance = 10_000.99m;

            //Act
            await accountService.SaveAsync(account);

            //Assert
            var accountResult = await context.Accounts
                .Where(a => a.AccountId == account.AccountId)
                .Include(a => a.AccountBuckets)
                .FirstOrDefaultAsync();

            Assert.NotNull(accountResult);
            accountResult.Should().BeEquivalentTo(account, options => options.IgnoringCyclicReferences());
        }

        [Fact]
        public async Task Should_delete_account_and_account_buckets()
        {
            //Arrange
            var account = CreateAccount();
            context.Accounts.Add(account);
            context.SaveChanges();

            //Act
            await accountService.DeleteAsync(account);

            //Assert
            var accountResult = await context.Accounts
                .Where(a => a.AccountId == account.AccountId)
                .Include(a => a.AccountBuckets)
                .FirstOrDefaultAsync();

            Assert.Null(accountResult);
        }

        private Account CreateAccount()
        {
            return new Account(new AccountDto { Description = "Checking Account", Balance = 1_548.38m, Positive = true, SandboxId = databaseFixture.Sandbox.SandboxId });
        }

    }
}
