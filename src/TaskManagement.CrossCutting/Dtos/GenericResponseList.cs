namespace TaskManagement.CrossCutting.Dtos
{
    public sealed class GenericResponseList<T>(IReadOnlyCollection<T> items)
    {
        public IReadOnlyCollection<T> Items { get; } = items;
        public int TotalCount { get; } = items?.Count ?? 0;
    }
}