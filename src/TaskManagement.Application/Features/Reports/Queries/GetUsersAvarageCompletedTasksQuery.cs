using MediatR;
using TaskManagement.CrossCutting.Dtos;
using TaskManagement.CrossCutting.Dtos.Report;

namespace TaskManagement.Application.Features.Reports.Queries
{
    public record GetUsersAvarageCompletedTasksQuery
        : IRequest<GenericResponseList<UsersAvarageCompletedTasksReportResponse>>;
}
