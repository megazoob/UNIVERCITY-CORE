using Authorization.Data.Models;
using Authorization.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.Api.Hubs.SignalR
{
    /// <summary>
    /// Авторизация из внешних сервисов. 
    /// Аналог TokenValidationController.
    /// </summary>
    public class TokenValidationSignalRHub : Hub
    {
        private readonly ITokenValidation _tokenValidation;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenValidation"></param>
        public TokenValidationSignalRHub(ITokenValidation tokenValidation)
        {
            _tokenValidation = tokenValidation;
        }

        /// <summary>
        /// Проверка является ли пользователь с token членом группы role.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="role"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public async Task<Boolean> IsUserInRole(string token, string role, string culture = "ru")
        {
            return await _tokenValidation.IsUserInRole(token, role);
        }

        /// <summary>
        /// Проверка, является ли пользователь из token членом какой-либо группы.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public async Task<Boolean> IsUserAuthorized(string token, string culture = "ru")
        {
            return await _tokenValidation.IsUserAuthorized(token);
        }
    }
}
