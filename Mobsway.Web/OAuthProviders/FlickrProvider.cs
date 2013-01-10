using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.AspNet;
using DotNetOpenAuth.AspNet.Clients;
using DotNetOpenAuth.OAuth;
using FlickrNet;
using Mobsway.Web.Helpers;

namespace Mobsway.Web.OAuthProviders
{
    public class FlickrProvider : IAuthenticationClient
    {

        private const string OAUTH_REQUEST_TOKEN = "flickr_oauth_request_token";
        private string _providerName = string.Empty, 
            _apiKey = string.Empty, 
            _sharedSecret = string.Empty;

        public FlickrProvider(string key, string secret):this("flickr", key, secret)
        {
        }

        public FlickrProvider(string providerName, string key, string secret)
        {
            _providerName = providerName;
            _apiKey = key;
            _sharedSecret = secret;
        }

        public string ProviderName
        {
            get { return _providerName; }
        }

        public void RequestAuthentication(HttpContextBase context, Uri returnUrl)
        {
            Flickr instance = FlickrManager.GetInstance(_apiKey, _sharedSecret);

            OAuthRequestToken token = instance.OAuthGetRequestToken(returnUrl.AbsoluteUri);

            var session = HttpContext.Current.Session;

            session.AddToSession<OAuthRequestToken>(OAUTH_REQUEST_TOKEN, token);

            string redirectUrl = instance.OAuthCalculateAuthorizationUrl(token.Token, AuthLevel.Write);

            context.Response.Redirect(redirectUrl);
        }

        public AuthenticationResult VerifyAuthentication(HttpContextBase context)
        {
            AuthenticationResult result = null;

            string verifier = context.Request.QueryString.Get("oauth_verifier");

            Flickr instance = FlickrManager.GetInstance(_apiKey, _sharedSecret);

            OAuthRequestToken request = HttpContext.Current.Session.GetFromSession<OAuthRequestToken>(OAUTH_REQUEST_TOKEN);

            var authToken = instance.OAuthGetAccessToken(request, verifier);

            if (authToken != null)
            {
                FlickrManager.OAuthToken = authToken.Token;
                FlickrManager.OAuthTokenSecret = authToken.TokenSecret;

                result = new AuthenticationResult(true);
            }
            else
            {
                result = new AuthenticationResult(false);
            }

            return result;
        }
    }
}