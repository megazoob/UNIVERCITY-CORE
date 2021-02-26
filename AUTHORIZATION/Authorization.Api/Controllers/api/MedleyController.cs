using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.Roles;

namespace Authorization.Api.Controllers.api
{
    /// <summary>
    /// Разное:
    /// список пользовательских ролей.
    /// </summary>
    //[Route("api/[controller]")]
    [ApiController]
    public class MedleyController : ControllerBase
    {
        /// <summary>
        /// Просто список строк - ролей пользователей.
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/getroleslist")]
        [Authorize(Policy = "ValidAccessToken", Roles = Roles.Admin)]
        public ICollection<string> GetRoles()
        {
            IList<string> roles = new List<string>();

            roles.Add(Roles.Admin);
            roles.Add(Roles.Employee);
            roles.Add(Roles.Student);
            roles.Add(Roles.WithoutRole);

            return roles;
        }

    }
}
