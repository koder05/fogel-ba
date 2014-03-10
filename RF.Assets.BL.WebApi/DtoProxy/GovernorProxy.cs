using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RF.BL.Model;
using Svc = WebApi.Svc;

namespace RF.BL.WebApi.DtoProxy
{
    public class GovernorProxy : Governor, IDtoProxy
    {
        public GovernorProxy(Svc.Governor dto)
            : base()
        {
            if (dto == null)
                throw new InvalidOperationException("Governor dto");

            this.Id = dto.Id;
            this.CompanyId = dto.CompanyId;
            this.Company = ProxyActivator.CreateProxy<Svc.Company, Company>(dto.Company);
            this.ShortName = dto.ShortName;

            this._dto = dto;
            this.PropertyChanged += ProxyActivator.ReflectChangedProperty;
        }

        private Svc.Governor _dto;
        public object Dto { get { return _dto; } }
    }
}
