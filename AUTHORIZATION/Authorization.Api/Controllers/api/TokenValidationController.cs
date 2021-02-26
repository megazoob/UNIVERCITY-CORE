using Authorization.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.Roles;

namespace Authorization.Api.Controllers.api
{
    /// <summary>
    /// Авторизация сервисов по токену.
    /// </summary>
    //[Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class TokenValidationController : ControllerBase
    {
        ITokenValidation _tokenValidation;

        public TokenValidationController(ITokenValidation tokenValidation)
        {
            _tokenValidation = tokenValidation;
        }

        /// <summary>
        /// Проверка - в роли ли пользователь.
        /// Пользователь определяется по токену авторизации.
        /// </summary>
        /// <param name="role"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        [HttpGet("api/isuserinrole")]
        [Authorize(Policy = "ValidAccessToken", Roles = Roles.AllRoles)]
        public async Task<Boolean> IsUserInRole(string role, string culture="ru")
        {
            string token = this.HttpContext.Request.Headers["Authorization"];
            return await _tokenValidation.IsUserInRole(token, role);
        }

        /// <summary>
        /// Проверка - авторизован ли пользователь.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        [HttpGet("api/isuserauthorized")]
        [Authorize(Policy = "ValidAccessToken", Roles = Roles.AllRoles)]
        public async Task<Boolean> IsUserAuthorized(string culture="ru")
        {
            string token = this.HttpContext.Request.Headers["Authorization"];
            return await _tokenValidation.IsUserAuthorized(token);
        }

    }
}
