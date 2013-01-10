using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.AspNet.Clients;
using DotNetOpenAuth.OAuth;

namespace Mobsway.Web.OAuthProviders
{
    public class ImgurProvider:OAuthClient
    {

        public static readonly ServiceProviderDescription ImgurServiceDescription;

        public ImgurProvider(string key, string secret)
            : base("imgur", ImgurProvider.ImgurServiceDescription, new SimpleConsumerTokenManager(key, secret, new AuthenticationOnlyCookieOAuthTokenManager()))
        {
        }

        static ImgurProvider()
        {
            ServiceProviderDescription service = new ServiceProviderDescription();
            service.RequestTokenEndpoint = new DotNetOpenAuth.Messaging.MessageReceivingEndpoint("http://www.flickr.com/services/oauth/request_token", DotNetOpenAuth.Messaging.HttpDeliveryMethods.PostRequest);
            service.AccessTokenEndpoint = new DotNetOpenAuth.Messaging.MessageReceivingEndpoint("http://www.flickr.com/services/oauth/access_token", DotNetOpenAuth.Messaging.HttpDeliveryMethods.PostRequest);
            ImgurProvider.ImgurServiceDescription = service;
        }

        public override void RequestAuthentication(HttpContextBase context, Uri returnUrl)
        {
            
        }

        protected override DotNetOpenAuth.AspNet.AuthenticationResult VerifyAuthenticationCore(DotNetOpenAuth.OAuth.Messages.AuthorizedTokenResponse response)
        {
            throw new NotImplementedException();
        }
    }
}