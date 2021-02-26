using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Gateway.Services.Inerfaces
{
    /// <summary>
    /// Токен приходит в эту библиотеку, которая сама сформирует 
    /// запрос нужного типа и отправит куда нужно, вернув результат.
    /// </summary>
    public interface IAuthorizationGateway
    {
        /// <summary>
        /// Просто проверяем, авторизован или нет. Без разделения ролей.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<Boolean> IsAuthorizedAsync(string token);

        /// <summary>
        /// Авторизации для выбранных ролей. В roles можно передать роли через запятую.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public Task<Boolean> AuthorizationWithRolesAsync(string token, string roles);
    }
}
