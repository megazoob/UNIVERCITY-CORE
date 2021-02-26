using Authorization.Data.Models;
using Authorization.Services.Interfaces;
using Users.Roles;
using Authorization.Services.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Authorization.Services.Services
{
    public class RegisterUser : IRegister
    {
        IStringLocalizer _localizer;
        UserManager<User> _userManager;
        RoleManager<IdentityRole> _roleManager;

        public RegisterUser(IStringLocalizer localizer, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _localizer = localizer;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<RegisterViewModel> Register(RegisterViewModel model, string role="")
        {
           
            model.Error = "";
            if (!model.State)
            {
                model.State = true;
            }

            if (_userManager.Users.Where(p => p.UserName.Equals(model.UserName)).Count() > 0)
            {
                model.State = false;
                model.Error = _localizer["This User Name Registered Already"];
                return model;
            }

            if (_userManager.Users.Where(p => p.Email.Equals(model.Email)).Count() > 0)
            {
                model.State = false;
                model.Error = _localizer["This Email Registered Already"];
                return model;
            }

            User user = new User { Email = model.Email, UserName = model.UserName, 
                                   DateOfBirn = model.DateOfBirn, EmailConfirmed = true,
                                   PhoneNumberConfirmed = true, LockoutEnabled = false }; 

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {

                try
                {
                    user = await _userManager.FindByNameAsync(model.UserName);

                    user.LockoutEnabled = false;

                    await _userManager.UpdateAsync(user);



                } catch (Exception e)
                {

                }
                

                

                if (_roleManager.Roles.Count() == 0) //нет ни одной роли, значит создаем администратора
                {
                    await _roleManager.CreateAsync(new IdentityRole(Roles.Admin));
                    await _userManager.AddToRoleAsync(user, Roles.Admin); //добавляем админа

                }
                else
                {
                    var withoutRole = await _roleManager.FindByNameAsync(Roles.WithoutRole);
                    if (withoutRole == null)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(Roles.WithoutRole));
                    }
                    var employeeRole = await _roleManager.FindByNameAsync(Roles.Employee);
                    if (employeeRole == null)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(Roles.Employee));
                    }
                    var studentRole = await _roleManager.FindByNameAsync(Roles.Student);
                    if (studentRole == null)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(Roles.Student));
                    }
                    if (!string.IsNullOrEmpty(role))
                    {
                        var xRole = await _roleManager.FindByNameAsync(role);
                        if (xRole == null)
                        {
                            await _roleManager.CreateAsync(new IdentityRole(role));
                        }
                        await _userManager.AddToRoleAsync(user, role);
                    } else
                    {
                        await _userManager.AddToRoleAsync(user, Roles.WithoutRole); //добавляем пользователя без роли
                    }
                    
                }

                model.State = true;

            }
            else
            {
                foreach (var err in result.Errors)
                {
                    model.State = false;
                    model.Error += err.Description + "\n";
                }
            }

            return model;
        }
    }
}
