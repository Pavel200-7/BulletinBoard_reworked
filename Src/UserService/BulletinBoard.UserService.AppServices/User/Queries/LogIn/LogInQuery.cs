using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletinBoard.UserService.AppServices.User.Queries.LogIn;

public class LogInQuery : IRequest<LogInResponse>
{
    public string Email { get; init; }
    public string Password { get; init; }
}
