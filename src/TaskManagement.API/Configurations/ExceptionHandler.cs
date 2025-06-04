using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.API.Configurations
{
    internal sealed class ExceptionHandler(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<ExceptionHandler> logger,
        IWebHostEnvironment environment
    ) : IExceptionHandler
    {
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        private readonly ILogger<ExceptionHandler> _logger = logger;

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken
        )
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

            var statusCode = (int)HttpStatusCode.InternalServerError;

            var problemDetails = new ProblemDetails
            {
                Title = "Erro no servidor",
                Detail = "Ocorreu um erro no servidor. Por favor, tente novamente",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                Status = statusCode,
            };

            if (!environment.IsProduction())
            {
                problemDetails.Extensions.Add("exception", exception.Message);
                problemDetails.Extensions.Add("stacktrace", exception.StackTrace);
            }

            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
