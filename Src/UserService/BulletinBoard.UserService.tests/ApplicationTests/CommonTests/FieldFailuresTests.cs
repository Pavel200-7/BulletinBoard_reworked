using BulletinBoard.UserService.AppServices.Common.Exceptions.Common;
using Microsoft.AspNetCore.Identity;


namespace BulletinBoard.UserService.tests.ApplicationTests.CommonTests;

public class FieldFailuresTests
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

        List<FieldFailures> expected = new List<FieldFailures>()
        {
            new FieldFailures("Email", new List<string>() { "Email error 1", "Email error 2" }),
            new FieldFailures("Phone", new List<string>() { "Phone error 1" }),
            new FieldFailures("UserName", new List<string>() { "UserName error 1", "UserName error 2", "UserName error 3" })
        };

        // Act 
        List<FieldFailures> resultlist = FieldFailures.FromIdentityErrors(errors);

        // Assert
        Assert.Equal(expected.Count, resultlist.Count);
        Assert.Equal(expected
                .Where(e => e.FieldName == "Email")
                .Select(e => e.FieldName),
            resultlist
                .Where(e => e.FieldName == "Email")
                .Select(e => e.FieldName));
        Assert.Equal(expected
                .Where(e => e.FieldName == "Email")
                .Select(e => e.Failures.Count),
            resultlist
                .Where(e => e.FieldName == "Email")
                .Select(e => e.Failures.Count));
        Assert.Equal(expected
                .Where(e => e.FieldName == "Email")
                .Select(e => e.Failures.FirstOrDefault()),
            resultlist
                .Where(e => e.FieldName == "Email")
                .Select(e => e.Failures.FirstOrDefault()));
        Assert.Equal(expected
                .Where(e => e.FieldName == "UserName")
                .Select(e => e.Failures.Count),
            resultlist
                .Where(e => e.FieldName == "UserName")
                .Select(e => e.Failures.Count));
        Assert.Equal(expected
                .Where(e => e.FieldName == "UserName")
                .Select(e => e.Failures.FirstOrDefault()),
            resultlist
                .Where(e => e.FieldName == "UserName")
                .Select(e => e.Failures.FirstOrDefault()));
    }
}
