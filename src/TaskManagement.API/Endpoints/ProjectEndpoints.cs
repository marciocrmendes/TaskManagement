using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Domain.Aggregates.Project.Commands;

namespace TaskManagement.API.Endpoints
{
    public static class ProjectEndpoints
    {
        public static IEndpointRouteBuilder RegisterProjectEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var routeGroupBuilder = endpoints
                .MapGroup("/project")
                .WithTags("Project");

            routeGroupBuilder.MapPost("/", async ([FromBody] CreateProjectCommand command,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(command, cancellationToken);
                return Results.Created("/", result);
            });

            return endpoints;
        }
    }
}
