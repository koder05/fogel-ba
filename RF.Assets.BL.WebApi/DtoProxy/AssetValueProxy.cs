using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RF.BL.Model;
using Svc = WebApi.Svc;

namespace RF.BL.WebApi.DtoProxy
{
    public class AssetValueProxy : AssetValue, IDtoProxy
    {
        public AssetValueProxy(Svc.AssetValue dto)
            : base()
        {
            if (dto == null)
                throw new InvalidOperationException("AssetValue dto");

            this.Id = dto.Id;
            this.TakingDate = dto.TakingDate;
            this.Value = dto.Value;
            this.CashFlow = dto.CashFlow;
            this.InsuranceTypeValue = dto.InsuranceTypeValue;
            this.GovernorId = dto.GovernorId;
            this.Governor = ProxyActivator.CreateProxy<Svc.Governor, Governor>(dto.Governor);

            this._dto = dto;
            this.PropertyChanged += ProxyActivator.ReflectChangedProperty;
        }

        private Svc.AssetValue _dto;
        public object Dto { get { return _dto; } }
    }
}
