using Authorization.Services.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Services.Interfaces
{
    /// <summary>
    /// Получение информации о пользователе
    /// </summary>
    /// <returns>
    /// UserInfoResponse
    /// </returns>
    public interface IUserInfo
    {
        /// <summary>
        /// Возвращает информацию о пользовтеле.
        /// </summary>
        /// <param name="model" type="UserInfoResponse"></param>
        /// <param name="email"></param>
        /// <returns>
        /// UserInfoResponse
        /// </returns>
        public Task<UserInfoResponse> GetUserInfo(UserInfoResponse model, string email);
    }
}
