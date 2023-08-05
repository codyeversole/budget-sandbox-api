using Microsoft.EntityFrameworkCore;
using BudgetSandbox.Api.Data;
using BudgetSandbox.Api.Models.Domain;
using BudgetSandbox.Api.Services.Data;

namespace BudgetSandbox.Api.Services.Domain
{
    public interface ISandboxService
    {
        Task<bool> HasAccessAsync(string userId, int sandboxId);
        Task<List<Sandbox>> GetAsync(string userId, bool tracking = false);
        Task SaveAsync(Sandbox sandbox, string userId);
        Task DeleteAsync(int sandboxId);
    }

    public class SandboxService : ISandboxService
    {
        private readonly IRepositoryService<Sandbox> sandboxRepository;
        private readonly IRepositoryService<UserSandbox> userSandboxRepository;

        public SandboxService(IRepositoryService<Sandbox> sandboxRepository, IRepositoryService<UserSandbox> userSandboxRepository)
        {
            this.sandboxRepository = sandboxRepository;
            this.userSandboxRepository = userSandboxRepository;
        }

        public async Task<bool> HasAccessAsync(string userId, int sandboxId)
        {
            return await userSandboxRepository.ExistsAsync(ub => ub.UserId == userId && ub.SandboxId == sandboxId);
        }

        public async Task<List<Sandbox>> GetAsync(string userId, bool tracking = false)
        {
            List<UserSandbox> userSandboxs = await userSandboxRepository.GetMultipleAsync(ub => ub.UserId == userId, tracking, "Sandbox");
            return userSandboxs.Select(ub => ub.Sandbox).ToList();
        }

        public async Task SaveAsync(Sandbox sandbox, string userId)
        {
            if (sandbox.SandboxId == 0)
            {
                await userSandboxRepository.AddOrUpdateAsync(new UserSandbox { UserId = userId, Sandbox = sandbox });
            }
            else
            {
                await sandboxRepository.AddOrUpdateAsync(sandbox);
            }
        }

        public async Task DeleteAsync(int sandboxId)
        {
            Sandbox? sandbox = await sandboxRepository.GetAsync(b => b.SandboxId == sandboxId);
            if (sandbox != null)
            {
                await sandboxRepository.DeleteAsync(sandbox);
            }
        }
    }
}
