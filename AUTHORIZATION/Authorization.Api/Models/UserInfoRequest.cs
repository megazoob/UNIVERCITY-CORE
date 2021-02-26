using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.Api.Models
{
    /// <summary>
    /// Модель для запроса информации о пользователе
    /// </summary>
    public class UserInfoRequest
    {
        [Required(ErrorMessage = "Email Required")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
