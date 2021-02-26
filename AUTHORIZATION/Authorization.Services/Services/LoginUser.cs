using Authorization.Data.Models;
using Authorization.Services.Interfaces;
using Authorization.Services.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Users.Roles;

namespace Authorization.Services.Services
{
    public class LoginUser : ILogin
    {
        public User currentUser { get; set; }

        private readonly IJwtGenerator _jwtGenerator;
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer _localizer;

        public LoginUser(IJwtGenerator jwtGenerator, UserManager<User> userManager, IStringLocalizer localizer)
        {
            _jwtGenerator = jwtGenerator;
            _userManager = userManager;
            _localizer = localizer;
        }

        public async Task<LoginViewModel> Login(LoginViewModel model)
        {
            model.Error = "";
            model.Token = "";

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                if (!user.LockoutEnabled && user.PhoneNumberConfirmed && user.EmailConfirmed)
                {

                    foreach (Claim claim in await _userManager.GetClaimsAsync(user))
                    {
                        if (claim.Type == ClaimTypes.Name || claim.Type == ClaimTypes.Email || 
                            claim.Type == ClaimTypes.Sid || claim.Type == ClaimsIdentity.DefaultRoleClaimType)
                        {
                            await _userManager.RemoveClaimAsync(user, claim);
                        }
                    }

                   var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Count()==0)
                    {
                        model.Error = _localizer["User Have Not Role"];
                        return model;
                    }

                    if (roles.Contains(Roles.WithoutRole))
                    {
                        model.Error = _localizer["User Without Role"];
                        return model;
                    }

                    //переопределяем claims, чтобы авторизация через токены и авторизация через кукисы обращалась к одним и тем же claims
                    await _userManager.AddClaimsAsync(user, await ClaimsSync.GetSyncedClaims(user, _userManager, user.Id));
                    model.Token = await _jwtGenerator.CreateToken(user, _userManager, user.Id); //получившийся токен
                    model.RefreshToken = await _jwtGenerator.CreateToken(user, _userManager, DateTime.Now.ToString("yyyyyMMddHHmmssfff")); //токен восстановления
                    currentUser = user; //пользователь

                    user.RefreshToken = model.RefreshToken;
                    await _userManager.UpdateAsync(user); //сохраняем RefreshToken в базе данных.
                } else
                {
                    model.Error = _localizer["User Blocked"];
                }

            } else
            {
                model.Error = _localizer["User Not Found"];
            }

            return model;
        }

        public async Task<string> GetRefreshedToken(string refreshToken)
        {
            string refreshedToken = "";

            User user = await _userManager.Users.Where(p => p.RefreshToken.Equals(refreshToken)).FirstOrDefaultAsync();

            if (user != null)
            {
                if (!user.LockoutEnabled)
                {

                    foreach (Claim claim in await _userManager.GetClaimsAsync(user))
                    {
                        if (claim.Type == ClaimTypes.Name || claim.Type == ClaimTypes.Email || claim.Type == ClaimTypes.Sid || claim.Type == ClaimsIdentity.DefaultRoleClaimType)
                        {
                            await _userManager.RemoveClaimAsync(user, claim);
                        }
                    }

                    //переопределяем claims, чтобы авторизация через токены и авторизация через кукисы обращалась к одним и тем же claims
                    await _userManager.AddClaimsAsync(user, await ClaimsSync.GetSyncedClaims(user, _userManager, user.Id));
                    refreshedToken = await _jwtGenerator.CreateToken(user, _userManager, user.Id); //получившийся токен
                    user = null;
                }
            }

            return refreshedToken;
        }
    }
}
