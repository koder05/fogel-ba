using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace RF.Sts.Auth
{
    public class InMemoryTokensStoreProvider : ITokensStoreProvider
    {
        private Dictionary<Uri, object> _store = new Dictionary<Uri, object>();

        public T TakeToken<T>(Uri realm)
        {
            if (_store.ContainsKey(realm))
                return (T)_store[realm];

            return default(T);
        }

        public void PutToken<T>(Uri realm, T rawToken)
        {
            lock (_store)
            {
                if (!_store.ContainsKey(realm))
                    _store.Add(realm, rawToken);
                else _store[realm] = rawToken;
            }
        }
    }
}
