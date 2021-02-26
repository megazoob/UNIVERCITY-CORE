using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Services.Interfaces
{
    /// <summary>
    /// Интерфейс проверок токена.
    /// </summary>
    public interface ITokenValidation
    {
        /// <summary>
        /// Проверка - присутствует ли роль у пользователя.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public Task<Boolean> IsUserInRole(string token, string role);

        /// <summary>
        /// Проверка - авторизован ли пользователь.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<Boolean> IsUserAuthorized(string token);
    }
}
