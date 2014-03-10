using System;
using System.Web.Mvc;
using System.Web.Http;

namespace RF.Sts.Auth
{
    public class StsAuthArea : AreaRegistration
    {
        public override string AreaName
        {
            get { return OverAreasMixProtectionModule.StsAreaName; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            var route = context.Routes.MapHttpRoute(name: this.AreaName, routeTemplate: this.AreaName + "/{controller}/{id}", defaults: new { id = RouteParameter.Optional });
            if (route.DataTokens == null)
                route.DataTokens = new System.Web.Routing.RouteValueDictionary();
            route.DataTokens["area"] = this.AreaName;
        }
    }

    public class SvcAuthArea : AreaRegistration
    {
        public override string AreaName
        {
            get { return OverAreasMixProtectionModule.SvcAreaName; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            var route = context.Routes.MapHttpRoute(name: this.AreaName, routeTemplate: this.AreaName + "/{*odataPath}", defaults: new { id = RouteParameter.Optional });
            if (route.DataTokens == null)
                route.DataTokens = new System.Web.Routing.RouteValueDictionary();
            route.DataTokens["area"] = this.AreaName;
        }
    }
}