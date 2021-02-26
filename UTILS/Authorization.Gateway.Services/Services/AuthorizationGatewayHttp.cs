using Authorization.Gateway.Services.Inerfaces;
using Authorization.Gateway.Services.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Gateway.Services.Services
{
    public class AuthorizationGatewayHttp : IAuthorizationGateway
    {
        UrlSettings _urls;

        public AuthorizationGatewayHttp(UrlSettings urls)
        {
            _urls = urls;
        }

        public async Task<bool> AuthorizationWithRolesAsync(string token, string roles)
        {

            if (!token.Contains ("Bearer"))
            {
                token = "Bearer " + token;
            }

            string url = _urls.IsUserInRoleUrl;
            if (url.EndsWith("/"))
            {
                url = url.Remove(url.Length - 1, 1);
            }

            var client = new HttpClient();

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url + "?role=" + Uri.EscapeDataString(roles) + "&culture=" + CultureInfo.CurrentCulture.Name),
                Headers = {
                                { HttpRequestHeader.Authorization.ToString(), token },
                          }
            };
            //httpRequestMessage.Headers.Add("custom-authorization", "Authorization.Gateway");

            HttpResponseMessage result = new HttpResponseMessage();
            try
            {
                result = await client.SendAsync(httpRequestMessage);
            } catch (Exception e)
            {
                result = null;
            }

            Boolean ret = false;

            if (result == null)
            {
                return false;
            } else
            {
                try
                {
                  string content = await result.Content.ReadAsStringAsync();
                    if (content.ToLower().Equals("true"))
                        ret = true;
                } catch (Exception e)
                {
                    return false;
                }
            }

            return ret;
        }

        public async Task<bool> IsAuthorizedAsync(string token)
        {
            if (!token.Contains("Bearer"))
            {
                token = "Bearer " + token;
            }

            string url = _urls.IsUserAuthorizedUrl;
            if (url.EndsWith("/"))
            {
                url = url.Remove(url.Length - 1, 1);
            }

            var client = new HttpClient();

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url + "?culture=" + CultureInfo.CurrentCulture.Name),
                Headers = {
                                { HttpRequestHeader.Authorization.ToString(), token },
                          }
            };
            //httpRequestMessage.Headers.Add("custom-authorization", "Authorization.Gateway");

            HttpResponseMessage result = new HttpResponseMessage();
            try
            {
                result = await client.SendAsync(httpRequestMessage);
            }
            catch (Exception e)
            {
                result = null;
            }

            Boolean ret = false;

            if (result == null)
            {
                return false;
            }
            else
            {
                try
                {
                    string content = await result.Content.ReadAsStringAsync();
                    if (content.ToLower().Equals("true"))
                        ret = true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            return ret;
        }
    }
}
