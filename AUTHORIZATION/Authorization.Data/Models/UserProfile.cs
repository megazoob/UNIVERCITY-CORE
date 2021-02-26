using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Data.Models
{
    /// <summary>
    /// Профиль пользователя.
    /// </summary>
    public class UserProfile
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Second Name")]
        public string SecondName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public UserProfile()
        {
            FirstName = "";
            LastName = "";
        }
    }
}
