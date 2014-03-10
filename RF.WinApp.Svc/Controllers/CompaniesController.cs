using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using RF.BL;
using RF.BL.Model;
using EF;
using RF.BL.EF;

namespace RF.WinApp.Svc.Controllers
{
    public class CompaniesController : ApiController
    {
        private AssetsEFCtx db = new AssetsEFCtx();

        [CustomQueryable]
        public IQueryable<Company> Get()
        {
            return db.Companies;
        }
    }
}
