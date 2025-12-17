using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletinBoard.UserService.AppServices.User.Repositiry;

public interface IUserRepository
{
    public Task<IdentityUser?> FindByPhoneAsync(string phoneNumber, CancellationToken cancellationToken);
}
