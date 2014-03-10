using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace RF.Sts.Auth.Configuration
{
    public class ServiceElement : ConfigurationElement
    {
        [ConfigurationProperty("realm", IsRequired = true)]
        public Uri Realm
        {
            get
            {
                return (Uri)this["realm"];
            }
            set
            {
                this["realm"] = value;
            }
        }
    }
}
