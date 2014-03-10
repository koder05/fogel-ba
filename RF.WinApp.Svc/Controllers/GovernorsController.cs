using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Security.Permissions;

using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using Microsoft.Data.OData;
using Microsoft.Data.OData.Query;

using RF.BL;
using RF.BL.Model;

namespace RF.WinApp.Svc.Controllers
{
    [Authorize]
    public class GovernorsController : ODataController
    {
        private IGovernorRepository _rep;

        public GovernorsController(IGovernorRepository rep)
        {
            _rep = rep;
        }

        [CustomQueryable]
        [PrincipalPermission(SecurityAction.Demand, Role = "AssetsServiceUser")]
        public IQueryable<Governor> Get()
        {
            return (_rep.Context as DbSet<Governor>).Include("Company");
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

        public HttpResponseMessage Post(Governor gov)
        {
            if (gov == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            _rep.Create(gov);
            var response = Request.CreateResponse(HttpStatusCode.Created, gov);

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

            response.Headers.Location = new Uri(Url.ODataLink(entitySetPathSegment, new KeyValuePathSegment(ODataUriUtils.ConvertToUriLiteral(gov.Id, ODataVersion.V3))));

            //response.Headers.Location = new Uri(Url.ODataLink(new EntitySetPathSegment("Governors"), new KeyValuePathSegment(gov.Id.ToString())));

            return response;
        }

        public HttpResponseMessage Delete([FromODataUri] Guid key)
        {
            _rep.Delete(_rep.GetById(key));
            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        public HttpResponseMessage Put([FromODataUri] Guid key, Governor update)
        {
            _rep.Update(update);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}
