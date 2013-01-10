using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Newtonsoft.Json;

namespace Mobsway.Web.Helpers
{
    public static class ExtensionMethods
    {

        public static int SetAuthCookie<T>(this HttpResponseBase response, string name, bool rememberMe, T extraData)
        {
            //create a default ticket in order to get settings from config file
            var cookie = FormsAuthentication.GetAuthCookie(name, rememberMe);
            var ticket = FormsAuthentication.Decrypt(cookie.Value);

            var newTicket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration, ticket.IsPersistent, extraData.ToJson(), ticket.CookiePath);
            var encTicket = FormsAuthentication.Encrypt(newTicket);

            cookie.Value = encTicket;

            response.Cookies.Add(cookie);

            return encTicket.Length;
        }

        public static string ToJson(this Object item)  
        {
            if (item != null)
                return JsonConvert.SerializeObject(item);

            return string.Empty;
        }

        public static void AddToSession<T>(this HttpSessionState session, string name, T item)
        {
            if (HttpContext.Current != null)
            {
                T existingItem = GetFromSession<T>(session, name);

                if (existingItem != null)
                {
                    HttpContext.Current.Session[name] = item;
                }
                else
                {
                    HttpContext.Current.Session.Add(name, item);
                }
            }
        }

        public static T GetFromSession<T>(this HttpSessionState session, string name)
        {
            T result = default(T);

            if (HttpContext.Current != null)
            {
                result = (T)HttpContext.Current.Session[name]; 
            }

            return result;
        }

    }
}