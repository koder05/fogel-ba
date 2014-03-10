using System;
using System.Linq;
using System.Xml;
using System.IO;
using System.Data.Services.Client;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Csdl;
using Microsoft.Data.OData;

using WebApi.Svc.Exceptions;
using RF.WcfDS.OAuth;
using RF.Sts.Auth.Configuration;  

namespace WebApi.Svc
{
    public partial class WebApiCtx : DataServiceContext
    {
        public WebApiCtx()
            //: this(new Uri("http://ipv4.fiddler:555/"))
            : this(OAuthConfiguration.Configuration.ServiceSettings.Realm)
            //: this(new Uri("http://localhost:555/svc/"))
        {
            OAuthProvider.RegisterDataService(this);
            this.SendingRequest += Container_SendingRequest;
            this.ReceivingResponse += Container_ReceivingResponse;
            this.IgnoreResourceNotFoundException = true;
            this.ResolveName = new global::System.Func<global::System.Type, string>(this.ResolveNameFromType);
            this.ResolveType = new global::System.Func<string, global::System.Type>(this.ResolveTypeFromName);
            this.OnContextCreated();
            //this.Format.LoadServiceModel = LoadModel;
            //this.Format.UseJson();
            //this.Format.UseAtom();
        }

        private IEdmModel LoadModel()
        {
            using (var reader = new XmlTextReader(GetMetadataUri().ToString()))
            {
                IEdmModel model = EdmxReader.Parse(reader);
                return model;
            }
        }

        void Container_SendingRequest(object sender, SendingRequestEventArgs e)
        {
            Console.WriteLine("\t{0} {1}", e.Request.Method, e.Request.RequestUri.ToString());
            //ICredentials credentials = new System.Net.NetworkCredential("username", "password");
            //e.Request.Proxy = new System.Net.WebProxy("http://localhost:666", true, null, credentials);
        }

        void Container_ReceivingResponse(object sender, ReceivingResponseEventArgs e)
        {
            if (e.ResponseMessage.StatusCode == (int)System.Net.HttpStatusCode.InternalServerError)
            {
                using (StreamReader reader = new StreamReader(e.ResponseMessage.GetStream()))
                {
                    string contents = reader.ReadToEnd();
                    var ex = new DataServiceClientException(contents);
                    if (this.Format.ODataFormat == ODataFormat.Atom)
                        DataServiceExceptionAtomParser.Throw(ex);
                    else if (this.Format.ODataFormat == ODataFormat.Json)
                        DataServiceExceptionJsonParser.Throw(ex);
                    else
                        throw ex;
                }
            }
            else if (e.ResponseMessage.StatusCode == (int)System.Net.HttpStatusCode.Unauthorized)
            {
                var h = e.ResponseMessage.Headers.Where(kvp => kvp.Key == "WWW-Authenticate").Select(kvp => kvp.Value).FirstOrDefault();
            }
        }
    }
}
