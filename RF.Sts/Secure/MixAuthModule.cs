using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;

namespace RF.Sts.Secure
{
	public class MixAuthModule : IHttpModule
	{
		FormsAuthenticationDisabler _formsAuthenticationDisabler = new FormsAuthenticationDisabler();

		#region IHttpModule Members

		public void Dispose()
		{
			_formsAuthenticationDisabler.Dispose();
		}

		public void Init(HttpApplication context)
		{
			_formsAuthenticationDisabler.Init(context);
		}
		#endregion

	}
}
