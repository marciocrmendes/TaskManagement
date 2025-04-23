using MediatR;
using TaskManagement.CrossCutting.Dtos.Authetication;
using TaskManagement.Domain.Aggregates.Authentication.Commands;
using TaskManagement.Domain.Services;

namespace TaskManagement.Domain.Aggregates.Project.Handlers
{
    public sealed class CreateAccessTokenHandler(TokenProvider tokenProvider) : 
        IRequestHandler<CreateAccessTokenCommand, CreatedTokenResponse>
    {
        public async Task<CreatedTokenResponse> Handle(CreateAccessTokenCommand request, 
            CancellationToken cancellationToken)
        {
            var token = tokenProvider.Create(new Entities.User(request.Name, request.Email), request.AuthType);
            return await System.Threading.Tasks.Task.FromResult(new CreatedTokenResponse(token));
        }
    }
}
