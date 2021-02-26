using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Services.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "EmailRequired")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "LoginRequired")]
        [Display(Name = "Login")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "DateOfBirnRequired", AllowEmptyStrings = true)]
        [Display(Name = "DateOfBirn")]
        public DateTime DateOfBirn { get; set; }

        [Required(ErrorMessage = "PasswordRequired")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "PasswordLength", MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "PasswordRepeatRequired")]
        [Compare("Password", ErrorMessage = "PasswordsNotEquals")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "PasswordLength", MinimumLength = 6)]
        [Display(Name = "RepeatPassword")]
        public string PasswordConfirm { get; set; }
        public bool State { get; set; }
        public string Error { get; set; }
    }
}
