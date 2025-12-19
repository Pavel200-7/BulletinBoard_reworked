using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletinBoard.UserService.AppServices.Common.Exceptions;

public class AccessDeniedExeption : Exception
{
    public AccessDeniedExeption(string message) 
        : base(message)
    {
    }
}
