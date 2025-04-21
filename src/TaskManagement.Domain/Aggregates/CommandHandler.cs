using System.Transactions;
using TaskManagement.CrossCutting.Persistences;

namespace TaskManagement.Domain.Aggregates
{
    public abstract class CommandHandler
    {
        protected readonly IUnitOfWork? _unitOfWork;

        private CommandHandler() { }

        protected CommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected static TransactionScope CreateTransactionScope(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            TimeSpan? transactionTimeout = null)
        {
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = isolationLevel,
                Timeout = transactionTimeout ?? TransactionManager.MaximumTimeout
            };

            return new TransactionScope(TransactionScopeOption.Required,
                transactionOptions,
                TransactionScopeAsyncFlowOption.Enabled);
        }

        protected async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            if (_unitOfWork is null) return default!;

            return await _unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
