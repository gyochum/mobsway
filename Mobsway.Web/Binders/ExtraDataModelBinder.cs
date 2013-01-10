using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Mobsway.Business.Entity;
using Newtonsoft.Json;

namespace Mobsway.Web.Binders
{
    public class ExtraDataModelBinder:IModelBinder
    {

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (controllerContext.HttpContext.Request.IsAuthenticated)
            {
                var cookie = controllerContext.HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];

                if (cookie == null)
                    return null;

                var decryptedTicket = FormsAuthentication.Decrypt(cookie.Value);

                if (!string.IsNullOrWhiteSpace(decryptedTicket.UserData))
                {
                    return JsonConvert.DeserializeObject<ExtraData>(decryptedTicket.UserData);
                }
            }

            return null;
        }

    }
}