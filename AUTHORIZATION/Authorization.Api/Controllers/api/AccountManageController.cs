using Authorization.Api.Models;
using Authorization.Data.Models;
using Authorization.Services.Interfaces;
using Authorization.Services.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Users.Roles;

namespace Authorization.Api.Controllers.api
{
    /// <summary>
    /// Операции с аккаунтом пользователя:
    /// Логин, регистрация, редактирование пароля, смена ролей, списки, информация.
    /// </summary>
    //[Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class AccountManageController : ControllerBase
    {
        ILogin _login;
        IStringLocalizer _localizer;
        IRegister _register;
        IUserRoleManage _roleManager;
        IUserInfo _userInfo;
        IUserProfileManage _profile;

        public AccountManageController(ILogin login, IStringLocalizer localizer, 
                                       IRegister register, IUserRoleManage roleManager,
                                       IUserInfo userInfo, IUserProfileManage profile)
        {
            _login = login;
            _localizer = localizer;
            _register = register;
            _roleManager = roleManager;
            _userInfo = userInfo;
            _profile = profile;
        }

        /// <summary>
        /// Авторизация. Обязательно передать логи и пароль. Вторым параметром (culture) передать значение нужной локализации  (варианты значения: ru, en, de)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="culture"></param>
        /// <remarks>
        /// Возвращает токен безопасности в поле Token или ошибку в поле Error
        /// или массив Errors в RequestBody
        /// </remarks>
        [HttpPost("api/login")]
        public async Task<LoginViewModel> Login(LoginViewModel model, string culture = "ru")
        {

            if (ModelState.IsValid)
            {
                model = await _login.Login(model); //авторизация и получение токена
                if (string.IsNullOrEmpty(model.Token))
                {
                    model.Error = _localizer["LoginError"];
                }
            }
            else
            {
                model.Error = _localizer["LoginRequired"] + "\n" + _localizer["PasswordRequired"] + "\n" + _localizer["PasswordLength"];
            }

            return model;
        }

        /// <summary>
        /// Пересоздание токена. В заголовке Authorization вместо обычного токена передаем RefreshToken.
        /// </summary>
        /// <remarks>
        /// Возвращает новый токен безопасности в виде строки.
        /// </remarks>
        [HttpPost("api/refresh-token")]
        public async Task<string> RefreshToken(string refreshToken)
        {
            return await _login.GetRefreshedToken(refreshToken);
        }

        /// <summary>
        /// Функция регистрации пользователя. Параметром (culture) передать значение нужной локализации  (варианты значения: ru, en, de).
        /// </summary>
        /// <param name="model"></param>
        /// <param name="culture"></param>
        /// <remarks>
        /// В случае ошибки возвращает ее описание в поле Error
        /// или массив Errors в RequestBody.
        /// </remarks>
        /// <returns></returns>
        [HttpPost("api/register")]
        public async Task<RegisterViewModel> Register(RegisterViewModel model, string culture = "ru")
        {
            if (ModelState.IsValid)
            {
                model = await _register.Register(model);
            }
            else
            {
                model.Error = _localizer["LoginRequired"] + "\n" + _localizer["PasswordRequired"] + "\n" + _localizer["PasswordLength"] + "\n" + _localizer["DateOfBirnRequired"];
            }

            return model;
        }

        /// <summary>
        /// Функция регистрации пользователя. Параметром (culture) передать значение нужной локализации  (варианты значения: ru, en, de).
        /// Параметром role передать роль, в которую должен входить пользователь.
        /// Доступно только админам.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="role"></param>
        /// <param name="culture"></param>
        /// <remarks>
        /// В случае ошибки возвращает ее описание в поле Error
        /// или массив Errors в RequestBody.
        /// </remarks>
        /// <returns></returns>
        [HttpPost("api/register-with-role")]
        [Authorize(Policy = "ValidAccessToken", Roles = Roles.Admin)]
        public async Task<RegisterViewModel> RegisterWithRole(RegisterViewModel model, string role="", string culture = "ru")
        {
            if (ModelState.IsValid)
            {
                model = await _register.Register(model,role);
            }
            else
            {
                model.Error = _localizer["LoginRequired"] + "\n" + _localizer["PasswordRequired"] + "\n" + _localizer["PasswordLength"] + "\n" + _localizer["DateOfBirnRequired"];
            }

            return model;
        }

        /// <summary>
        /// Добавляем пользователю роль. Параметром (culture) передать значение нужной локализации  (варианты значения: ru, en, de).
        /// </summary>
        /// <param name="identificator"></param>
        /// <param name="role"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        [HttpPost("api/addusertorole")]
        [HttpGet("api/addusertorole")]
        [Authorize(Policy = "ValidAccessToken", Roles = Roles.Admin)]
        public async Task<string> AddRoleToUser(string identificator, string role, string culture = "ru")
        {
           return await _roleManager.AddRoleToUser(identificator, role);
        }

        /// <summary>
        /// Исключаем пользователя из роли. Параметром (culture) передать значение нужной локализации  (варианты значения: ru, en, de).
        /// </summary>
        /// <param name="identificator"></param>
        /// <param name="role"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        [HttpPost("api/removeuserfromrole")]
        [HttpGet("api/removeuserfromrole")]
        [Authorize(Policy = "ValidAccessToken", Roles = Roles.Admin)]
        public async Task<string> RemoveRoleFromUser(string identificator, string role, string culture = "ru")
        {
            return await _roleManager.RemoveRoleFromUser(identificator, role);
        }

        /// <summary>
        /// Активирует (restore = true) или деактивирует пользователя (restore = false)
        /// </summary>
        /// <param name="identificator"></param>
        /// <param name="restore"></param>
        /// <returns>True.toString() or Description of error</returns>
        [HttpPost("api/setuserstatus")]
        [Authorize(Policy = "ValidAccessToken", Roles = Roles.Admin)]
        public async Task<string> SetUserStatus(string identificator, Boolean restore)
        {
            return await _roleManager.UpdateUserStatus(identificator, restore);
        }

        ///// <summary>
        ///// Получаем информацию о пользователе. Параметром (culture) передать значение нужной локализации  (варианты значения: ru, en, de).
        ///// </summary>
        ///// <param name="model"></param>
        ///// <param name="culture"></param>
        ///// <returns></returns>
        [HttpPost("api/getuserinfo")]
        [Authorize(Policy = "ValidAccessToken", Roles = Roles.Admin)]
        public async Task<UserInfoResponse> GetUserInfo(UserInfoRequest model, string culture = "ru")
        {
            UserInfoResponse response = new UserInfoResponse();
            if (ModelState.IsValid)
            {

                response = await _userInfo.GetUserInfo(response, model.Email);
            }
            else
            {
                response.Error = _localizer["Email Required"];
            }

            return response;
        }

        /// <summary>
        /// Cписок пользователей.
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/getuserslist")]
        [HttpPost("api/getuserslist")]
        [Authorize(Policy = "ValidAccessToken", Roles = Roles.Admin)]
        public async Task<ICollection<User>> GetUsersList(string culture="ru")
        {
            return await _profile.GetUsersList();
        }

        /// <summary>
        /// Изменение пароля.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("api/updateuserpassword")]
        [Authorize(Policy = "ValidAccessToken", Roles = Roles.Admin)]
        public async Task<UserPassword> UpdateUserPassword(UserPassword model)
        {
            if (ModelState.IsValid)
            {
                model = await _profile.UpdateUserPassword(model);
            }

            return model;
        }
    }
}
