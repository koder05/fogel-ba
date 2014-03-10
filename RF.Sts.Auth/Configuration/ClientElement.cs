using System;
using System.Configuration;
using System.ComponentModel;

using RF.Common.Security;

namespace RF.Sts.Auth.Configuration
{
    public class ClientElement : ConfigurationElement
    {
        [TypeConverter(typeof(TypeNameConverter))]
        [ConfigurationProperty("tokensstore")]
        private Type TokensStoreProviderType
        {
            get
            {
                return (Type)this["tokensstore"];
            }
            set
            {
                this["tokensstore"] = value;
            }
        }

        public ITokensStoreProvider TokensStoreProvider
        {
            get
            {
                return (ITokensStoreProvider)Activator.CreateInstance(this.TokensStoreProviderType);
            }
        }

        [TypeConverter(typeof(TypeNameConverter))]
        [ConfigurationProperty("wcfds")]
        public Type WcfDSBehaviorType
        {
            get
            {
                return (Type)this["wcfds"];
            }
            set
            {
                this["wcfds"] = value;
            }
        }

        [ConfigurationProperty("formsauth")]
        public string FormsAuthCookieName
        {
            get
            {
                return (string)this["formsauth"];
            }
            set
            {
                this["formsauth"] = value;
            }
        }
    }
}

