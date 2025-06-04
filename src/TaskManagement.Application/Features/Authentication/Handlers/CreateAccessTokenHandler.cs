using MediatR;
using TaskManagement.Application.Features.Authentication.Commands;
using TaskManagement.CrossCutting.Dtos.Authetication;
using TaskManagement.Domain.Services;

namespace TaskManagement.Application.Features.Authentication.Handlers
{
    public sealed class CreateAccessTokenHandler(TokenProvider tokenProvider)
        : IRequestHandler<CreateAccessTokenCommand, CreatedTokenResponse>
    {
        public async Task<CreatedTokenResponse> Handle(
            CreateAccessTokenCommand request,
            CancellationToken cancellationToken
        )
        {
            var token = tokenProvider.Create(
                new Domain.Entities.User(request.Name, request.Email),
                request.AuthType
            );
            return await System.Threading.Tasks.Task.FromResult(new CreatedTokenResponse(token));
        }
    }
}
