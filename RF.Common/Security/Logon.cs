using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RF.Common.Security
{
    public static class Logon
    {
        private static readonly object sync = new object();
        private static ILogonPage _page;

        public static ILogonPage Page
        {
            get
            {
                if (_page == null)
                    throw new InvalidOperationException("Не задана форма входа.");
                return _page;
            }
            set
            {
                lock (sync)
                {
                    _page = value;
                }
            }
        }
    }
}
