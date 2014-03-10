using System;
using RF.LinqExt;
using RF.BL.Model;

namespace RF.Assets.BL
{
    public class AssetsPropResolver : PropMapper
    {
        protected override void OnMapConfigure()
        {
            CreateMap<AssetValue, AssetValue>(ListControlActionType.Sorting).ForMember(s => s.InsuranceTypeString, d => d.InsuranceTypeValue);
            CreateMap<AssetValue, AssetValue>(ListControlActionType.Filtering)
                .ForMember(s => s.InsuranceTypeValue, d => (int)d.InsuranceTypeValue);
        }
    }
}
