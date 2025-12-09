using BulletinBoard.UserService.AppServices.Study.Helpers;
using ESourcerGenerator.Attributes;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletinBoard.UserService.AppServices.Study;

public class StudyService : IStudyService
{
    private readonly ISomeHelper _someHelper;

    public StudyService(ISomeHelper someHelper)
    {
        _someHelper = someHelper;
    }

    //[LogCall(LoggerType=typeof(ILogger<StudyService>), LogMessage ="Операция началась.")]
    public void DoSomeThing(string someArgument)
    {
        var someConculation = _someHelper.SomeHelp(someArgument);
        Console.WriteLine("Я полезный, верьте мне. Смотри чо могу. Хопа: {0}", someConculation);
    }

    public void DoSomeThingElse(string someArgument)
    {
        var someConculation = _someHelper.SomeHelp(someArgument);
        someConculation = _someHelper.SomeHelp(someConculation);

        Console.WriteLine("Я полезный, верьте мне. Смотри чо могу. Хопа: {0}", someConculation);
    }
}
