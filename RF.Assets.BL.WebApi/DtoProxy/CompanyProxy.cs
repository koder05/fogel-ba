using System;
using System.Collections.Generic;
using System.Linq;

using RF.BL.Model;
using Svc = WebApi.Svc;

namespace RF.BL.WebApi.DtoProxy
{
    public class CompanyProxy : Company, IDtoProxy
    {
        public CompanyProxy(Svc.Company dto)
            : base()
        {
            if (dto != null)
            {
                this.Id = dto.Id;
                this.Name = dto.Name;
                this.lawFormValue = dto.lawFormValue;

                this._dto = dto;
                this.PropertyChanged += ProxyActivator.ReflectChangedProperty;
            }
        }

        private Svc.Company _dto;
        public object Dto { get { return _dto; } }
    }
}
