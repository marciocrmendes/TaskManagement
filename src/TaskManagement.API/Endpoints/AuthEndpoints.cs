using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Domain.Aggregates.Authentication.Commands;

namespace TaskManagement.API.Endpoints
{
    public static class AuthEndpoints
    {
        public static IEndpointRouteBuilder RegisterAuthEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var routeGroupBuilder = endpoints
                .MapGroup("/auth")
                .AllowAnonymous()
                .WithTags("Authentication");

            routeGroupBuilder.MapPost("/", async ([FromBody] CreateAccessTokenCommand command,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(command, cancellationToken);
                return Results.Created("/", result);
            }).WithDescription("AuthType: [0] - User | [1] - Manager");

            return endpoints;
        }
    }
}
