using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mobsway.Business.Entity;
using Mobsway.Web.Binders;

namespace Mobsway.Web
{
    public class ModelBinderConfig
    {

        public static void ConfigureModelBinders()
        {
            ModelBinders.Binders.Add(typeof(ExtraData), new ExtraDataModelBinder());
        }

    }
}