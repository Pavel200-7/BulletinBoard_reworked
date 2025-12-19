using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletinBoard.UserService.AppServices.Common.Configurations;

public class RefreshTokenOptions
{
    public TimeSpan TokenLifespan { get; set; } = TimeSpan.FromDays(7);
}
