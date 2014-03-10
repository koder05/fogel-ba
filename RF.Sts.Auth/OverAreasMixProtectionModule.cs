using System;
using System.Web;

namespace RF.Sts.Auth
{
    public class OverAreasMixProtectionModule : IHttpModule
    {
        public const string StsAreaName = "sts";
        public const string SvcAreaName = "svc";

        private OAuthProtectionModule _oauthModule;
        private StsProtectionModule _stsModule;

        public void Dispose()
        {
            _oauthModule.Dispose();
            _stsModule.Dispose();
        }

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += OnAuthenticateRequest;
            context.PostAuthenticateRequest += OnPostAuthenticateRequest;
            context.EndRequest += OnEndRequest;

            HttpApplication fakeApp = new HttpApplication();
            _stsModule = new StsProtectionModule();
            _stsModule.Init(fakeApp);
            _oauthModule = new OAuthProtectionModule();
            _oauthModule.Init(fakeApp);
        }

        void OnAuthenticateRequest(object sender, EventArgs args)
        {
            var area = GetArea((HttpApplication)sender);
            if (area == SvcAreaName)
            {
                _oauthModule.OnAuthenticateRequest(sender, args);
            }
            else if (area == StsAreaName)
            {
                _stsModule.OnAuthenticateRequest(sender, args);
            }
        }

        void OnPostAuthenticateRequest(object sender, EventArgs args)
        {
            var area = GetArea((HttpApplication)sender);
            if (area == SvcAreaName)
            {
                _oauthModule.OnPostAuthenticateRequest(sender, args);
            }
            else if (area == StsAreaName)
            {
                _stsModule.OnPostAuthenticateRequest(sender, args);
            }
        }

        void OnEndRequest(object sender, EventArgs args)
        {
            var area = GetArea((HttpApplication)sender);
            if (area == SvcAreaName)
            {
                _oauthModule.OnEndRequest(sender, args);
            }
            else if (area == StsAreaName)
            {
                _stsModule.OnEndRequest(sender, args);
            }
        }

        private string GetArea(HttpApplication app)
        {
            HttpContext context = app.Context;

            if (context.Request.Path.StartsWith(string.Concat("/", SvcAreaName, "/")))
                return SvcAreaName;

            if (context.Request.Path.StartsWith(string.Concat("/", StsAreaName, "/")))
                return StsAreaName;

            return null;
        }
    }
}
