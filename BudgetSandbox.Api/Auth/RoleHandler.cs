using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace BudgetSandbox.Api.Auth
{
    public class RoleHandler : AuthorizationHandler<RoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            var realmAccess = context.User.Claims.FirstOrDefault(c => c.Type == "realm_access")?.Value;

            if(!string.IsNullOrWhiteSpace(realmAccess))
            {
                List<string>? roles = JsonSerializer.Deserialize<RealmAccessModel>(realmAccess)?.Roles;

                if(roles != null && roles.Any() && requirement.Roles.All(reqRole => roles.Any(role => role == reqRole)))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
