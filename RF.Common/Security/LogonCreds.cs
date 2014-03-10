using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RF.Common.Security
{
    public class LogonCreds
    {
        public string Name { get; set; }
        public string Psw { get; set; }
        public bool IsSuccessful { get; set; }
        public bool IsCanceled { get; set; }
    }
}
