using Authorization.Data.Models;
using Authorization.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Users.Roles;

namespace Authorization.Services.Services
{
    public class TokenValidation : ITokenValidation
    {
        UserManager<User> _userManager;

        public TokenValidation(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Авторизован ли пользователь.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> IsUserAuthorized(string token)
        {
            token = token.Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadJwtToken(token);

            if (tokenS == null)
            {
                return false;
            }

            var utcNow = DateTime.UtcNow;
            if (utcNow < tokenS.ValidTo)
            {
                var identity = new ClaimsPrincipal(new ClaimsIdentity(tokenS.Claims));
                var sid = identity.Claims.Where(p => p.Type == ClaimTypes.Sid).FirstOrDefault().Value;

                User user = GetUser(sid);

                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Count() == 0)
                    {
                        return false;
                    } else
                    {
                        if (roles.Where(p => p.Contains(Roles.WithoutRole)).Count()>0)
                        {
                            return false;
                        } else
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Разбираем токен по частям).
        /// Присутствует ли роль у пользователя.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<bool> IsUserInRole(string token, string role)
        {
            token = token.Replace("Bearer ", "");
            
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(role))
            {
                return false;
            }

            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadJwtToken(token);

            if (tokenS==null)
            {
                return false;
            }

            var utcNow = DateTime.UtcNow;
            if (utcNow < tokenS.ValidTo)
            {
                var identity = new ClaimsPrincipal(new ClaimsIdentity(tokenS.Claims));
                var sid = identity.Claims.Where(p => p.Type == ClaimTypes.Sid).FirstOrDefault().Value;

                User user = GetUser(sid);

                if (user!=null)
                {
                    IList<string> roles = await _userManager.GetRolesAsync(user);

                    if (roles.Count()==0)
                    {
                        return false;
                    } else
                    {
                        if (roles.Where(p => p.Contains(Roles.WithoutRole)).Count() > 0)
                        {
                            return false;
                        }
                        else
                        {
                            string[] rls = role.Split(",");
                            Boolean have = false;

                            foreach (string r in rls)
                            {
                                if (roles.Where(p => p.Contains(r)).Count() > 0)
                                {
                                    have = true;
                                }
                            }

                            return have;
                            
                        }
                    }
                } else
                {
                    return false;
                }

            } else
            {
                return false;
            }
         }

        User GetUser(string sid)
        {
            return _userManager.Users.Where(p => p.Id.Equals(sid) && p.LockoutEnabled == false
                                                      && p.EmailConfirmed == true
                                                      && p.PhoneNumberConfirmed == true).FirstOrDefault();
        }
    }
}
