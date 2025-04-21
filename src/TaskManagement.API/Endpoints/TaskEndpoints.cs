using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Domain.Aggregates.Task.Commands;

namespace TaskManagement.API.Endpoints
{
    public static class TaskEndpoints
    {
        public static IEndpointRouteBuilder RegisterTaskEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var routeGroupBuilder = endpoints
                .MapGroup("/task")
                .WithTags("Task");

            routeGroupBuilder.MapPost("/", async ([FromBody] CreateTaskCommand command,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(command, cancellationToken);
                return Results.Created("/", result);
            });

            //routeGroupBuilder.MapGet("/{id}", async (
            //    Guid id,
            //    ISender sender,
            //    CancellationToken cancellationToken) =>
            //{
            //    var result = await sender.Send(command, cancellationToken);

            //    return Results.Created("/", result);
            //});

            return endpoints;
        }
    }
}
