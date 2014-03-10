using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RF.Common.Security
{
    public interface ILogonPage
    {
        LogonCreds GetLogin(Exception showReason);
    }
}
