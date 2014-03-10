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

namespace RF.Sts.Secure
{
    internal class FormsAuthenticationDisabler
    {
        private FormsAuthenticationModule _formsAuthenticationModule;
        private MethodInfo _formsAuthenticationModuleOnEnter;
        private MethodInfo _formsAuthenticationModuleOnLeave;

        #region IHttpModule Members

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
        #endregion

        #region implementation

        private void OnEndRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpContext context = app.Context;

            if (context.Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                _formsAuthenticationModuleOnLeave.Invoke(_formsAuthenticationModule, new Object[] { sender, e });
            }
        }

        private void OnPostAuthenticateRequest(object sender, EventArgs e)
        {
            // Отработала WindowsAuthentication - пользуемся результатами.
            HttpApplication app = (HttpApplication)sender;
            HttpContext context = app.Context;

            var area = RouteTable.Routes.GetRouteData(new HttpContextWrapper(context)).DataTokens["area"] as string;

            if (context.User == null || context.User.Identity.IsAuthenticated == false)
            {
                if (area == WindowsAuthArea.Name)
                {
                    WindowsIdentity iisIdentity = context.Request.LogonUserIdentity;
                    if (iisIdentity != null)
                    {
                        context.User = new WindowsPrincipal(iisIdentity.IsAnonymous ? WindowsIdentity.GetAnonymous() : iisIdentity);
                        FormsAuthentication.SetAuthCookie(context.User.Identity.Name, true);
                    }
                }
                else if (area == BasicAuthArea.Name)
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
                            IIdentity basicIdentity = new GenericIdentity(userName);
                            context.User = new GenericPrincipal(basicIdentity, new string[0]);
                            FormsAuthentication.SetAuthCookie(context.User.Identity.Name, true);
                        }
                    }
                    else
                    {
                        context.Response.StatusDescription = HttpStatusCode.Unauthorized.ToString();
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        context.Response.AddHeader(HttpResponseHeader.WwwAuthenticate.GetName(), AuthenticationSchemes.Basic.ToString());
                        context.Response.End();
                    }
                }
            }
            
            if (context.User != null && context.User.Identity.IsAuthenticated)
            {
                if (area == WindowsAuthArea.Name)
                {
                    context.Response.Redirect(FormsAuthentication.DefaultUrl);
                }
                else if (area == BasicAuthArea.Name)
                {
                    context.Response.Redirect(FormsAuthentication.DefaultUrl);
                    //context.Response.Redirect("http://ipv4.fiddler:333/api/issue/");
                }
            }
        }

        private void OnAuthenticateRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpContext context = app.Context;

            if (context.Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                _formsAuthenticationModuleOnEnter.Invoke(_formsAuthenticationModule, new Object[] { sender, e });
            }

            //if (context.User == null)
            //{
            //    var oldTicket = ExtractTicketFromCookie(context, FormsAuthentication.FormsCookieName);
            //    if (oldTicket != null && !oldTicket.Expired)
            //    {
            //        var ticket = oldTicket;
            //        if (FormsAuthentication.SlidingExpiration)
            //        {
            //            ticket = FormsAuthentication.RenewTicketIfOld(oldTicket);
            //            if (ticket == null)
            //                return;
            //        }

            //        context.User = new GenericPrincipal(new FormsIdentity(ticket), new string[0]);
            //        if (ticket != oldTicket)
            //        {
            //            // update the cookie since we've refreshed the ticket
            //            string cookieValue = FormsAuthentication.Encrypt(ticket);
            //            var cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName] ??
            //                         new HttpCookie(FormsAuthentication.FormsCookieName, cookieValue) { Path = ticket.CookiePath };

            //            if (ticket.IsPersistent)
            //                cookie.Expires = ticket.Expiration;
            //            cookie.Value = cookieValue;
            //            cookie.Secure = FormsAuthentication.RequireSSL;
            //            cookie.HttpOnly = true;
            //            if (FormsAuthentication.CookieDomain != null)
            //                cookie.Domain = FormsAuthentication.CookieDomain;
            //            context.Response.Cookies.Remove(cookie.Name);
            //            context.Response.Cookies.Add(cookie);
            //        }
            //    }
            //}
        }

        #endregion

        private FormsAuthenticationTicket ExtractTicketFromCookie(HttpContext context, string name)
        {
            FormsAuthenticationTicket ticket = null;
            string encryptedTicket = null;

            var cookie = context.Request.Cookies[name];
            if (cookie != null)
            {
                encryptedTicket = cookie.Value;
            }

            if (!string.IsNullOrEmpty(encryptedTicket))
            {
                try
                {
                    ticket = FormsAuthentication.Decrypt(encryptedTicket);
                }
                catch
                {
                    context.Request.Cookies.Remove(name);
                }

                if (ticket != null && !ticket.Expired)
                {
                    return ticket;
                }

                // if the ticket is expired then remove it
                context.Request.Cookies.Remove(name);
                return null;
            }

            return null;
        }
    }
}
