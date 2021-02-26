using Authorization.Data.Models;
using Authorization.Services.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Services.Interfaces
{
    public interface ILogin
    {
        public User currentUser { get; set; }

        public Task<LoginViewModel> Login(LoginViewModel model);

        public Task<string> GetRefreshedToken(string refreshToken);
    }
}
