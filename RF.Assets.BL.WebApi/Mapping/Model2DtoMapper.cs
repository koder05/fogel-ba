using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

using RF.BL.Model;
using RF.BL.WebApi.DtoProxy;
using Svc = WebApi.Svc;

namespace RF.BL.WebApi.Mapping
{
	/// <summary>
    /// Mapping from BaseModel derived classes to their auto-generated dto classes using AutoMapper
	/// </summary>
	public class Model2DtoMapper
	{
		private static Dictionary<BaseModel, object> _cache = new Dictionary<BaseModel, object>();

		public Model2DtoMapper()
		{
            Mapper.CreateMap<Governor, Svc.Governor>();
            Mapper.CreateMap<Company, Svc.Company>();
            Mapper.CreateMap<WorkCalendar, Svc.WorkCalendar>();
            Mapper.CreateMap<AssetValue, Svc.AssetValue>();
			//Mapper.CreateMap<CompanyModel, Company>().ForMember(p => p.lawFormValue, op => op.Ignore());
		}

        public TResult Map<T, TResult>(T model)
            where T : BaseModel
            where TResult : class, new()
        {
            //if dto and model already was synchronize (exists proxy)
            if (model is IDtoProxy)
            {
                return (TResult)((IDtoProxy)model).Dto;
            }

            lock (_cache)
            {
                if (_cache.ContainsKey(model))
                {
                    return (TResult)_cache[model];
                }

                TResult dto = Activator.CreateInstance<TResult>();

                _cache.Add(model, dto);
                Mapper.Map<T, TResult>(model, dto);
                _cache.Remove(model);

                return dto;
            }
        }

		public TResult Map<T, TResult>(T model, TResult dto)
			where T : BaseModel
            where TResult : class, new()
		{
			return Mapper.Map<T, TResult>(model, dto);
		}
	}
}
