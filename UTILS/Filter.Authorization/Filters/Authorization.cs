using Authorization.Gateway.Services.Inerfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

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
        IAuthorizationGateway _gateway;

        public IsAuthorizedFilter(IAuthorizationGateway gateway)
        {
            _gateway = gateway;
        }

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

            //отправить токен в библиотеку, которая сама сформирует запрос нужного типа и отправит куда нужно - здесь только результат.
            return _gateway.IsAuthorizedAsync(token).GetAwaiter().GetResult(); //вызываем асинхронную функцию из синхронного метода.

        }

    }
}
