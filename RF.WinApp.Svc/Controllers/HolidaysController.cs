using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;

using RF.BL;
using RF.BL.Model;
using EF;
using RF.BL.EF;

namespace RF.WinApp.Svc.Controllers
{
    public class HolidaysController : ODataController
    {
        private IWorkcalendarRepository _rep;

        public HolidaysController(IWorkcalendarRepository rep)
        {
            _rep = rep;
        }

        [CustomQueryable]
        public IQueryable<WorkCalendar> Get()
        {
            return _rep.Context;
        }

        public HttpResponseMessage Put([FromODataUri] Guid key, WorkCalendar entity)
        {
            _rep.Update(entity);
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
    }
}
