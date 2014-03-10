using System;
using System.Configuration;

namespace RF.Sts.Auth.Configuration
{
    public class StsElement : ConfigurationElement
    {
        [ConfigurationProperty("url", IsRequired = true)]
        //[RegexStringValidator(@"http(s)?:\/\/[\w.]+(:(\d){2,})?\/(\w+\/)*")]
        public Uri IssuerUri
        {
            get
            {
                return (Uri)this["url"];
            }
            set
            {
                this["url"] = value;
            }
        }

        [ConfigurationProperty("symmetrickey")]
        public string SymmetricKey
        {
            get
            {
                return (string)this["symmetrickey"];
            }
            set
            {
                this["symmetrickey"] = value;
            }
        }

        [ConfigurationProperty("timeout")]
        public int TokenLifeTimeInSec
        {
            get
            {
                return (int)this["timeout"];
            }
            set
            {
                this["timeout"] = value;
            }
        }

        [ConfigurationProperty("authmode")]
        public string AuthenticationMode
        {
            get
            {
                return (string)this["authmode"];
            }
            set
            {
                this["authmode"] = value;
            }
        }
    }
}
