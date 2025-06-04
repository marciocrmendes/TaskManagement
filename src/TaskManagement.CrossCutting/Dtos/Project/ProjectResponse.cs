namespace TaskManagement.CrossCutting.Dtos.Project
{
    public sealed record ProjectResponse(Guid Id, string Name, string? Description);
}
