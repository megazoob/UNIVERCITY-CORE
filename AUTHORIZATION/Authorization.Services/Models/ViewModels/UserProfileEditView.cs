using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Services.Models.ViewModels
{
    public class UserProfileEditView
    {
        public UserProfileView Profile { get; set; }
        [Required]
        public string Identificator { get; set; }
        public string Error { get; set; }

        public UserProfileEditView()
        {
            Profile = new UserProfileView();
            Identificator = "";
            Error = "";
        }
    }
}
