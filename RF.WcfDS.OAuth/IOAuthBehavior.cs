using System;
using System.Data.Services.Client;

namespace RF.WcfDS.OAuth
{
    public interface IOAuthBehavior
    {
        Uri RegisterDataService(DataServiceContext ctx);
    }
}
