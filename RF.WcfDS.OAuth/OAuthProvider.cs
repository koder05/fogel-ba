using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Services.Client;

using RF.Sts.Auth.Configuration;

namespace RF.WcfDS.OAuth
{
    public static class OAuthProvider
    {
        private static readonly object sync = new object();
        public static IOAuthBehavior _behavior;

        public static IOAuthBehavior Behavior 
        {
            get
            {
                if (_behavior == null)
                    throw new InvalidOperationException("Не задан сервис поддержки OAuth.");
                return _behavior;
            }
            set
            {
                lock (sync)
                {
                    _behavior = value;
                }
            }
        }

        public static Uri RegisterDataService(DataServiceContext ctx)
        {
            if (_behavior == null)
                Behavior = Activator.CreateInstance(OAuthConfiguration.Configuration.ClientSettings.WcfDSBehaviorType) as IOAuthBehavior;

            return Behavior.RegisterDataService(ctx);
        }
    }
}
