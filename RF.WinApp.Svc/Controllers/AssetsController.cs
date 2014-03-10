using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;

using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using System.Web.Http.OData.Query;
using Microsoft.Data.OData;
using Microsoft.Data.OData.Query;

using RF.Common.Transactions;
using RF.BL;
using RF.BL.Model;
using RF.BL.Model.Enums;
using RF.WinApp.Svc.Extensions;

namespace RF.WinApp.Svc.Controllers
{
    public class AssetsController : ODataController
    {
        private IAssetsRepository _rep;

        public AssetsController(IAssetsRepository rep)
        {
            _rep = rep;
        }

        [CustomQueryable]
        public IQueryable<AssetValue> Get()
        {
            return (_rep.Context as DbSet<AssetValue>).Include("Governor").Include("Governor.Company");
        }

        public HttpResponseMessage Get([FromODataUri] Guid key)
        {
            var gov = _rep.GetById(key);
            if (gov == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, gov);
            }
        }

        public HttpResponseMessage Post(AssetValue val)
        {
            if (val == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            _rep.Create(val);
            var response = Request.CreateResponse(HttpStatusCode.Created, val);

            var odataPath = Request.GetODataPath();
            if (odataPath == null)
            {
                throw new InvalidOperationException("There is no ODataPath in the request.");
            }
            var entitySetPathSegment = odataPath.Segments.FirstOrDefault() as EntitySetPathSegment;
            if (entitySetPathSegment == null)
            {
                throw new InvalidOperationException("ODataPath doesn't start with EntitySetPathSegment.");
            }

            response.Headers.Location = new Uri(Url.ODataLink(entitySetPathSegment, new KeyValuePathSegment(ODataUriUtils.ConvertToUriLiteral(val.Id, ODataVersion.V3))));

            //response.Headers.Location = new Uri(Url.ODataLink(new EntitySetPathSegment("Governors"), new KeyValuePathSegment(gov.Id.ToString())));

            return response;
        }

        public HttpResponseMessage Delete([FromODataUri] Guid key)
        {
            _rep.Delete(_rep.GetById(key));
            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        public HttpResponseMessage Put([FromODataUri] Guid key, AssetValue val)
        {
            _rep.Update(val);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [AcceptVerbs("PUT")]
        public HttpResponseMessage PutGovernor([FromODataUri] Guid key, [FromBody] Uri link)
        {
            Guid relatedKey = Request.GetKeyValue<Guid>(link);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        [AcceptVerbs("POST")]
        public string Report(ODataActionParameters parameters)
        {
            DateTime db = (DateTime)parameters["DateBegin"];
            DateTime de = (DateTime)parameters["DateEnd"];
            InsuranceType insType = (InsuranceType)(byte)parameters["InsuranceType"];
            Guid? govId = (Guid?)parameters["GovernorId"];
            return _rep.PublicAssetProfitReport(db, de, insType, govId);
        }

        [AcceptVerbs("POST")]
        public bool CreateBatch(ODataActionParameters parameters)
        {
            bool ret = false;

            var values = parameters["Values"] as ICollection<string>;

            using (var trans = Transactions.StartNew())
            {
                foreach (var s in values)
                {
                    var val = Newtonsoft.Json.JsonConvert.DeserializeObject<AssetValue>(s);
                    _rep.Create(val);
                }

                trans.Complete();
                ret = true;
            }

            return ret;
        }
    }
}
