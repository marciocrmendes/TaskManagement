namespace TaskManagement.CrossCutting.Dtos.Project
{
    public sealed record CreateProjectResponse(Guid Id, string Name, string? Description);
}
