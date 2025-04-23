namespace TaskManagement.Domain.Aggregates
{
    public abstract class DomainEventHandler
    {
        protected readonly IServiceProvider? _serviceProvider;

        private DomainEventHandler() { }

        protected DomainEventHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
    }
}
