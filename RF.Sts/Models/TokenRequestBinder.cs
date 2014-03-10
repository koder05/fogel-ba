using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RF.Sts
{
    public class TokenRequestBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}