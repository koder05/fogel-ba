using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RF.Sts.Auth
{
    public interface ITokensStoreProvider
    {
        T TakeToken<T>(Uri realm);
        void PutToken<T>(Uri realm, T rawToken);
    }
}
