using MediatR;
using TaskManagement.CrossCutting.Dtos;
using TaskManagement.CrossCutting.Dtos.Report;

namespace TaskManagement.Domain.Aggregates.Report.Queries
{
    public record GetUsersAvarageCompletedTasksQuery : IRequest<GenericResponseList<UsersAvarageCompletedTasksReportResponse>>;
}
