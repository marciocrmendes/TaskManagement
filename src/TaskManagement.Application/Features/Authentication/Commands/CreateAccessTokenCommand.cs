using MediatR;
using TaskManagement.CrossCutting.Dtos.Authetication;
using TaskManagement.CrossCutting.Enums;

namespace TaskManagement.Application.Features.Authentication.Commands
{
    public sealed record CreateAccessTokenCommand(string Name, string Email, AuthTypeEnum AuthType)
        : IRequest<CreatedTokenResponse>;
}
