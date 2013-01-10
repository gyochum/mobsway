using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FlickrNet;

namespace Mobsway.Web.OAuthProviders
{
    public class FlickrManager
    {
        private static string _apiKey = string.Empty, _sharedSecret = string.Empty;

        public static Flickr GetInstance(string key, string secret)
        {
            _apiKey = key;
            _sharedSecret = secret;

            return new Flickr(_apiKey, _sharedSecret);
        }

        public static Flickr GetAuthInstance()
        {
            var f = new Flickr(_apiKey, _sharedSecret);
            f.OAuthAccessToken = OAuthToken;
            f.OAuthAccessTokenSecret = OAuthTokenSecret;
            return f;
        }

        public static string OAuthToken
        {
            get
            {
                if (!HttpContext.Current.Request.Cookies.AllKeys.Contains("OAuthToken"))
                {
                    return null;
                }
                return HttpContext.Current.Request.Cookies["OAuthToken"].Value;
            }
            set
            {
                HttpContext.Current.Response.AppendCookie(new HttpCookie("OAuthToken", value));
            }
        }

        public static string OAuthTokenSecret
        {
            get
            {
                if (!HttpContext.Current.Request.Cookies.AllKeys.Contains("OAuthTokenSecret"))
                {
                    return null;
                }
                return HttpContext.Current.Request.Cookies["OAuthTokenSecret"].Value;
            }
            set
            {
                HttpContext.Current.Response.AppendCookie(new HttpCookie("OAuthTokenSecret", value));
            }
        }
    }
}