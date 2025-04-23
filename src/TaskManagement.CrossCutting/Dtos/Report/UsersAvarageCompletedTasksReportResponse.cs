
namespace TaskManagement.CrossCutting.Dtos.Report
{
    public class UsersAvarageCompletedTasksReportResponse
    {
        public Guid UserId { get; set; }
        public double AvarageCompletedTasks { get; set; }
    }
}
