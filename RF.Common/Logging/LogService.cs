using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RF.Common.Logging
{
	public class LogService
	{
		private ILogLauncher _log;
		public LogService(ILogLauncher log)
		{
			_log = log;
		}
		
		public ILogger LogApp()
		{
			return _log.GetLogApp();
		}

		public ILogger LogUser()
		{
			return _log.GetLogUser(); 
		}

        public ILogger LogDb()
        {
            return _log.GetLogDb();
        }
	}
}
