using Authorization.Data.Models;
using Authorization.Services.Interfaces;
using Authorization.Services.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Services.Services
{
    /// <summary>
    /// Получение информации о пользователе
    /// </summary>
    /// <returns>
    /// UserInfoResponse
    /// </returns>
    public class UserInfo : IUserInfo
    {
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer _localizer;


        public UserInfo(UserManager<User> userManager, IStringLocalizer localizer)
        {
            _userManager = userManager;
            _localizer = localizer;
        }

        /// <summary>
        /// Возвращает информацию о пользовтеле.
        /// </summary>
        /// <param name="model" type="UserInfoResponse"></param>
        /// <param name="email"></param>
        /// <returns>
        /// UserInfoResponse
        /// </returns>
        public async Task<UserInfoResponse> GetUserInfo(UserInfoResponse model, string email)
        {
            model.Error = "";

            User user = _userManager.Users.Where(p => p.Email.Equals(email)).FirstOrDefault();

            if (user == null)
            {
                model.Error = _localizer["User Not Found"];
            }
            else
            {
                model.Identificator = user.Id;

                model.DateOfBirn = user.DateOfBirn;
                model.Email = user.Email;
                model.UserName = user.UserName;

                IList<string> roles = await _userManager.GetRolesAsync(user);
                foreach (string role in roles)
                {
                    model.Roles += role + "\n";
                }

            }

            return model;
        }
    }
}
