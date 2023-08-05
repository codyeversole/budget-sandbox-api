using BudgetSandbox.Api.Models.Domain;
using BudgetSandbox.Api.Services.Data;
using BudgetSandbox.Api.Services.Domain;

namespace BudgetSandbox.Api.Services
{
    public static class BudgetSandboxApiServiceCollection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryService<UserSandbox>, RepositoryService<UserSandbox>>();
            services.AddScoped<IRepositoryService<Sandbox>, RepositoryService<Sandbox>>();
            services.AddScoped<IRepositoryService<Bucket>, RepositoryService<Bucket>>();
            services.AddScoped<IRepositoryService<Account>, RepositoryService<Account>>();
            services.AddScoped<IRepositoryService<Asset>, RepositoryService<Asset>>();
            services.AddScoped<IRepositoryService<CashFlowItem>, RepositoryService<CashFlowItem>>();
            services.AddScoped<ISandboxService, SandboxService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IBucketService, BucketService>();
            services.AddScoped<IAssetService, AssetService>();
            services.AddScoped<ICashFlowItemService, CashFlowItemService>();
            services.AddScoped<IReportService, ReportService>();

            return services;
        }
    }
}
