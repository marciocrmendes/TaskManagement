using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Domain.Aggregates.Project.Commands;
using TaskManagement.Domain.Aggregates.Project.Queries;

namespace TaskManagement.API.Endpoints
{
    public static class ProjectEndpoints
    {
        public static IEndpointRouteBuilder RegisterProjectEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var routeGroupBuilder = endpoints
                .MapGroup("/project")
                .WithTags("Project")
                .RequireAuthorization("UserPolicy");

            routeGroupBuilder.MapPost("/", async ([FromBody] CreateProjectCommand command,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(command, cancellationToken);
                return Results.Created("/", result);
            });


            routeGroupBuilder.MapGet("/", async (ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(new GetAllProjectsQuery(), cancellationToken);
                return Results.Ok(result);
            });

            routeGroupBuilder.MapGet("/{id:guid}/tasks", async (Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(new GetTasksByIdQuery(id), cancellationToken);
                return Results.Ok(result);
            });

            return endpoints;
        }
    }
}
