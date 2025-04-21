using FluentValidation;
using MediatR;
using TaskManagement.CrossCutting.Notifications;

namespace TaskManagement.CrossCutting.Validation
{
    public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators,
        INotificationHandler notificationHandler) :
        IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);

            var validationFailures = await Task.WhenAll(
                validators.Select(validator => validator.ValidateAsync(context)));

            IReadOnlyCollection<Notification> errors = [.. validationFailures
                .Where(validationResult => !validationResult.IsValid)
                .SelectMany(validationResult => validationResult.Errors)
                .Select(validationFailure => new Notification(
                    validationFailure.PropertyName,
                    validationFailure.ErrorMessage))];

            if (errors.Count != 0)
            {
                notificationHandler.AddNotifications(errors);
                return default!;
            }

            return await next(cancellationToken);
        }
    }
}
