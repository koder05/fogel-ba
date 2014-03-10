using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using Newtonsoft.Json;

using RF.Common.Http;
using RF.Common.Security;
using RF.Sts.Auth.Configuration;

namespace RF.Sts.Auth
{
    public static class OAuthClientModule
    {
        private static readonly object sync = new object();
        static OAuthClientModule()
        {
            TokensStore.StoreProvider = OAuthConfiguration.Configuration.ClientSettings.TokensStoreProvider;
        }

        public static string GetAccessToken(Uri realm, bool force)
        {
            realm = RealmOverFiddler(realm);
            string token = TokensStore.StoreProvider.TakeToken<string>(realm);

            if (string.IsNullOrEmpty(token) || force)
                lock (sync)
                    token = IssueAccessToken(realm);

            return token;
        }

        private static string IssueAccessToken(Uri realm)
        {
            realm = RealmOverFiddler(realm);
            var method = HttpMethod.Post;
            var requestUri = OAuthConfiguration.Configuration.StsSettings.IssuerUri;
            var rst = new TokenRequest() { Scope = realm };
            var content = JsonConvert.SerializeObject(rst);
            var contentType = "application/json";
            AuthenticationHeaderValue auth = null;

            HttpResponseMessage r = Send(method, requestUri, content, contentType, auth);

            while (r.StatusCode != HttpStatusCode.OK)
            {
                if (r.StatusCode == HttpStatusCode.Unauthorized)
                {
                    if (r.Headers.WwwAuthenticate.Any(h => h.Scheme == AuthenticationSchemes.Basic.ToString()))
                    {
                        var creds = Logon.Page.GetLogin(null);
                        if (!creds.IsSuccessful || creds.IsCanceled)
                            break;

                        string loginpsw = String.Format("{0}:{1}", creds.Name, creds.Psw);
                        byte[] bytes = Encoding.ASCII.GetBytes(loginpsw);
                        auth = new AuthenticationHeaderValue(AuthenticationSchemes.Basic.ToString(), Convert.ToBase64String(bytes));
                    }
                    else if (r.Headers.Any(h => h.Key == HttpResponseHeader.SetCookie.GetName()))
                    {
                    }
                    else
                    {
                        break;
                    }
                }
                else if (r.StatusCode == HttpStatusCode.Redirect || r.StatusCode == HttpStatusCode.Moved)
                {
                    requestUri = new Uri(requestUri, r.Headers.Location.OriginalString);
                    auth = null;
                }
                else
                {
                    break;
                }

                r = Send(method, requestUri, content, contentType, auth);
            }

            try
            {
                r.EnsureSuccessStatusCode();
                var token = r.Content.ReadAsStringAsync().Result;
                var rstr = JsonConvert.DeserializeObject<TokenResponse>(token);
                Console.WriteLine(rstr.AccessToken);
                token = HttpUtility.UrlDecode(rstr.AccessToken);
                token = Convert.ToBase64String(Encoding.ASCII.GetBytes(token));
                TokensStore.StoreProvider.PutToken(realm, token);
                return token;
            }
            catch (Exception ex)
            {
                throw new Exception("Unsuccessful attempt to login on STS.", ex);
            }
        }

        private static Uri RealmOverFiddler(Uri realm)
        {
            return new Uri(realm.ToString().Replace("ipv4.fiddler", "localhost"));
        }

        private static HttpResponseMessage Send(HttpMethod method, Uri requestUri, string content, string contentType, AuthenticationHeaderValue auth)
        {
            HttpResponseMessage ret;
            HttpRequestMessage req = new HttpRequestMessage(method, requestUri);

            //to avoid error "Unable to read data from the transport connection: The connection was closed" when redirect
            //http://briancaos.wordpress.com/2012/07/06/unable-to-read-data-from-the-transport-connection-the-connection-was-closed/
            //req.Version = new Version(1, 0);

            if (!string.IsNullOrWhiteSpace(content))
            {
                HttpContent httpContent = new StringContent(content, Encoding.UTF8);
                httpContent.Headers.Remove("Content-Type");
                httpContent.Headers.Add("Content-Type", contentType);
                req.Content = httpContent;
            }

            using (var handler = new HttpClientHandler() { AllowAutoRedirect = false, CookieContainer = new CookieContainer() })
            {
                var stsAuthCookieName = OAuthConfiguration.Configuration.ClientSettings.FormsAuthCookieName;
                var cookiesBaseUri = new Uri(requestUri, "/");

                var stsAuthCookie = TokensStore.StoreProvider.TakeToken<Cookie>(cookiesBaseUri);
                if (stsAuthCookie != null)
                    handler.CookieContainer.Add(cookiesBaseUri, stsAuthCookie);

                using (HttpClient client = new HttpClient(handler))
                {
                    if (auth != null)
                        client.DefaultRequestHeaders.Authorization = auth;

                    try
                    {
                        var task = client.SendAsync(req);
                        //System.Diagnostics.Debug.WriteLine(string.Format("client.SendAsync(req) : auth={0}, url={1}", auth, req.RequestUri));
                        ret = task.Result;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                CookieCollection cookies = handler.CookieContainer.GetCookies(cookiesBaseUri);
                stsAuthCookie = cookies[stsAuthCookieName];
                if (stsAuthCookie != null)
                {
                    TokensStore.StoreProvider.PutToken(cookiesBaseUri, stsAuthCookie);
                }
            }

            return ret;
        }
    }
}
