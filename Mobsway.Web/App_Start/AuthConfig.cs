using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using Mobsway.Web.Models;
using Mobsway.Web.OAuthProviders;

namespace Mobsway.Web
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            OAuthWebSecurity.RegisterMicrosoftClient(
                clientId: "000000004C0DF23F",
                clientSecret: "ytkIexLUw7pCFm7G81jYHJ0jAfxi0dx9",
                displayName: "Mobsway"
                );

            OAuthWebSecurity.RegisterTwitterClient(
                consumerKey: ConfigurationManager.AppSettings.Get("twitterConsumerKey"),
                consumerSecret: ConfigurationManager.AppSettings.Get("twitterConsumerSecret"));

            OAuthWebSecurity.RegisterFacebookClient(
                appId: ConfigurationManager.AppSettings.Get("facebookAppId"),
                appSecret: ConfigurationManager.AppSettings.Get("facebookAppSecret"));

            OAuthWebSecurity.RegisterGoogleClient();

            //TODO: register custom OAUTH image providers
            OAuthWebSecurity.RegisterClient(new FlickrProvider("eb41784b44fe8dd2562b37b438a5a32f", "970bd436007145bf"));
            
        }
    }
}
