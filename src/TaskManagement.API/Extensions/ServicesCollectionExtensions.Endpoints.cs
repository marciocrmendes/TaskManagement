using TaskManagement.API.Endpoints;
using TaskManagement.API.Filters;

namespace TaskManagement.API.Extensions;

public static class ServicesCollectionExtensionsEndpoints
{
    public static IEndpointRouteBuilder RegisterEndpoints(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGroup("/api/v1")
            .RequireAuthorization()
            .AddEndpointFilter<NotificationFilter>()
            .RegisterAuthEndpoints()
            .RegisterTaskEndpoints()
            .RegisterProjectEndpoints()
            .RegisterReportEndpoints();
    }
}
