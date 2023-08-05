using Microsoft.AspNetCore.Authorization;

namespace BudgetSandbox.Api.Auth
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public RoleRequirement(List<string> roles)
        {
            Roles = roles;
        }

        public List<string> Roles { get; set; }
    }
}
