using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletinBoard.UserService.AppServices.Study;

public class StudyServiceLoggingDecorator : IStudyService
{
    private readonly IStudyService _decorated;
    public readonly ILogger<StudyServiceLoggingDecorator> _logger;

    public StudyServiceLoggingDecorator(IStudyService decorated, ILogger<StudyServiceLoggingDecorator> logger)
    {
        _decorated = decorated;
        _logger = logger;
    }

    public void DoSomeThing(string someArgument)
    {
        _logger.LogInformation("Я типо есть и я типо логирую");
        _decorated.DoSomeThing(someArgument);
    }

    public void DoSomeThingElse(string someArgument)
    {
        _decorated.DoSomeThingElse(someArgument);
    }
}
