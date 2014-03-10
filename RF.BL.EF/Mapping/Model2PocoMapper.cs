using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

using EF;
using EF.Poco;
using RF.BL.Model;
using RF.BL.EF.PocoProxy; 

namespace RF.BL.EF.Mapping
{
	/// <summary>
	/// Mapping from BaseModel derived classes to their POCO using AutoMapper
	/// </summary>
	public class Model2PocoMapper
	{
		private static Dictionary<BaseModel, BasePoco> _cache = new Dictionary<BaseModel, BasePoco>();

		public Model2PocoMapper()
		{
			Mapper.CreateMap<Person, PersonPoco>();
            Mapper.CreateMap<Zl, ZlPoco>();

			//Mapper.CreateMap<CompanyModel, Company>().ForMember(p => p.lawFormValue, op => op.Ignore());
		}

        public TResult Map<T, TResult>(T model)
            where T : BaseModel
            where TResult : BasePoco, new()
        {
            //if poco and model already was synchronize (exists proxy)
            if (model is IPocoProxy)
            {
                return (TResult)((IPocoProxy)model).Poco;
            }

            lock (_cache)
            {
                if (_cache.ContainsKey(model))
                {
                    return (TResult)_cache[model];
                }

                TResult poco = Activator.CreateInstance<TResult>();

                _cache.Add(model, poco);
                Mapper.Map<T, TResult>(model, poco);
                _cache.Remove(model);

                return poco;
            }
        }

		public TResult Map<T, TResult>(T model, TResult poco)
			where T : BaseModel
			where TResult : BasePoco
		{
			return Mapper.Map<T, TResult>(model, poco);
		}
	}
}
