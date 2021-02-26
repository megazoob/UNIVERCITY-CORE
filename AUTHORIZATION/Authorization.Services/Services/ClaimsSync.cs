using Authorization.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Services.Services
{
    public class ClaimsSync
    {
        /// <summary>
        /// Создает унифицрованный список клеймов
        /// </summary>
        /// <param name="user" type = "User"></param>
        /// <param name="userManager" type = "UserManager User"></param>
        /// <param name="sid"></param>
        /// <returns>
        /// Список клеймов
        /// </returns>
        public static async Task<List<Claim>> GetSyncedClaims(User user, UserManager<User> userManager, string sid)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(ClaimTypes.Sid, sid));
            var roles = await userManager.GetRolesAsync(user);

            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role));

            }
            return claims;
        }
    }
}
