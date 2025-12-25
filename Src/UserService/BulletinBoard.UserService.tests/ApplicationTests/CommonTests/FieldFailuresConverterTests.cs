using BulletinBoard.UserService.AppServices.Common.Exceptions.Common.FieldFailures;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;


namespace BulletinBoard.UserService.tests.ApplicationTests.CommonTests;

public class FieldFailuresConverterTests
{
    [Fact]
    public void FromIdentityErrorsTest()
    {
        // Arrange 
        List<IdentityError> errors = new List<IdentityError>()
        {
            new IdentityError() { Code = "UserName", Description = "UserName error 1" },
            new IdentityError() { Code = "UserName", Description = "UserName error 2" },
            new IdentityError() { Code = "UserName", Description = "UserName error 3" },
            new IdentityError() { Code = "Email", Description = "Email error 1" },
            new IdentityError() { Code = "Email", Description = "Email error 2" },
            new IdentityError() { Code = "Phone", Description = "Phone error 1" },
        };

        List<FieldFailure> expected = CreateExpectedFieldFailures();

        // Act 
        List<FieldFailure> resultlist = FieldFailuresConverter.FromIdentityErrors(errors);

        // Assert
        AssertEqual(expected, resultlist);
    }

    [Fact]
    public void FromValidationErrorsTest()
    {
        // Arrange 
        List<ValidationFailure> errors = new List<ValidationFailure>()
        {
            new ValidationFailure() { PropertyName = "UserName", ErrorMessage = "UserName error 1" },
            new ValidationFailure() { PropertyName = "UserName", ErrorMessage = "UserName error 2" },
            new ValidationFailure() { PropertyName = "UserName", ErrorMessage = "UserName error 3" },
            new ValidationFailure() { PropertyName = "Email", ErrorMessage = "Email error 1" },
            new ValidationFailure() { PropertyName = "Email", ErrorMessage = "Email error 2" },
            new ValidationFailure() { PropertyName = "Phone", ErrorMessage = "Phone error 1" },
        };

        List<FieldFailure> expected = CreateExpectedFieldFailures();

        // Act 
        List<FieldFailure> resultlist = FieldFailuresConverter.FromValidationErrors(errors);

        // Assert
        AssertEqual(expected, resultlist);      
    }

    private List<FieldFailure> CreateExpectedFieldFailures()
    {
        return new List<FieldFailure>()
        {
            new FieldFailure("Email", new List<string>() { "Email error 1", "Email error 2" }),
            new FieldFailure("Phone", new List<string>() { "Phone error 1" }),
            new FieldFailure("UserName", new List<string>() { "UserName error 1", "UserName error 2", "UserName error 3" })
        };
    }

    private void AssertEqual(List<FieldFailure> expected, List<FieldFailure> actual)
    {
        Assert.Equal(expected.Count, actual.Count);
        Assert.Equal(expected
                .Where(e => e.FieldName == "Email")
                .Select(e => e.FieldName),
            actual
                .Where(e => e.FieldName == "Email")
                .Select(e => e.FieldName));
        Assert.Equal(expected
                .Where(e => e.FieldName == "Email")
                .Select(e => e.Failures.Count),
            actual
                .Where(e => e.FieldName == "Email")
                .Select(e => e.Failures.Count));
        Assert.Equal(expected
                .Where(e => e.FieldName == "Email")
                .Select(e => e.Failures.FirstOrDefault()),
            actual
                .Where(e => e.FieldName == "Email")
                .Select(e => e.Failures.FirstOrDefault()));
        Assert.Equal(expected
                .Where(e => e.FieldName == "UserName")
                .Select(e => e.Failures.Count),
            actual
                .Where(e => e.FieldName == "UserName")
                .Select(e => e.Failures.Count));
        Assert.Equal(expected
                .Where(e => e.FieldName == "UserName")
                .Select(e => e.Failures.FirstOrDefault()),
            actual
                .Where(e => e.FieldName == "UserName")
                .Select(e => e.Failures.FirstOrDefault()));
    }
}
