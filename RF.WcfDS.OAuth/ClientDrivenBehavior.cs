using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Data.Services.Client;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Csdl;

using RF.Sts.Auth;
using RF.Common;

namespace RF.WcfDS.OAuth
{
    public class ClientDrivenBehavior : IOAuthBehavior
    {
        private TimeSpan _skew = new TimeSpan(0, 0, 5);
        private Uri _realm;
        private ManualResetEvent _modelLoadCompleteEvent = new ManualResetEvent(false);   

        public Uri RegisterDataService(DataServiceContext ctx)
        {
            ctx.SendingRequest += DS_SendingRequest;
            _realm = ctx.BaseUri;
            ctx.Format.LoadServiceModel = () => LoadModel(_realm);

            AsyncHelper.Stitch(() => ctx.Format.UseJson(), null); 
            return ctx.BaseUri;
        }

        void DS_SendingRequest(object sender, SendingRequestEventArgs e)
        {
            //if (_modelLoadCompleteEvent.WaitOne(30000) == false)                 throw new TimeoutException("The load edm model method has`t been returned.");
            _modelLoadCompleteEvent.WaitOne();
            e.Request.Headers.Add("Authorization", "Bearer " + GetRawToken());
        }

        public IEdmModel LoadModel(Uri realm)
        {
            using (HttpClient client = new HttpClient())
            {
                var header = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetRawToken());
                client.DefaultRequestHeaders.Authorization = header;

                Console.WriteLine("\t{0} {1}", "GET", realm.ToString() + "$metadata");

                HttpResponseMessage response = client.GetAsync(realm.ToString() + "$metadata").Result;
                var stream = response.Content.ReadAsStreamAsync().Result;
                using (var reader = XmlReader.Create(stream))
                {
                    IEdmModel model = EdmxReader.Parse(reader);
                    _modelLoadCompleteEvent.Set();
                    return model;
                }
            }
        }

        private string GetRawToken()
        {
            string rawToken = OAuthClientModule.GetAccessToken(this._realm, false);
            var tokenHandler = new SimpleWebTokenHandler();
            var token = tokenHandler.ReadToken(Encoding.ASCII.GetString(Convert.FromBase64String(rawToken)));
            if (DateTime.Compare(token.ValidTo, DateTime.UtcNow.Add(_skew)) <= 0)
            {
                rawToken = OAuthClientModule.GetAccessToken(this._realm, true);
            }

            return rawToken;
        }
    }
}
