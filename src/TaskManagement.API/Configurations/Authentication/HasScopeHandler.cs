using Microsoft.AspNetCore.Authorization;

namespace TaskManagement.API.Configurations.Authentication
{
    public sealed class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            HasScopeRequirement requirement)
        {
            var scopeClaim = context.User.FindFirst("scope");
            if (scopeClaim == null)
            {
                return Task.CompletedTask;
            }

            var scopes = scopeClaim.Value.Split(',');

            if (scopes.Any(s => s == requirement.Scope))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
