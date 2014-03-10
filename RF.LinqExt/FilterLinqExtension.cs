using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RF.LinqExt
{
	public static class FilterLinqExtension
	{
        public static IQueryable Filtering(this IQueryable list, FilterParameterCollection filters, Type type)
        {
            var minfo = typeof(FilterLinqExtension).GetGenericMethod("Filtering", new Type[] { typeof(IQueryable<>), typeof(FilterParameterCollection) });
            return minfo.MakeGenericMethod(type).Invoke(null, new object[] { list, filters }) as IQueryable;
        }

        public static IQueryable<T> Filtering<T>(this IQueryable<T> list, FilterParameterCollection filters) where T : class, new()
        {
            if (filters != null)
                return list.Where(filters.GetLinqCondition<T>(filters.PropertyNameResolver, filters.OperatorActionResolver));

            return list;
        }

        public static IQueryable<T> Filtering<T>(this IQueryable<T> list, FilterParameterCollection filters, IFilterOperatorResolver opResolver) where T : class, new()
		{
			if (filters != null)
                return list.Where(filters.GetLinqCondition<T>(filters.PropertyNameResolver, opResolver));

			return list;
		}

		public static IEnumerable<T> Filtering<T>(this IEnumerable<T> list, FilterParameterCollection filters) where T : class, new()
		{
			if (filters != null)
                return list.Where(filters.GetLinqCondition<T>(filters.PropertyNameResolver, filters.OperatorActionResolver).Compile());

			return list;
		}

        public static IQueryable<T> Filtering<T>(this IQueryable<T> list, FilterParameterCollection filters, IFilterSortPropResolver propResolver, IFilterOperatorResolver opResolver) where T : class, new()
        {
            if (filters != null)
                return list.Where(filters.GetLinqCondition<T>(propResolver, opResolver));

            return list;
        }

        public static IEnumerable<T> Filtering<T>(this IEnumerable<T> list, FilterParameterCollection filters, IFilterSortPropResolver propResolver, IFilterOperatorResolver opResolver) where T : class, new()
        {
            if (filters != null)
                return list.Where(filters.GetLinqCondition<T>(propResolver, opResolver).Compile());

            return list;
        }
	}
}
