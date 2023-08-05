using Microsoft.EntityFrameworkCore;
using BudgetSandbox.Api.Models.Domain;

namespace BudgetSandbox.Api.Data
{
    public class BudgetSandboxContext : DbContext
    {
        public BudgetSandboxContext(DbContextOptions<BudgetSandboxContext> options) : base(options)
        {
        }

        public DbSet<UserSandbox> UserSandboxs { get; set; }
        public DbSet<Sandbox> Sandboxs { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountBucket> AccountBuckets { get; set; }
        public DbSet<Bucket> Buckets { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<CashFlowItem> CashFlowItems { get; set; }
        public DbSet<CashFlowItemAccount> CashFlowItemAccounts { get; set; }
        public DbSet<CashFlowItemBucket> CashFlowItemBuckets { get; set; }
    }
}
