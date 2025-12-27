using FluentValidation;
using MediatR;
using ValidationException = BulletinBoard.NotificationService.AppServices.Common.Exceptions.ValidationException;
using BulletinBoard.NotificationService.AppServices.Common.Exceptions.Common.FieldFailures;


namespace BulletinBoard.UserService.AppServices.Common.Behaviors.ValidatingBehavior;

public class ValidatingBehavior<TRequest, TResponse>
         : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidatingBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(e => e.Errors).Where(f => f != null).ToList();
            if (failures.Count != 0)
            {
                throw new ValidationException(FieldFailuresConverter.FromValidationErrors(failures));
            }
        }

        return await next();
    }
}