using Authorization.Gateway.Services.Inerfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Filter.Authorization
{
    /// <summary>
    /// Атрибут авторизации для выбранных ролей.
    /// [AuthorizationWithRoles("Administrator,Employee,Student")]
    /// </summary>
    public class AuthorizationWithRolesAttribute : TypeFilterAttribute
    {
        public AuthorizationWithRolesAttribute(string roles) : base(typeof(AuthorizationWithRolesFilter))
        {
            Arguments = new object[] { new string(roles) };
        }
    }

    public class AuthorizationWithRolesFilter : IAuthorizationFilter
    {
        readonly string _roles;
        readonly IAuthorizationGateway _gateway;

        public AuthorizationWithRolesFilter(string roles, IAuthorizationGateway gateway)
        {
            _roles = roles;
            _gateway = gateway;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string token = "";
            try
            {
                token = context.HttpContext.Request.Headers["Authorization"];
            }
            catch (Exception e)
            {
                token = "";
            }

            //если нет токена или пользователь не входит в список перечисленных через запятую _roles
            if (string.IsNullOrEmpty(token) || !UserIsInRole(token, _roles))
            {
                context.Result = new UnauthorizedResult();
            }
        }

        Boolean UserIsInRole(string token, string roles)
        {

            //отправить токен в библиотеку, которая сама сформирует запрос нужного типа и отправит куда нужно - здесь только результат.
            return _gateway.AuthorizationWithRolesAsync(token, roles).GetAwaiter().GetResult();

        }


    }
}
