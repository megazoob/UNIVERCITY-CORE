using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.Gateway.Services.Models
{
    public class UrlSettings
    {
        public string IsUserInRoleUrl { get; set; }
        public string IsUserAuthorizedUrl { get; set; }
    }
}
