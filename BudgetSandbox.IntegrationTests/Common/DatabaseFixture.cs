using Microsoft.EntityFrameworkCore;
using BudgetSandbox.Api.Data;
using BudgetSandbox.Api.Models.Constants;
using BudgetSandbox.Api.Models.Domain;

namespace BudgetSandbox.IntegrationTests.Common
{
    public class DatabaseFixture : IDisposable
    {
        private const string ConnectionString = "Host=localhost;Port=4432;Database=budget_sandbox_db;Username=postgres;Password=docker";

        public readonly Sandbox Sandbox;
        public string UserId { get; } = "randomuserid";

        public DatabaseFixture()
        {
            Sandbox = SeedDatabase(UserId);

            /*
             * add misc data of other users to make sure only the data of the current user ('randomuserid')
             * shows up in the integration tests 
            */
            _ = SeedDatabase("otheruserid1");
            _ = SeedDatabase("otheruserid2");
        }

        public void Dispose()
        {
            using (var context = CreateContext())
            {
                context.UserSandboxs.RemoveRange(context.UserSandboxs);
                context.Sandboxs.RemoveRange(context.Sandboxs);
                context.SaveChanges();
            }
        }

        public BudgetSandboxContext CreateContext()
            => new BudgetSandboxContext(new DbContextOptionsBuilder<BudgetSandboxContext>().UseNpgsql(ConnectionString).UseSnakeCaseNamingConvention().Options);

        private Sandbox SeedDatabase(string userId)
        {
            Sandbox sandbox = new Sandbox { Description = "My Sandbox" };
            List<Account> accounts;
            List<Bucket> buckets;
            List<Asset> assets;
            List<CashFlowItem> cashFlowItems;

            using (var context = CreateContext())
            {
                context.UserSandboxs.Add(new UserSandbox { UserId = userId, Sandbox = sandbox });
             
                accounts = new List<Account>
                {
                    new Account
                    {
                        Sandbox = sandbox,
                        Description = "Checking Account",
                        Balance = 1_548.38m,
                        Positive = true
                    },
                    new Account
                    {
                        Sandbox = sandbox,
                        Description = "Savings Account",
                        Balance = 11_548.38m,
                        Positive = true
                    }
                };
                context.Accounts.AddRange(accounts);
            
                buckets = new List<Bucket>
                {
                    new Bucket
                    {
                        Sandbox = sandbox,
                        Description = "Bucket 1",
                        Balance = 100m,
                        GoalBalance = 1_000m
                    },
                    new Bucket
                    {
                        Sandbox = sandbox,
                        Description = "Bucket 2",
                        Balance = 1_938.80m
                    },
                };
                context.Buckets.AddRange(buckets);

                assets = new List<Asset>
                {
                    new Asset
                    {
                        Sandbox = sandbox,
                        Description = "Asset 1",
                        AmountValue = 100m
                    },
                    new Asset
                    {
                        Sandbox = sandbox,
                        Description = "Asset 2",
                        AmountValue = 200m
                    }
                };
                context.Assets.AddRange(assets);

                cashFlowItems = new List<CashFlowItem>
                {
                    new CashFlowItem
                    {
                        Sandbox = sandbox,
                        Description = "Job",
                        Amount = 2_000m,
                        Frequency = CashFlowFrequency.BiWeekly,
                    },
                    new CashFlowItem
                    {
                        Sandbox = sandbox,
                        Description = "Rent",
                        Amount = 900m,
                        Frequency = CashFlowFrequency.Monthly
                    }
                };
                context.CashFlowItems.AddRange(cashFlowItems);

                var accountBuckets = new List<AccountBucket>
                {
                    new AccountBucket
                    {
                        Account = accounts[0],
                        Bucket = buckets[0]
                    },
                    new AccountBucket
                    {
                        Account = accounts[0],
                        Bucket = buckets[1]
                    }
                };
                context.AccountBuckets.AddRange(accountBuckets);

                var cashFlowItemAccounts = new List<CashFlowItemAccount>
                {
                    new CashFlowItemAccount
                    {
                        Account = accounts[0],
                        CashFlowItem = cashFlowItems[0]
                    },
                    new CashFlowItemAccount
                    {
                        Account = accounts[0],
                        CashFlowItem = cashFlowItems[1]
                    },
                };
                context.CashFlowItemAccounts.AddRange(cashFlowItemAccounts);

                var cashFlowItemBuckets = new List<CashFlowItemBucket>
                {
                    new CashFlowItemBucket
                    {
                        Bucket = buckets[0],
                        CashFlowItem = cashFlowItems[0]
                    },
                    new CashFlowItemBucket
                    {
                        Bucket = buckets[0],
                        CashFlowItem = cashFlowItems[1]
                    },
                };
                context.CashFlowItemBuckets.AddRange(cashFlowItemBuckets);

                context.SaveChanges();
            }

            return sandbox;
        }

    }
}
