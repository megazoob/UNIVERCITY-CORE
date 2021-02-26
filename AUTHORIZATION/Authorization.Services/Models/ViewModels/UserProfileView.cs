using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Services.Models.ViewModels
{
    public class UserProfileView
    {
        [Required(ErrorMessage = "Login Required")]
        [Display(Name = "Login")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email Required")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Name")]
        public string FirstName { get; set; }
        [Display(Name = "Second Name")]
        public string SecondName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Role { get; set; }
        public DateTime DateOfBirn { get; set; }

        public UserProfileView()
        {
            UserName = "";
            Email = "";
            FirstName = "";
            SecondName = "";
            LastName = "";
            Role = "";
        }
    }
}
