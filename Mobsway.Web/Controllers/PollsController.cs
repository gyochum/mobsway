using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Web.WebPages.OAuth;
using Mobsway.Data.Persistence;
using Mobsway.Web.Models;
using Raven.Client;
using WebMatrix.WebData;
using Mobsway.Web.Helpers;
using Mobsway.Business.Entity;
using Mobsway.Utilities;
using DotNetOpenAuth.AspNet;
using System.Web.Routing;

namespace Mobsway.Web.Controllers
{
    public class PollsController : BaseController
    {

        private IPollRepository repository = null;

        public PollsController(IPollRepository r)
        {
            repository = r;
        }

        #region authorize action
        public ActionResult Authorize()
        {
            var result = OAuthWebSecurity.VerifyAuthentication();

            if (result.IsSuccessful)
            {
                //TODO: grab email and name from results.ExtraData : based on provider
                string name = string.Empty, email = string.Empty;
                IDictionary<string, string> extraData = result.ExtraData;

                switch (result.Provider.ToLower())
                {
                    case "facebook":
                        name = result.UserName;

                        break;
                    case "twitter":
                        name = extraData["name"];

                        break;
                    case "yahoo":
                        email = extraData["email"];
                        name = extraData["fullName"];

                        break;
                    case "google":
                        email = result.UserName;

                        break;
                }

                ExtraData extraUserData = new ExtraData()
                {
                    Id = string.Format("{0}/{1}", result.Provider, result.UserName),
                    Email = email,
                    Name = name
                };

                //create forms authentication ticket
                Response.SetAuthCookie<ExtraData>(string.Format("{0}/{1}", result.Provider, result.UserName), false, extraUserData);

                return RedirectToAction("Index");
            }
            else
            {
                //provide error message

                //take user back to login page
                return RedirectToAction("Login", "Account");
            }
        } 
        #endregion

        #region index action
        [Authorize]
        public ActionResult Index(ExtraData user)
        {
            PollViewModel model = new PollViewModel();

            if (user != null)
            {
                model.Polls = repository.GetUserPolls(user.Id);
            }

            return View(model);
        } 
        #endregion

        #region [ create poll ]
        [HttpGet, Authorize]
        public ActionResult Create()
        {
            return View(new PollViewModel());
        }

        [HttpPost, Authorize]
        public ActionResult Create(PollViewModel model, ExtraData user)
        {
            if (ModelState.IsValid)
            {
                Poll poll = new Poll();
                
                poll.CreatedBy = user.Id;
                poll.PollNumber = KeyGenerator.GetUniqueKey();
                poll.CreatedDate = DateTime.Today;

                poll.HashTag = model.Poll.HashTag;
                poll.StartDate = model.Poll.StartDate;
                poll.EndDate = model.Poll.EndDate;
                poll.Description = model.Poll.Description;
                poll.IsActive = model.Poll.IsActive;

                repository.Save(poll);

                if (model.PollHasOptions)
                {
                    return RedirectToAction("Pics", new { id = poll.PollNumber });
                }
                else
                {
                    return RedirectToAction("finish", new { id = poll.PollNumber });
                }
            }

            return View(model);
        }
        #endregion

        #region pics action
        [HttpGet, Authorize]
        public ActionResult Pics(string id)
        {
            PollViewModel model = new PollViewModel();

            model.Poll = repository.GetPoll(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Pics(PollViewModel model)
        {
            if (model.Poll != null)
            {
                bool providerFound = false;
                AuthenticationClientData provider = null;

                switch (model.Poll.ImageProvider)
                {
                    case ImageProviderTypes.Flickr:
                        providerFound = OAuthWebSecurity.TryGetOAuthClientData("flickr", out provider);

                        break;
                    case ImageProviderTypes.Imgur:

                        break;
                    case ImageProviderTypes.None:
                        break;
                }

                if (providerFound && provider != null)
                {
                    RouteValueDictionary param = new RouteValueDictionary()
                    {
                        {"id", model.Poll.PollNumber},
                        {"_provider_", provider.AuthenticationClient.ProviderName}
                    };
                    

                    Uri returnUrl = new Uri(Url.Action("Options", "Polls", param, "http", Url.RequestContext.HttpContext.Request.Url.Host));


                    provider.AuthenticationClient.RequestAuthentication(this.HttpContext, returnUrl);
                }
                else
                {
                    return RedirectToAction("Options", new { id = model.Poll.PollNumber });
                }
            }

            return View(model);
        }
        #endregion

        #region Options action
        [HttpGet, Authorize]
        public ActionResult Options(string id)
        {
            PollViewModel model = new PollViewModel();

            if (Request.QueryString.Get("oauth_verifier") != string.Empty)
            {
                string providerName = Request.QueryString.Get("_provider_");

                bool providerFound = false;
                AuthenticationClientData provider = null;

                providerFound = OAuthWebSecurity.TryGetOAuthClientData(providerName, out provider);

                //verify the oauth request
                var result = provider.AuthenticationClient.VerifyAuthentication(ControllerContext.HttpContext);

                if (!result.IsSuccessful)
                {
                    //TODO: figure out where to take the user
                }
            }
            model.Poll = repository.GetPoll(id);

            return View(model);
        }
        #endregion

    }
}
