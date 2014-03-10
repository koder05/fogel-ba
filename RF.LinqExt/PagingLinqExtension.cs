using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;  

namespace RF.LinqExt
{
	public static class PagingLinqExtension
	{
		private static readonly MethodInfo SkipMethodInfo = typeof(Queryable).GetMethod("Skip");
		private static readonly MethodInfo TakeMethodInfo = typeof(Queryable).GetMethod("Take");

		public static IQueryable<T> Paging<T>(this IOrderedQueryable<T> list, int pageIndex, int pageSize) where T : class, new()
		{
			pageSize = pageSize == 0 ? 10 : pageSize;
			int skip = pageIndex  * pageSize;
			return Taking(Skiping(list, () => skip), () => pageSize);
		}

		private static IQueryable<TSource> Skiping<TSource>(IOrderedQueryable<TSource> source, Expression<Func<int>> countAccessor)
		{
			return Parameterize(SkipMethodInfo, source, countAccessor);
		}

		private static IQueryable<TSource> Taking<TSource>(IQueryable<TSource> source, Expression<Func<int>> countAccessor)
		{
			return Parameterize(TakeMethodInfo, source, countAccessor);
		}

		private static IQueryable<TSource> Parameterize<TSource, TParameter>(MethodInfo methodInfo, IQueryable<TSource> source, Expression<Func<TParameter>> parameterAccessor)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			if (parameterAccessor == null)
				throw new ArgumentNullException("parameterAccessor");
			return source.Provider.CreateQuery<TSource>(
				Expression.Call(null, methodInfo.MakeGenericMethod(new[] { typeof(TSource) }), new[] { source.Expression, parameterAccessor.Body }));
		}
	}
}
