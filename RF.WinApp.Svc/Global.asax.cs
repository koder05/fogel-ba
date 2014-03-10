using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http.Controllers;
using System.Web.Http.OData.Routing;
using System.Web.Http.OData.Routing.Conventions;

using RF.Common.DI;
using RF.Common.Transactions;
using Ms.Unity._2;

namespace RF.WinApp.Svc
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            //AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }

        public override void Init()
        {
            base.Init();

            IContainerWrapper ioc = new UnityContainerWrapper();
            IoC.InitializeWith(ioc);
            Transactions.Service = IoC.Resolve<TransactionService>();
            GlobalConfiguration.Configuration.DependencyResolver = new RF.WebApp.DependencyResolver(ioc);
        }
    }
}