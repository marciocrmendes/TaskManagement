using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Features.TaskComments.Commands;
using TaskManagement.Application.Features.Tasks.Commands;
using TaskManagement.Application.Features.Tasks.Queries;

namespace TaskManagement.API.Endpoints
{
    public static class TaskEndpoints
    {
        public static IEndpointRouteBuilder RegisterTaskEndpoints(
            this IEndpointRouteBuilder endpoints
        )
        {
            var routeGroupBuilder = endpoints
                .MapGroup("/task")
                .WithTags("Task")
                .RequireAuthorization("UserPolicy");

            routeGroupBuilder.MapPost(
                "/",
                async (
                    [FromBody] CreateTaskCommand command,
                    ISender sender,
                    CancellationToken cancellationToken
                ) =>
                {
                    var result = await sender.Send(command, cancellationToken);
                    return Results.Created("/", result);
                }
            );

            routeGroupBuilder.MapGet(
                "/{id:guid}",
                async ([FromRoute] Guid id, ISender sender, CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(new GetTaskByIdQuery(id), cancellationToken);
                    return Results.Ok(result);
                }
            );

            routeGroupBuilder.MapPut(
                "/",
                async (
                    [FromBody] UpdateTaskCommand command,
                    ISender sender,
                    CancellationToken cancellationToken
                ) =>
                {
                    var result = await sender.Send(command, cancellationToken);
                    return Results.Ok(result);
                }
            );

            routeGroupBuilder.MapDelete(
                "/{id:guid}",
                async ([FromRoute] Guid id, ISender sender, CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(new DeleteTaskCommand(id), cancellationToken);
                    return Results.NoContent();
                }
            );

            routeGroupBuilder.MapPost(
                "/{id:guid}/comment",
                async (
                    [FromRoute] Guid id,
                    [FromBody] CreateTaskCommentCommand command,
                    ISender sender,
                    CancellationToken cancellationToken
                ) =>
                {
                    command.TaskId = id;
                    var result = await sender.Send(command, cancellationToken);
                    return Results.NoContent();
                }
            );

            return endpoints;
        }
    }
}
