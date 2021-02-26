using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Data.Models
{
    /// <summary>
    /// Расширение Identity User.
    /// </summary>
    /// <remarks>
    /// Добавлено свойство DateOfBirn
    /// </remarks>
    public class User : IdentityUser
    {
        [Required]
        [Display(Name = "Date Of Birn")]
        public DateTime DateOfBirn { get; set; }

        /// <summary>
        /// токен восстановления.
        /// </summary>
        public string RefreshToken { get; set; }

        public UserProfile Profile { get; set; }

        /// <summary>
        /// Проверка, активен ли пользователь
        /// </summary>
        /// <returns></returns>
        public Boolean CheckUserValid()
        {

            if (this.LockoutEnabled)
            {
                return true;
            }

            return false;
        }

        public User()
        {

        }
    }
}
