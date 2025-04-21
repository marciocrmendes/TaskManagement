using Microsoft.AspNetCore.Mvc;
using System.Net;
using TaskManagement.CrossCutting.Notifications;

namespace TaskManagement.API.Filters
{
    public sealed class NotificationFilter(INotificationHandler notificationContext) : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            var result = await next(context);

            if (!notificationContext.HasNotifications) return result;

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.HttpContext.Response.ContentType = "application/json";

            var problemDetails = new ProblemDetails
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "Erro na validação dos dados",
                Detail = "Um ou mais erros foram encontrados",
                Status = (int)HttpStatusCode.BadRequest,
                Extensions =
                {
                    ["errors"] = notificationContext.Notifications
                }
            };

            return Results.Problem(problemDetails);
        }
    }
}
