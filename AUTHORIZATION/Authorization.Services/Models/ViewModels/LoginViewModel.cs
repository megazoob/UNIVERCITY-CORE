using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Services.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "LoginRequired")]
        [Display(Name = "Login")]
        public string UserName { get; set; } = "";

        [Required(ErrorMessage = "PasswordRequired")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "PasswordLength", MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; } = "";

        [Display(Name = "RememberMe")]
        public bool RememberMe { get; set; } = true;

        public string ReturnUrl { get; set; } = "";
        public string Token { get; set; } = "";
        public string RefreshToken { get; set; } = "";
        public string Error { get; set; } = "";
    }
}
