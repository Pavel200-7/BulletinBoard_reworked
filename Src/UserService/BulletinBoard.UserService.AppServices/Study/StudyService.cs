using BulletinBoard.UserService.AppServices.Study.Helpers;
using BulletinBoard.UserService.Generators.SourceGenerators.Decorators.Logging.MethodLogDecorator;
//using BulletinBoard.UserService.Generators.SourceGenerators.Decorators.Logging.MethodLogDecorator.DecoratorAttribute;


namespace BulletinBoard.UserService.AppServices.Study;

public class StudyService : IStudyService
{
    private readonly ISomeHelper _someHelper;

    public StudyService(ISomeHelper someHelper)
    {
        _someHelper = someHelper;
    }

    [MethodLog("ЛогиЛоги")]
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


