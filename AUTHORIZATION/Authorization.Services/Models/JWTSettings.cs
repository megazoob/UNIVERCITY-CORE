using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.Services.Models
{
    /// <summary>
    /// Настройки токенов из appsettings.json.
    /// </summary>
    public class JWTSettings
    {
        /// <summary>
        /// Ключ токена.
        /// </summary>
        public string TokenKey { get; set; }
        /// <summary>
        /// Время жизни токена в днях.
        /// </summary>
        public string JWTLifeDays { get; set; }
    }
}
