using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace RF.LinqExt
{
    public static class SortLinqExtension
    {
        #region PrivateCommon

        private static MethodInfo GetOrderMethod(Type sourceListType, string methodName)
        {
            return sourceListType
                .GetMethods()
                .Single(method => method.Name == methodName && method.IsGenericMethodDefinition && method.GetGenericArguments().Length == 2 && method.GetParameters().Length == 2);
        }

        private static IEnumerable<LambdaExpression> GetExpressionList<T>(SortParameter par, IFilterSortPropResolver propResolver) where T : class, new()
        {
            IEnumerable<LambdaExpression> exprList = null;
            if (propResolver != null)
            {
                exprList = propResolver.FindResolution(par.Type ?? typeof(T), typeof(T), ListControlActionType.Sorting, par.ColumnName);
            }

            if (exprList == null || exprList.Count() == 0)
            {
                exprList = new LambdaExpression[] { par.GetLinqSortExpr<T>() };
            }

            return exprList;
        }

        #endregion

        #region PrivateOnEnumerable

        private static object InvokeSortParameterOnEnumerable<T>(object sourceList, LambdaExpression expr, ListSortDirection sortDir, string ascMethodName, string descMethodName)
            where T : class, new()
        {
            Type t = expr.Type.GetGenericArguments()[1];
            return GetOrderMethod(typeof(Enumerable), sortDir == ListSortDirection.Descending ? descMethodName : ascMethodName)
                        .MakeGenericMethod(typeof(T), t).Invoke(null, new object[] { sourceList, expr.Compile() });
        }

        private static object InvokeSortParameterOnEnumerable<T>(object sourceList, SortParameter par, IFilterSortPropResolver propResolver, string ascMethodName, string descMethodName)
            where T : class, new()
        {
            bool isFirst = true;
            foreach (var expr in GetExpressionList<T>(par, propResolver))
            {
                if (isFirst)
                {
                    sourceList = InvokeSortParameterOnEnumerable<T>(sourceList, expr, par.SortDirection, ascMethodName, descMethodName);
                    isFirst = false;
                }
                else
                {
                    sourceList = InvokeSortParameterOnEnumerable<T>(sourceList, expr, par.SortDirection, "ThenBy", "ThenByDescending");
                }
            }

            return sourceList;
        }

        private static object InvokeSortParameterOnEnumerableFirst<T>(object sourceList, SortParameter par, IFilterSortPropResolver propResolver) where T : class, new()
        {
            return InvokeSortParameterOnEnumerable<T>(sourceList, par, propResolver, "OrderBy", "OrderByDescending");
        }

        private static object InvokeSortParameterOnEnumerableNext<T>(object sourceList, SortParameter par, IFilterSortPropResolver propResolver) where T : class, new()
        {
            return InvokeSortParameterOnEnumerable<T>(sourceList, par, propResolver, "ThenBy", "ThenByDescending");
        }

        #endregion

        #region PrivateOnQueryable

        private static object InvokeSortParameterOnQueryable<T>(object sourceList, LambdaExpression expr, ListSortDirection sortDir, string ascMethodName, string descMethodName) where T : class, new()
        {
            Type t = expr.Type.GetGenericArguments()[1];
            return GetOrderMethod(typeof(Queryable), sortDir == ListSortDirection.Descending ? descMethodName : ascMethodName)
                        .MakeGenericMethod(typeof(T), t).Invoke(null, new object[] { sourceList, expr });
        }

        private static object InvokeSortParameterOnQueryable<T>(object sourceList, SortParameter par, IFilterSortPropResolver propResolver, string ascMethodName, string descMethodName)
            where T : class, new()
        {
            bool isFirst = true;
            foreach (var expr in GetExpressionList<T>(par, propResolver))
            {
                if (isFirst)
                {
                    sourceList = InvokeSortParameterOnQueryable<T>(sourceList, expr, par.SortDirection, ascMethodName, descMethodName);
                    isFirst = false;
                }
                else
                {
                    sourceList = InvokeSortParameterOnQueryable<T>(sourceList, expr, par.SortDirection, "ThenBy", "ThenByDescending");
                }
            }

            return sourceList;
        }

        private static object InvokeSortParameterOnQueryableFirst<T>(object sourceList, SortParameter par, IFilterSortPropResolver propResolver) where T : class, new()
        {
            return InvokeSortParameterOnQueryable<T>(sourceList, par, propResolver, "OrderBy", "OrderByDescending");
        }

        private static object InvokeSortParameterOnQueryableNext<T>(object sourceList, SortParameter par, IFilterSortPropResolver propResolver) where T : class, new()
        {
            return InvokeSortParameterOnQueryable<T>(sourceList, par, propResolver, "ThenBy", "ThenByDescending");
        }

        #endregion

        private static IOrderedEnumerable<T> Sorting<T>(this IOrderedEnumerable<T> list, SortParameterCollection sc, IFilterSortPropResolver propResolver, bool skipFirst) where T : class, new()
        {
            IOrderedEnumerable<T> result = list;
            foreach (var par in sc.Skip(skipFirst ? 1 : 0))
            {
                result = (IOrderedEnumerable<T>)InvokeSortParameterOnEnumerableNext<T>(result, par, propResolver);
            }

            if (sc.DefaultOrder != null)
                foreach (var par in sc.DefaultOrder)
                {
                    if (sc.Any(p => p.Type == typeof(T) && p.ColumnName == par.ColumnName) == false)
                        result = (IOrderedEnumerable<T>)InvokeSortParameterOnEnumerableNext<T>(result, par, propResolver);
                }

            return result;
        }

        private static IOrderedQueryable<T> Sorting<T>(this IOrderedQueryable<T> list, SortParameterCollection sc, IFilterSortPropResolver propResolver, bool skipFirst) where T : class, new()
        {
            IOrderedQueryable<T> result = list;
            foreach (var par in sc.Skip(skipFirst ? 1 : 0))
            {
                result = (IOrderedQueryable<T>)InvokeSortParameterOnQueryableNext<T>(result, par, propResolver);
            }

            if (sc.DefaultOrder != null)
                foreach (var par in sc.DefaultOrder)
                {
                    if (sc.Any(p => p.Type == typeof(T) && p.ColumnName == par.ColumnName) == false)
                        result = (IOrderedQueryable<T>)InvokeSortParameterOnQueryableNext<T>(result, par, propResolver);
                }

            return result;
        }

        public static IOrderedEnumerable<T> Sorting<T>(this IEnumerable<T> list, SortParameterCollection sc, IFilterSortPropResolver propResolver) where T : class, new()
        {
            foreach (var par in sc)
            {
                return ((IOrderedEnumerable<T>)InvokeSortParameterOnEnumerableFirst<T>(list, par, propResolver)).Sorting(sc, propResolver, true);
            }

            if (sc.DefaultOrder != null)
                foreach (var par in sc.DefaultOrder)
                {
                    return ((IOrderedEnumerable<T>)InvokeSortParameterOnEnumerableFirst<T>(list, par, propResolver)).Sorting(sc.DefaultOrder, propResolver, true);
                }

            return list.OrderBy<T, int>(m => 1);
        }

        public static IOrderedEnumerable<T> Sorting<T>(this IOrderedEnumerable<T> list, SortParameterCollection sc, IFilterSortPropResolver propResolver) where T : class, new()
        {
            return Sorting<T>(list, sc, propResolver, false);
        }

        public static IOrderedQueryable<T> Sorting<T>(this IQueryable<T> list, SortParameterCollection sc, IFilterSortPropResolver propResolver) where T : class, new()
        {
            foreach (var par in sc)
            {
                return ((IOrderedQueryable<T>)InvokeSortParameterOnQueryableFirst<T>(list, par, propResolver)).Sorting(sc, propResolver, true);
            }

            if (sc.DefaultOrder != null)
                foreach (var par in sc.DefaultOrder)
                {
                    return ((IOrderedQueryable<T>)InvokeSortParameterOnQueryableFirst<T>(list, par, propResolver)).Sorting(sc.DefaultOrder, propResolver, true);
                }

            return list.OrderBy<T, int>(m => 1);
        }

        public static IOrderedQueryable<T> Sorting<T>(this IOrderedQueryable<T> list, SortParameterCollection sc, IFilterSortPropResolver propResolver) where T : class, new()
        {
            return Sorting<T>(list, sc, propResolver, false);
        }

        public static IOrderedEnumerable<T> Sorting<T>(this IEnumerable<T> list, SortParameterCollection sc) where T : class, new()
        {
            return Sorting<T>(list, sc, sc.PropertyNameResolver);
        }

        public static IOrderedEnumerable<T> Sorting<T>(this IOrderedEnumerable<T> list, SortParameterCollection sc) where T : class, new()
        {
            return Sorting<T>(list, sc, sc.PropertyNameResolver, false);
        }

        public static IOrderedQueryable<T> Sorting<T>(this IQueryable<T> list, SortParameterCollection sc) where T : class, new()
        {
            return Sorting<T>(list, sc, sc.PropertyNameResolver);
        }

        public static IOrderedQueryable<T> Sorting<T>(this IOrderedQueryable<T> list, SortParameterCollection sc) where T : class, new()
        {
            return Sorting<T>(list, sc, sc.PropertyNameResolver, false);
        }

        public static IOrderedQueryable Sorting(this IQueryable list, SortParameterCollection sc, Type type)
        {
            var minfo = typeof(SortLinqExtension).GetGenericMethod("Sorting", new Type[] { typeof(IQueryable<>), typeof(SortParameterCollection) });
            return minfo.MakeGenericMethod(type).Invoke(null, new object[] { list, sc }) as IOrderedQueryable;
        }
    }
}
