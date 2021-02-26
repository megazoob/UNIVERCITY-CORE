using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Services.Models.ViewModels
{
    public class UserPassword
    {
        [Required(ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "Password Length", MinimumLength = 6)]
        [Display(Name = "New Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password Repeat Required")]
        [Compare("Password", ErrorMessage = "Passwords Not Equals")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "Password Length", MinimumLength = 6)]
        [Display(Name = "Repeat Password")]
        public string PasswordConfirm { get; set; }

        public string Error { get; set; }

        [Required(ErrorMessage = "Identificator Required")]
        public string Identificator { get; set; }
    }
}
