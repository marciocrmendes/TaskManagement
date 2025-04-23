using Microsoft.AspNetCore.Authorization;

namespace TaskManagement.API.Configurations.Authentication
{
    public sealed class HasScopeRequirement(string scope) : IAuthorizationRequirement
    {
        public string Scope { get; } = scope ?? throw new ArgumentNullException(nameof(scope));
    }
}
