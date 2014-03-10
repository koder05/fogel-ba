using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RF.LinqExt
{
	public enum ListControlActionType
	{
		Common = 0,
		Filtering = 1,
		Sorting = 2
	}
	
	public abstract class PropMapper : IFilterSortPropResolver
	{
		public PropMapper()
		{
			Configure();
		}
		
		public void Configure()
		{
			OnMapConfigure();
		}

		protected abstract void OnMapConfigure();

		public IEnumerable<LambdaExpression> FindResolution(Type sourceType, Type destType, ListControlActionType actionType, string memberName)
		{
			PropMap map = Maps
				.Where(m => m.SourceType == sourceType && m.DestinationType == destType && (m.ListControlAction == actionType || m.ListControlAction == ListControlActionType.Common))
				.OrderByDescending(m => m.ListControlAction)
				.FirstOrDefault();

			if (map != null && map.Members.ContainsKey(memberName))
				return map.Members[memberName];

			return null;
		}

		public PropMapExpr<TSource, TDest> CreateMap<TSource, TDest>()
			where TSource : class, new()
			where TDest : class, new()
		{
			return new PropMapExpr<TSource, TDest>(this.CreatePropMap(typeof(TSource), typeof(TDest), ListControlActionType.Common));
		}

		public PropMapExpr<TSource, TDest> CreateMap<TSource, TDest>(ListControlActionType actionType)
			where TSource : class, new()
			where TDest : class, new()
		{
			return new PropMapExpr<TSource, TDest>(this.CreatePropMap(typeof(TSource), typeof(TDest), actionType));
		}

		private PropMap CreatePropMap(Type sourceType, Type destType, ListControlActionType actionType)
		{
			PropMap map = new PropMap(sourceType, destType, actionType);
			
			if (Maps.Any(m=>m.Equals (map)))
  				throw new InvalidOperationException("Same map already added."); 

			Maps.Add(map);
			return map;
		}

		private List<PropMap> Maps = new List<PropMap>();
	}
}
