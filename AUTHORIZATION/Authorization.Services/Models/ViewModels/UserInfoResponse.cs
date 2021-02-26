using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Services.Models.ViewModels
{
    /// <summary>
    /// Модель информации о пользователе
    /// </summary>
    public class UserInfoResponse
    {
        [Display(Name = "Sid")]
        public string Identificator { get; set; }

        [Display(Name = "Login")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Date Of Birn")]
        public DateTime DateOfBirn { get; set; }

        [Display(Name = "Roles")]
        public string Roles { get; set; }
        public string Error { get; set; }
    }
}
