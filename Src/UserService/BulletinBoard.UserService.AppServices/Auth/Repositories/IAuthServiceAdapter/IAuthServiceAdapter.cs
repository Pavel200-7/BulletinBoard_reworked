using BulletinBoard.UserService.AppServices.Auth.Repositories.IAuthServiceAdapter.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletinBoard.UserService.AppServices.Auth.Repositories.IAuthServiceAdapter;

/// <summary>
/// Интерфейс, задающий стандарт операций авторизации для внешних провайдеров.
/// </summary>
public interface IAuthServiceAdapter
{
    /// <summary>
    /// Создать в БД сущность пользователя.
    /// </summary>
    /// <param name="userDto">Данные пользователя</param>
    /// <returns>Был ли создан пользователь</returns>
    public Task<bool> RegisterAsync(UserDto userDto);
}
