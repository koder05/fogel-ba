using System;
using System.Web.Mvc;
using System.Web.Http;

namespace RF.Sts
{
    public class BasicAuthArea : AreaRegistration
    {
        public const string Name = "basic";

        public override string AreaName
        {
            get { return Name; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            var route = context.Routes.MapHttpRoute(name: "BasicAuthArea", routeTemplate: "api/" + this.AreaName + "/{controller}/{id}", defaults: new { id = RouteParameter.Optional });
            if (route.DataTokens == null)
                route.DataTokens = new System.Web.Routing.RouteValueDictionary();
            route.DataTokens["area"] = this.AreaName;
        }
    }
}