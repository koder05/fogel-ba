using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Services.Client;

namespace RF.WcfDS.OAuth
{
    public class ListenerDrivenBehavior : IOAuthBehavior
    {
        public Uri RegisterDataService(DataServiceContext ctx)
        {
            ctx.BaseUri = new Uri("http://localhost:4656");
            return ctx.BaseUri;
        }
    }
}
