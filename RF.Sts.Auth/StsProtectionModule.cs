using System;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Web.Routing;
using System.Configuration;
using System.Web.Configuration;

using RF.Common.Http;
using RF.Sts.Auth.Configuration;

namespace RF.Sts.Auth
{
    public class StsProtectionModule : IHttpModule
    {
        public const string BasicAuthenticationMode = "basic";
        public const string WinAuthenticationMode = "win";

        private FormsAuthenticationModule _formsAuthenticationModule;
        private MethodInfo _formsAuthenticationModuleOnEnter;
        private MethodInfo _formsAuthenticationModuleOnLeave;

        public void Dispose()
        {
            _formsAuthenticationModule.Dispose();
            _formsAuthenticationModule = null;
            GC.SuppressFinalize(this);
        }

        public void Init(HttpApplication context)
        {
            _formsAuthenticationModule = new FormsAuthenticationModule();

            // используется блок try-catch, так как при инициализации _formsAuthenticationModule могут возникнуть всякие exception'ы (например, при попытке его подцепиться ко всяким событиям application'а, context'а или request'а).
            try
            {
                using (HttpApplication fakeApplication = new HttpApplication())
                {
                    _formsAuthenticationModule.Init(fakeApplication);
                }
            }
            catch (Exception)
            {
            }

            // using reflection - need FULL TRUST
            Type t = _formsAuthenticationModule.GetType();

            _formsAuthenticationModuleOnEnter = t.GetMethod("OnEnter", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(Object), typeof(EventArgs) }, null);
            _formsAuthenticationModuleOnLeave = t.GetMethod("OnLeave", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(Object), typeof(EventArgs) }, null);

            if (_formsAuthenticationModuleOnEnter == null || _formsAuthenticationModuleOnLeave == null)
                throw new Exception("Unable to get all required FormsAuthenticationModule entrypoints using reflection.");

            context.AuthenticateRequest += new EventHandler(OnAuthenticateRequest);
            context.PostAuthenticateRequest += new EventHandler(OnPostAuthenticateRequest);
            context.EndRequest += new EventHandler(OnEndRequest);
        }

        internal void OnEndRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpContext context = app.Context;

            if (context.Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                _formsAuthenticationModuleOnLeave.Invoke(_formsAuthenticationModule, new Object[] { sender, e });
            }
        }

        internal void OnPostAuthenticateRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpContext context = app.Context;

            string authmode = OAuthConfiguration.Configuration.StsSettings.AuthenticationMode;
            if (string.IsNullOrEmpty(authmode))
                authmode = StsProtectionModule.BasicAuthenticationMode;

            if (context.User == null || context.User.Identity.IsAuthenticated == false)
            {
                if (authmode == StsProtectionModule.BasicAuthenticationMode)
                {
                    var authHeaders = context.Request.Headers.GetValues(HttpRequestHeader.Authorization.GetName());
                    if (authHeaders != null && authHeaders.Length > 0)
                    {
                        string[] parts = authHeaders[0].Split(' ');

                        if (parts.Length == 2 && parts[0] == AuthenticationSchemes.Basic.ToString())
                        {
                            string credentials = Encoding.ASCII.GetString(Convert.FromBase64String(parts[1]));
                            parts = credentials.Split(':');
                            string userName = parts[0];
                            string password = parts[1];

                            if (userName == "public" && password == "public")
                            {
                                IIdentity basicIdentity = new GenericIdentity(userName);
                                context.User = new GenericPrincipal(basicIdentity, new string[0]);
                                FormsAuthentication.SetAuthCookie(context.User.Identity.Name, true);
                            }
                        }
                    }
                }
            }

            if (context.User == null || context.User.Identity.IsAuthenticated == false)
            {
                context.Response.StatusDescription = HttpStatusCode.Unauthorized.ToString();
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.AddHeader(HttpResponseHeader.WwwAuthenticate.GetName(), AuthenticationSchemes.Basic.ToString());
                context.Response.End();
            }
        }

        internal void OnAuthenticateRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpContext context = app.Context;

            if (context.Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                _formsAuthenticationModuleOnEnter.Invoke(_formsAuthenticationModule, new Object[] { sender, e });
            }
        }
    }
}
