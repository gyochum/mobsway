using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mobsway.Data.Persistence;
using Raven.Client.Document;

namespace Mobsway.Web.Controllers
{
    public class RavenDbControllerFactory : DefaultControllerFactory
    {

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
                return base.GetControllerInstance(requestContext, controllerType);

            var documentStore = MvcApplication.DataStore as DocumentStore;
            var dbRepository = new RavenDbRepository(documentStore);

            return Activator.CreateInstance(controllerType, dbRepository) as IController;
        }

    }
}