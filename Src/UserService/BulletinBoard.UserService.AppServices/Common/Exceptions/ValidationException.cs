using BulletinBoard.UserService.AppServices.Common.Exceptions.Common;
using FluentValidation.Results;
using System.Collections.Generic;


namespace BulletinBoard.UserService.AppServices.Common.Exceptions;

/// <summary>
/// Ошибки валидации
/// </summary>
public class ValidationException : DomainIntegrityException
{
    public ValidationException(List<FieldFailures> fieldsFailures, string message = "Ошибки валидации") 
        : base(fieldsFailures, message)
    {
    }

    public ValidationException(List<ValidationFailure> fieldsFailures, string message = "Ошибки валидации")
        : base(ConvertValidationFailure(fieldsFailures), message)
    {
    }

    private static List<FieldFailures> ConvertValidationFailure(List<ValidationFailure> failures)
    {
        return failures.Any() ?
            failures
            .GroupBy(f => f.PropertyName)
            .Select(g => new FieldFailures(
                fieldName: g.Key,
                fieldFailures: g.Where(g => !string.IsNullOrEmpty(g.ErrorMessage))
                    .Select(g => g.ErrorMessage)
                    .ToList()
                ))
            .Where(f => f.Failures.Any())
            .ToList() : new List<FieldFailures>();
    }
}
