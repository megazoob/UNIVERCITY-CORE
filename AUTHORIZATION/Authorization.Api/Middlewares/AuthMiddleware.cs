using Authorization.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection; //внедрение зависимостей context.RequestServices.GetService<T>()
using System.Security.Claims;
using Users.Roles;
using System.IdentityModel.Tokens.Jwt;

namespace Authorization.Api.Middlewares
{
    /// <summary>
    /// Проверка, валиден ли пользователь и так же не удален ли он из группы администраторов.
    /// Включение middleware в контейнер :  app.UseMiddleware AuthMiddleware()".
    /// Включать только строго после авторизации, так как проверка производится только при наличии
    /// хидера Authorization.
    /// </summary>
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
           if (!string.IsNullOrEmpty(context.Request.Headers["Authorization"]))
                {
                    var userManager = context.RequestServices.GetService<UserManager<User>>();

                    if (userManager == null)
                    {
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                    }
                    else
                    {

                    string sid = context.User.Claims.Where(p => p.Type == ClaimTypes.Sid).FirstOrDefault().Value;
                   
                    
                    

                        if (string.IsNullOrEmpty(sid))
                        {
                            context.Response.StatusCode = StatusCodes.Status404NotFound;
                        }
                        else
                        {
                            User user = userManager.Users.Where(p => p.Id.Equals(sid) && p.EmailConfirmed == true &&
                                                                p.PhoneNumberConfirmed == true &&
                                                                p.LockoutEnabled == false).FirstOrDefault();

                            if (user == null)
                            {
                                context.Response.StatusCode = StatusCodes.Status404NotFound;
                            }
                            else
                            {
                                //может быть так, что клэйм из контекста содержит роль администратора, но пользователю уже сменили роль. 
                                //проверяем этот случац
                                var roleAdminClaimsCount = context.User.Claims.Where(p => p.Type.Contains("role")).Where(p => p.Value == Roles.Admin).Count();
                                if (roleAdminClaimsCount > 0)
                                {
                                    IList<string> roles = await userManager.GetRolesAsync(user);
                                    if (!roles.Contains(Roles.Admin))
                                    {
                                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                                        await context.Response.WriteAsync("Forbidden.");
                                    }
                                    else
                                    {
                                        await _next.Invoke(context);
                                    }
                                }
                                else
                                {
                                    await _next.Invoke(context);
                                }
                            }
                        }
                    }
                }
                else
                {
                    await _next.Invoke(context);
                }
            }
            
        }
    }

