using BulletinBoard.UserService.AppServices.Common.Exceptions.Common.FieldFailures;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ValidationException = BulletinBoard.UserService.AppServices.Common.Exceptions.ValidationException;


namespace BulletinBoard.UserService.AppServices.Common.Behaviors.Validating;

/// <summary>
/// Промежуточный валидатор команд.
/// Автоматически наложен на все обработчики команд и проверяет 
/// команду с помощью валидаторов наследующихся от абстрактного 
/// класса AbstractValidator<TКоманда>
/// </summary>
public class ValidatingBehavior<TRequest, TResponse>
         : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ValidatingBehavior<TRequest, TResponse>> _logger;
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidatingBehavior(
        ILogger<ValidatingBehavior<TRequest, TResponse>> logger, 
        IEnumerable<IValidator<TRequest>> validators)
    {
        _logger = logger;
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            _logger.LogWarning("Начало валидации.");
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(e => e.Errors).Where(f => f != null).ToList();
            if (failures.Count != 0)
            {
                _logger.LogWarning("Валидация провалилась.");
                throw new ValidationException(FieldFailuresConverter.FromValidationErrors(failures));
            }
        }
        _logger.LogWarning("Валидация пройдена.");

        return await next();
    }
}