using Authorization.Data.Models;
using Authorization.Services.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Services.Interfaces
{
    public interface IUserProfileManage
    {
        /// <summary>
        /// Список пользователей.
        /// </summary>
        /// <returns></returns>
        public Task<ICollection<User>> GetUsersList();

        /// <summary>
        /// Профиль на редактирование.
        /// </summary>
        /// <param name="identificator"></param>
        /// <returns></returns>
        public Task<UserProfileEditView> GetUserProfile(string identificator);
        
        /// <summary>
        /// Создает или сохраняет измененный профиль пользователя.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="currentUserSid"></param>
        /// <returns></returns>
        public Task<UserProfileEditView> SaveOrCreateUserProfile(UserProfileEditView model);

        /// <summary>
        /// Изменяет пароль пользователя.
        /// </summary>
        /// <param name="userPassword"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public Task<UserPassword> UpdateUserPassword(UserPassword userPassword);
        
    }
}
