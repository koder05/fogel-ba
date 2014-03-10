using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RF.Common.Logging
{
    public static class Logs
    {
        public static LogService Service { get; set; }

        public static ILogger LogApp
        {
            get
            {
                if (Service != null)
                    return Service.LogApp();
                return new EmptyLogger();
            }
        }

        public static ILogger LogUser
        {
            get
            {
                if (Service != null)
                    return Service.LogUser();
                return new EmptyLogger();

            }
        }

        public static ILogger LogDb
        {
            get
            {
                if (Service != null)
                    return Service.LogDb();
                return new EmptyLogger();

            }
        }
    }
}
