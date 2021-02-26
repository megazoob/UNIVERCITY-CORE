using Authorization.Api.Models;
using Authorization.Data.Models;
using Authorization.Services.Interfaces;
using Authorization.Services.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Users.Roles;

namespace Authorization.Api.Controllers.api
{
    /// <summary>
    /// Управление профилем пользовтеля:
    /// редактирование, просмотр.
    /// </summary>
    //[Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class ProfileManageController : ControllerBase
    {
        IUserProfileManage _profile;
        IStringLocalizer _localizer;

        public ProfileManageController(IUserProfileManage profile, IStringLocalizer localizer)
        {
            _profile = profile;
            _localizer = localizer;
        }

        /// <summary>
        /// Профиль пользователея на редактирование
        /// </summary>
        /// <param name="identificator"></param>
        /// <returns></returns>
        [HttpGet("api/getuserprofile")]
        [HttpPost("api/getuserprofile")]
        [Authorize(Policy = "ValidAccessToken", Roles = Roles.Admin)]
        public async Task<UserProfileEditView> GetUserProfile(string identificator)
        {
            return await _profile.GetUserProfile(identificator);
        }

        /// <summary>
        /// Создание или изменение профиля пользовтаеля
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("api/saveuserprofile")]
        [Authorize(Policy = "ValidAccessToken", Roles = Roles.Admin)]
        public async Task<UserProfileEditView> CreateOrUpdateUserProfile(UserProfileEditView model, string culture="ru")
        {
            if (ModelState.IsValid)
            {
                return await _profile.SaveOrCreateUserProfile(model);
            } else
            {
                model.Error = _localizer["Model State Is Invalid"];
                return model;
            }
        }
    }
}
