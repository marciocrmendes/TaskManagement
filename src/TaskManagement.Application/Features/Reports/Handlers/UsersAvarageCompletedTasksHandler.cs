using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Features.Reports.Queries;
using TaskManagement.CrossCutting.Dtos;
using TaskManagement.CrossCutting.Dtos.Report;
using TaskManagement.CrossCutting.Enums;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Application.Features.Reports.Handlers
{
    public class UsersAvarageCompletedTasksHandler(IProjectRepository projectRepository)
        : IRequestHandler<
            GetUsersAvarageCompletedTasksQuery,
            GenericResponseList<UsersAvarageCompletedTasksReportResponse>
        >
    {
        public async Task<GenericResponseList<UsersAvarageCompletedTasksReportResponse>> Handle(
            GetUsersAvarageCompletedTasksQuery request,
            CancellationToken cancellationToken
        )
        {
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

            var projects = await projectRepository
                .Table.AsNoTracking()
                .Include(p => p.Tasks)
                .Where(x => x.CreatedBy != Guid.Empty)
                .ToListAsync(cancellationToken);

            var userIdList = projects.Select(x => x.CreatedBy).Distinct();

            var result = userIdList
                .Select(userId =>
                {
                    var tasks = projects
                        .Where(p => p.CreatedBy == userId)
                        .SelectMany(p => p.Tasks)
                        .ToArray();

                    var total = tasks.Length;

                    total = total <= 0 ? 1 : total;

                    var completedTasksCount = tasks.Count(t =>
                        t.Status == TaskStatusEnum.Completed
                        && (
                            t.UpdatedAt >= thirtyDaysAgo
                            || (t.UpdatedAt == null && t.CreatedAt >= thirtyDaysAgo)
                        )
                    );

                    return new UsersAvarageCompletedTasksReportResponse
                    {
                        UserId = userId,
                        AvarageCompletedTasks = Math.Round((double)completedTasksCount / total, 2),
                    };
                })
                .OrderByDescending(r => r.AvarageCompletedTasks)
                .ToList();

            return new GenericResponseList<UsersAvarageCompletedTasksReportResponse>(result);
        }
    }
}
