using Authorization.Services.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Services.Interfaces
{
    public interface IRegister
    {
        public Task<RegisterViewModel> Register(RegisterViewModel model, string role="");
    }
}
