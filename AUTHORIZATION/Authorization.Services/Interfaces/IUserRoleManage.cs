using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Services.Interfaces
{
    /// <summary>
    /// Управление ролями пользователя.
    /// </summary>
    public interface IUserRoleManage
    {
        /// <summary>
        /// В случае успеха возвращает "true".
        /// </summary>
        /// <param name="identificator"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public Task<string> AddRoleToUser(string identificator, string role);

        /// <summary>
        /// В случае успеха возвращает "true".
        /// </summary>
        /// <param name="identificator"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public Task<string> RemoveRoleFromUser(string identificator, string role);

        /// <summary>
        /// В случае успеха возвращает "true".
        /// </summary>
        /// <param name="identificator"></param>
        /// <param name="restore"></param>
        /// <returns></returns>
        public Task<string> UpdateUserStatus(string identificator, Boolean restore);
    }
}
