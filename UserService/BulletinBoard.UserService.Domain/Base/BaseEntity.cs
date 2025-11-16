using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletinBoard.UserService.Domain.Base;

/// <summary>
/// Базовая сущность
/// </summary>
public class BaseEntity
{
    /// <summary>
    /// Базовый идентификатор сущности.
    /// </summary>
    public Guid Id { get; set; }
}
