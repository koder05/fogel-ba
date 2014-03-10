using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RF.Sts.Auth
{
    public static class TokensStore
    {
        private static readonly object sync = new object();
        private static ITokensStoreProvider _provider;

        public static ITokensStoreProvider StoreProvider
        {
            get
            {
                if (_provider == null)
                    throw new InvalidOperationException("Не задан провайдер билетов безопасности.");

                return _provider;
            }
            set
            {
                lock (sync)
                {
                    _provider = value;
                }
            }
        }
    }
}
