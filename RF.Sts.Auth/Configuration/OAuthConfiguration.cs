using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace RF.Sts.Auth.Configuration
{
    public class OAuthConfiguration : ConfigurationSection
    {
        //public const string IssuerUrl = "http://localhost:333";
        //public const string RelyingPartyRealm = "http://localhost:555/";
        //public const string RelyingPartySigningKey = "qqO5yXcbijtAdYmS2Otyzeze2XQedqy+Tp37wQ3sgTQ=";

   		public static readonly string ConfigurationSectionName = "rf.oauth";

		[ConfigurationProperty("sts", IsRequired=true)]
		public StsElement StsSettings
		{
			get
			{
				return (StsElement)base["sts"];
			}
			set
			{
				base["sts"] = value;
			}
		}

        [ConfigurationProperty("service")]
        public ServiceElement ServiceSettings
        {
            get
            {
                return (ServiceElement)base["service"];
            }
            set
            {
                base["service"] = value;
            }
        }

        [ConfigurationProperty("client")]
        public ClientElement ClientSettings
        {
            get
            {
                return (ClientElement)base["client"];
            }
            set
            {
                base["client"] = value;
            }
        }

        private static object _sync = new object();
        private static OAuthConfiguration _cnfg = null;
		public static OAuthConfiguration Configuration
		{
            get
            {
                if (_cnfg == null)
                    lock(_sync)
                        _cnfg = ConfigurationManager.GetSection(ConfigurationSectionName) as OAuthConfiguration;
                return _cnfg;
            }
		}
    }
}
