using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Filter.Authorization
{
    /// <summary>
    /// Атрибут авторизации.
    /// [IsAuthorized]
    /// </summary>
    public class IsAuthorizedAttribute : TypeFilterAttribute
    {
        public IsAuthorizedAttribute() : base(typeof(IsAuthorizedFilter))
        {
            Arguments = new object[] { };
        }
    }

    /// <summary>
    /// Просто проверяем, авторизован или нет. Без разделения ролей.
    /// </summary>
    public class IsAuthorizedFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string token = "";
            try
            {
                token = context.HttpContext.Request.Headers["Authorization"];
            } catch (Exception e)
            {
                token = "";
            }
            
            if (string.IsNullOrEmpty(token) || !IsAuthorized(token))
            {
                context.Result = new UnauthorizedResult();
            }
            
        }

        /// <summary>
        /// Проверка, атворизован ли пользователь.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Boolean IsAuthorized(string token)
        {
            return true; //заглушка
        }

    }
}
