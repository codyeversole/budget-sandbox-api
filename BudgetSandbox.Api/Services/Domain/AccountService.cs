using BudgetSandbox.Api.Models.Domain;
using BudgetSandbox.Api.Services.Data;

namespace BudgetSandbox.Api.Services.Domain
{
    public interface IAccountService
    {
        Task<List<Account>> GetAllAsync(int sandboxId, bool tracking = false);
        Task<Account?> GetAsync(int accountId, bool tracking = false);
        Task SaveAsync(Account account);
        Task DeleteAsync(Account account);
    }

    public class AccountService : IAccountService
    {
        private readonly IRepositoryService<Account> repositoryService;

        public AccountService(IRepositoryService<Account> repositoryService)
        {
            this.repositoryService = repositoryService;
        }

        public async Task DeleteAsync(Account account) => await repositoryService.DeleteAsync(account);

        public async Task<List<Account>> GetAllAsync(int sandboxId, bool tracking = false)
        {
            return await repositoryService.GetMultipleAsync(a => a.SandboxId == sandboxId, tracking, "AccountBuckets");
        }

        public async Task<Account?> GetAsync(int accountId, bool tracking = false)
        {
            return await repositoryService.GetAsync(a => a.AccountId == accountId, tracking, "AccountBuckets");
        }

        public async Task SaveAsync(Account account) => await repositoryService.AddOrUpdateAsync(account);
    }
}
