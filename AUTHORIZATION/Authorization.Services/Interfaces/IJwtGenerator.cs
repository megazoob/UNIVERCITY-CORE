using Authorization.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Services.Interfaces
{
    public interface IJwtGenerator
    {

        Task<string> CreateToken(User user, UserManager<User> userManager, string sid);

    }
}
