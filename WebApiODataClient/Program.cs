using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using System.Data.Services.Client;

using System.Web;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Globalization;
using Microsoft.IdentityModel.Protocols.OAuth;
using Microsoft.IdentityModel.Protocols.OAuth.Client;

using WebApi.Svc;
using RF.LinqExt;
using RF.Sts.Auth;
using RF.Sts.Auth.Configuration;
using RF.WcfDS.OAuth;
using RF.Common.Security;

namespace WebApiODataClient
{
    class Program
    {
        static void Main(string[] args)
        {
            OAuthProvider.Behavior = Activator.CreateInstance(OAuthConfiguration.Configuration.ClientSettings.WcfDSBehaviorType) as IOAuthBehavior;
            Logon.Page = new LogonProc();
            var ctx = new WebApiCtx();
            FilterParameterCollection fc = new FilterParameterCollection();
            fc.Add("ShortName", "магомед ук");
            SortParameterCollection sc = new SortParameterCollection();
            sc.Add(null, "Company.Name", System.ComponentModel.ListSortDirection.Ascending);
            sc.Add(null, "Id", System.ComponentModel.ListSortDirection.Ascending);

            //var val = ctx.Governors.AddFilters(fc).AddOrders(sc).First();
            //var val = ctx.Governors.First();
            //val.Company.lawFormValue = 6;
            Guid g = new Guid("CEB5254F-5FFE-469B-ABC5-09AB388E6505");
            //var val = ctx.Governors.Where(gov => gov.Id == g).FirstOrDefault();
            Guid g2 = new Guid("CEB5254F-5FFE-469B-ABC5-09AB388E6505");
            Governor val2 = null;
            //ctx.TryGetEntity(new Uri("http://localhost:555/Governors(guid'ceb5254f-5ffe-469b-abc5-09ab388e6505')"), out val2);
            //Governor val2 = ctx.Governors.Where(gov => gov.Id == g2).FirstOrDefault();
            //val2 = ctx.Entities.FirstOrDefault(e => e.Identity.Contains("Governors(guid'ceb5254f-5ffe-469b-abc5-09ab388e6505')")).Entity as Governor;
            //val2 = ctx.Governors.GetById(g);

            ctx.Governors.ToList();

            //Console.WriteLine(ctx.Governors.TotalCount());

            //var ass = ctx.Assets.Where(a => a.Id == new Guid("D3B67671-87C5-4B33-8DF1-7D83947E5FB8")).First();

            var ass = new AssetValue();
            ass.Id = Guid.NewGuid();
            ass.InsuranceTypeValue = 1;
            ass.TakingDate = DateTime.Today;
            ass.Value = 3344434.56m;
            ass.GovernorId = g2;

            string s = JsonConvert.SerializeObject(ass);
            var list = new List<string>();
            list.Add(s);

            UriBuilder urib = new UriBuilder(ctx.BaseUri);
            //urib.Path = string.Format("{0}/CreateBatch", ctx.Assets.RequestUri.PathAndQuery);
            //var r = ctx.Execute<bool>(urib.Uri, "POST", true, new BodyOperationParameter("Values", list)).FirstOrDefault();

            //ctx.AddToAssets(ass);
            //ctx.SetLink(ass, "Governor", val2);

            //ctx.SaveChanges();
            //ctx.AttachLink(ass, "Governor", val2);

            //Guid g = new Guid("32D3F7C1-97E1-4A69-8C8A-E3706490329E");
            //var val = ctx.Holidays.Where(h => h.Id == g).FirstOrDefault();

            //val.Comment = "Test Comment";


            var newc = new Company();
            newc.Id = Guid.NewGuid();
            newc.Name = "Test Governor Company Name";
            var newg = new Governor();
            newg.Id = Guid.NewGuid();
            newg.ShortName = "Test Governor Name";
            newg.Company = newc;
            ctx.AddToGovernors(newg);
            ctx.SaveChanges();
            newg.ShortName = "Test Governor Name Updated";
            ctx.UpdateObject(newg);
            //ctx.SaveChanges(SaveChangesOptions.PatchOnUpdate);
            System.Threading.Thread.Sleep(1000);
            ctx.SaveChanges(SaveChangesOptions.ReplaceOnUpdate);
            ctx.DeleteObject(newg);
            ctx.SaveChanges();

            //var serviceCreds = new NetworkCredential("Administrator", "SecurePassword");
            //var cache = new CredentialCache();
            //var serviceUri = new Uri("http://ipv4.fiddler:333/api/issue");
            //cache.Add(serviceUri, "Basic", serviceCreds);
            //ctx.Credentials = cache;
            //var r = ctx.Execute(new Uri("http://ipv4.fiddler:333/api/issue"), "POST", new BodyOperationParameter("rst", new TokenRequest() { GrantType = "client_credentials", Scope = "http://localhost" }));
            Console.ReadLine();
        }

        static string DoAuth(Uri realm)
        {
            string token = String.Empty;

            using (HttpClient client = new HttpClient())
            {
                string creds = String.Format("{0}:{1}", "badri", "badri");
                byte[] bytes = Encoding.ASCII.GetBytes(creds);
                var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes));
                client.DefaultRequestHeaders.Authorization = header;

                var rst = new TokenRequest() { Scope = realm };
                HttpContent content = new StringContent(JsonConvert.SerializeObject(rst), Encoding.UTF8, "application/json");
                token = client.PostAsync("http://ipv4.fiddler:333/api/issue", content).Result.Content.ReadAsStringAsync().Result;
                //token = client.PostAsync("http://localhost:333/api/issue", content).Result.Content.ReadAsStringAsync().Result;
            }

            var rstr = JsonConvert.DeserializeObject<TokenResponse>(token);
            Console.WriteLine(rstr.AccessToken);
            return HttpUtility.UrlDecode(rstr.AccessToken);
        }

        static void RegisterOAuthProxy()
        {
            HttpListener listener = new HttpListener();
            //for non-admin users needs
            //netsh http add urlacl url=http://localhost:666/ user="moscow\domain users"
            listener.Prefixes.Add("http://localhost:666/");
            listener.Start();
            Console.WriteLine("Listening...");

            TaskScheduler.UnobservedTaskException += (object sender, UnobservedTaskExceptionEventArgs e) => { throw e.Exception.InnerException; };

            Task.Factory.StartNew(new Action(() => StartOAuthProxy(listener)));
        }

        static void StartOAuthProxy(HttpListener listener)
        {
            while (true)
            {
                var ctx = listener.GetContext();
                Task.Factory.StartNew(new Action(() => ListenerCallback(ctx)));
            }
        }

        static string BearerToken = "";

        static void ListenerCallback(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            try
            {
                string requestBody;
                using (StreamReader reader = new StreamReader(request.InputStream))
                    requestBody = reader.ReadToEnd();

                var method = new HttpMethod(request.HttpMethod);
                //var requestUri = new Uri("http://ipv4.fiddler:555" + request.Url.PathAndQuery);
                var requestUri = new Uri("http://localhost:555" + request.Url.PathAndQuery);
                HttpResponseMessage r = Send(method, requestUri, requestBody, request.ContentType);

                if (r.StatusCode == HttpStatusCode.Unauthorized)
                {
                    string token = DoAuth(new Uri("http://localhost:555"));
                    byte[] bytes = Encoding.ASCII.GetBytes(token);
                    lock (BearerToken)
                        BearerToken = Convert.ToBase64String(bytes);

                    r = Send(method, requestUri, requestBody, request.ContentType);
                }

                // Construct a response. 
                foreach (var kvp in r.Headers)
                    foreach (var val in kvp.Value)
                        response.Headers.Set(kvp.Key, val);

                if (r.Content.Headers.ContentType != null)
                    response.ContentType = r.Content.Headers.ContentType.MediaType;

                response.StatusCode = (int)r.StatusCode;
                response.StatusDescription = r.StatusCode.ToString();

                string responseString = r.Content.ReadAsStringAsync().Result;
                //replace uri by Listener address becouse DataServiceContext send Update, Delete operations (PUT, DELETE) according entities uri from server serponse body, not BaseUri
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString.Replace("http://localhost:555", "http://localhost:666"));
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine(ex.Message);
                Exception innerEx = ex.InnerException;
                while (innerEx != null)
                {
                    Console.WriteLine(innerEx.Message);
                    innerEx = innerEx.InnerException;
                }
                throw ex;
            }
        }

        static HttpResponseMessage Send(HttpMethod method, Uri requestUri, string content, string contentType)
        {
            HttpRequestMessage req = new HttpRequestMessage(method, requestUri);
            if (!string.IsNullOrWhiteSpace(content))
            {
                HttpContent httpContent = new StringContent(content, Encoding.UTF8);
                httpContent.Headers.Remove("Content-Type");
                httpContent.Headers.Add("Content-Type", contentType);
                req.Content = httpContent;
            }

            using (HttpClient client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(BearerToken))
                {
                    var header = new AuthenticationHeaderValue("Bearer", BearerToken);
                    client.DefaultRequestHeaders.Authorization = header;
                }

                return client.SendAsync(req).Result;
            }
        }
    }
}
