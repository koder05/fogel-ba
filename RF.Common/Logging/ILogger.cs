using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RF.Common.Logging
{
	public interface ILogger
	{
		void Log(object o, string message, string eventType);
	}

    public class EmptyLogger : ILogger
    {
        public void Log(object o, string message, string eventType)
        {
            
        }
    }
}
