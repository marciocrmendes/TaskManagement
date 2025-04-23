using MediatR;
using TaskManagement.Domain.Aggregates.Report.Queries;

namespace TaskManagement.API.Endpoints
{
    public static class ReportEndpoints
    {
        public static IEndpointRouteBuilder RegisterReportEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var routeGroupBuilder = endpoints
                .MapGroup("/report")
                .WithTags("Report")
                .RequireAuthorization("ManagerPolicy");

            routeGroupBuilder.MapGet("/user/avarege-completed-task", async (ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(new GetUsersAvarageCompletedTasksQuery(), cancellationToken);
                return Results.Ok(result);
            });

            return endpoints;
        }
    }
}
