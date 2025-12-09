using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletinBoard.UserService.AppServices.Study.Helpers;

public class SomeHelper : ISomeHelper
{
    public string SomeHelp(string someArgument)
    {
        return $"Какое-то выражение: {someArgument} - {someArgument}";
    }
}
