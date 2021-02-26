using Authorization.Data.DataContext.SQLServer;
using Authorization.Data.Models;
using Authorization.Services.Interfaces;
using Authorization.Services.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Services.Services
{
    public class UserProfileManage : IUserProfileManage
    {
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer _localizer;
        private readonly IPasswordValidator<User> _passwordValidator;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ASPUserContext _db;

        public UserProfileManage(UserManager<User> userManager, IStringLocalizer localizer, 
                                IPasswordValidator<User> passwordValidator, IPasswordHasher<User> passwordHasher, ASPUserContext db)
        {
            _userManager = userManager;
            _localizer = localizer;
            _passwordValidator = passwordValidator;
            _passwordHasher = passwordHasher;
            _db = db;
        }

        /// <summary>
        /// Профиль пользователя.
        /// </summary>
        /// <param name="identificator"></param>
        /// <returns></returns>
        public async Task<UserProfileEditView> GetUserProfile(string identificator)
        {
            UserProfileEditView model = new UserProfileEditView();

            var user = _userManager.Users.Include(p => p.Profile).Where(p => p.Id.Equals(identificator)).FirstOrDefault();

            if (user != null)
            {
                model.Profile.Email = user.Email;
                model.Profile.UserName = user.UserName;
                model.Profile.DateOfBirn = user.DateOfBirn.Date;

                IList<string> roles = await _userManager.GetRolesAsync(user);
                model.Profile.Role = roles.FirstOrDefault();

                if (user.Profile != null)
                {
                    model.Profile.FirstName = user.Profile.FirstName;
                    model.Profile.SecondName = user.Profile.SecondName;
                    model.Profile.LastName = user.Profile.LastName;
                }
                model.Identificator = user.Id;
            }


            return model;
        }

        /// <summary>
        /// Список пользователей.
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<User>> GetUsersList()
        {
            return await _userManager.Users.Include(p => p.Profile).OrderBy(p => p.UserName).ToListAsync<User>();
        }

        /// <summary>
        /// Создает или изменяет профиль пользователя.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public async Task<UserProfileEditView> SaveOrCreateUserProfile(UserProfileEditView model)
        {

            if (!string.IsNullOrEmpty(model.Identificator))
            {
                if (_userManager.Users.Where(p => p.Id != model.Identificator
                                            && (p.UserName.Equals(model.Profile.UserName)
                                            || p.Email.Equals(model.Profile.Email))).Count() > 0)
                {
                    model.Error = _localizer["This Email Registered Already"];
                }
                else
                {
                    model.Error = "";
                    var user = _userManager.Users.Include(p => p.Profile).Where(p => p.Id.Equals(model.Identificator)).FirstOrDefault();
                    if (user != null)
                    {
                        user.UserName = model.Profile.UserName;
                        user.Email = model.Profile.Email;

                        var result = await _userManager.UpdateAsync(user);

                        if (result.Succeeded)
                        {
                            if (user.Profile == null)
                            {
                                UserProfile profile = new UserProfile();
                                profile.UserId = model.Identificator;
                                profile.FirstName = model.Profile.FirstName;
                                profile.LastName = model.Profile.LastName;


                                await _db.UserProfiles.AddAsync(profile);
                                await _db.SaveChangesAsync();
                            }
                            else
                            {
                                UserProfile profile = user.Profile;
                                profile.FirstName = model.Profile.FirstName;
                                profile.LastName = model.Profile.LastName;

                                _db.UserProfiles.Update(profile);
                                await _db.SaveChangesAsync();
                                
                            }

                        }
                        else
                        {
                            foreach (IdentityError item in result.Errors)
                            {
                                model.Error += item.Code + " " + item.Description + "\n";
                            }
                        }
                    }
                }
            }
            return model;
        }

        /// <summary>
        /// Изменяет пароль пользователя.
        /// </summary>
        /// <param name="userPassword"></param>
        /// <returns></returns>
        public async Task<UserPassword> UpdateUserPassword(UserPassword userPassword)
        {
            userPassword.Error = _localizer["Password Changes Successfully"];

            User user = await _userManager.FindByIdAsync(userPassword.Identificator);
            if (user != null)
            {
                IdentityResult result =
                     await _passwordValidator.ValidateAsync(_userManager, user, userPassword.Password);
                if (result.Succeeded)
                {
                    user.PasswordHash = _passwordHasher.HashPassword(user, userPassword.Password);
                    await _userManager.UpdateAsync(user);

                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        userPassword.Error += error.Code + " " + error.Description + "\n";
                    }
                }
            }

            return userPassword;
        }
    }
}
