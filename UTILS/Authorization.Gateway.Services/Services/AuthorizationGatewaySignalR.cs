using Authorization.Gateway.Services.Inerfaces;
using Authorization.Gateway.Services.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Gateway.Services.Services
{
    /// <summary>
    /// Реализация шлюза SignalR.
    /// </summary>
    public class AuthorizationGatewaySignalR : IAuthorizationGateway
    {
        UrlSettings _urls; //адреса методов авторизации.
        HubConnection _hubConnection;

        public AuthorizationGatewaySignalR(UrlSettings urls)
        {
            _urls = urls;
        }

        /// <summary>
        /// Возвращает результат проверки пользователя с token в ролях roles.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public async Task<Boolean> AuthorizationWithRolesAsync(string token, string roles)
        {
            CultureInfo culture = CultureInfo.CurrentCulture;
            
            token = token.Replace("Bearer ", "");

            string[] requestMethodArray = _urls.IsUserInRoleUrl.Split("/");
            string requestMethod = "";
            requestMethod = requestMethodArray[requestMethodArray.Length - 1];

            string requestAddress = _urls.IsUserInRoleUrl
                                   .Replace(_urls.IsUserInRoleUrl.Split("/")[_urls.IsUserInRoleUrl.Split("/").Length - 1], "");

            if (requestAddress.EndsWith("/"))
            {
                requestAddress = requestAddress.Remove(requestAddress.Length - 1, 1);
            }

            _hubConnection = new HubConnectionBuilder()
                    .WithUrl(requestAddress)
                    .WithAutomaticReconnect()
                    .Build();

            await _hubConnection.StartAsync();

            Boolean ret = await _hubConnection.InvokeAsync<Boolean>(requestMethod, token, roles, culture.Name);

            await _hubConnection.StopAsync();

            return ret;
        }

        /// <summary>
        /// Возвращает результат проверки пользователя с token - авторизован ли он.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Boolean> IsAuthorizedAsync(string token)
        {
            CultureInfo culture = CultureInfo.CurrentCulture;

            token = token.Replace("Bearer ", "");

            string[] requestMethodArray = _urls.IsUserAuthorizedUrl.Split("/");
            string requestMethod = "";
            requestMethod = requestMethodArray[requestMethodArray.Length - 1];

            string requestAddress = _urls.IsUserAuthorizedUrl
                                   .Replace(_urls.IsUserAuthorizedUrl.Split("/")[_urls.IsUserAuthorizedUrl.Split("/").Length - 1], "");

            if (requestAddress.EndsWith("/"))
            {
                requestAddress = requestAddress.Remove(requestAddress.Length - 1, 1);
            }

            _hubConnection = new HubConnectionBuilder()
                    .WithUrl(requestAddress)
                    .WithAutomaticReconnect()
                    .Build();

            await _hubConnection.StartAsync();

            Boolean ret = await _hubConnection.InvokeAsync<Boolean>(requestMethod, token, culture.Name);

            await _hubConnection.StopAsync();

            return ret;
        }
    }
}
