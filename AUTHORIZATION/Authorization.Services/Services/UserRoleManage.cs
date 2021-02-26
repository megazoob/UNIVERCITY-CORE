using Authorization.Data.Models;
using Authorization.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Roles;

namespace Authorization.Services.Services
{
    public class UserRoleManage : IUserRoleManage
    {
        UserManager<User> _userManager;
        IStringLocalizer _localizer;

        public UserRoleManage(UserManager<User> userManager, IStringLocalizer localizer)
        {
            _userManager = userManager;
            _localizer = localizer;
        }

        /// <summary>
        /// Если все нормально, возвращает "true".
        /// </summary>
        /// <param name="identificator"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<string> AddRoleToUser(string identificator, string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                return _localizer["RoleNotFound"];
            }

            User user = await getUser(identificator);

            if (user == null)
            {
                return _localizer["UserNotFound"];
            }
            else
            {
                if (role.Equals(Roles.Admin) || role.Equals(Roles.Employee) ||
                    role.Equals(Roles.Student) || role.Equals(Roles.WithoutRole))
                {
                    var result = await _userManager.AddToRoleAsync(user, role);
                    if (result.Succeeded)
                    {
                        return true.ToString();
                    }
                    else
                    {
                        string error = "";
                        foreach (var err in result.Errors)
                        {
                            error += err.Description + "\n";
                        }
                        return error;
                    }
                }
                else
                {
                    return _localizer["RoleNotFound"];
                }
            }
        }

        /// <summary>
        /// В случае успеха возвращает "true".
        /// </summary>
        /// <param name="identificator"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<string> RemoveRoleFromUser(string identificator, string role)
        {
            User user = await getUser(identificator);

            if (user == null)
            {
                return _localizer["UserNotFound"];
            }
            else
            {
                if (role.Equals(Roles.Admin) || role.Equals(Roles.Employee) ||
                    role.Equals(Roles.Student) || role.Equals(Roles.WithoutRole))
                {
                    var result = await _userManager.RemoveFromRoleAsync(user, role);
                    if (result.Succeeded)
                    {
                        return true.ToString();
                    }
                    else
                    {
                        string error = "";
                        foreach (var err in result.Errors)
                        {
                            error += err.Description + "\n";
                        }
                        return error;
                    }
                }
                else
                {
                    return _localizer["RoleNotFound"];
                }
            }
        }

        /// <summary>
        /// В случае успеха возвращает "true".
        /// </summary>
        /// <param name="identificator"></param>
        /// <param name="restore"></param>
        /// <returns></returns>
        public async Task<string> UpdateUserStatus(string identificator, bool restore)
        {
            string ret = "false";

            if (restore)
            {
                restore = false;
            } else
            {
                restore = true;
            }

            var user = await getUser(identificator);

            if (user != null)
            {
                user.LockoutEnabled = restore;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    ret = true.ToString();
                }
                else
                {
                    foreach (var err in result.Errors)
                    {
                        ret += err.Code + " " + err.Description + "\n";
                    }
                }
            }
            else
            {
                ret = "User not found.";
            }


            return ret;
        }

        async Task<User> getUser(string identificator)
        {
            return await _userManager.FindByIdAsync(identificator);
        }
    }
}
