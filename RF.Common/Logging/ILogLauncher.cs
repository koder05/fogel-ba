using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RF.Common.Logging
{
	public interface ILogLauncher
	{
		ILogger GetLogApp();
		ILogger GetLogDb();
		ILogger GetLogUser();
		ILogger GetLogErr();
	}
}
