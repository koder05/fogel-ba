using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Linq.Expressions;

using RF.LinqExt;

namespace RF.WinApp
{
    public interface IDataView
    {
        Type ModelType { get; }
        object ActivateEmptyModel();
        int GetListCount(FilterParameterCollection filters);
        int GetIndexOf(object o, FilterParameterCollection filters, SortParameterCollection orderBy);
        IEnumerable<object> GetList(FilterParameterCollection filters, int pageIndex, int pageSize, SortParameterCollection orderBy);
        void Create(object o);
        void Delete(IEnumerable<object> pool);
        void Update(object o);
    }

    public class ListFilterOperatorResolver : DefaultFilterOperatorResolver
    {
        public override Expression FindResolution(OperatorType op, Expression prop, Expression val)
        {
            if (op == OperatorType.Like)
            {
                string s = val.ToString().Trim('"');

                if (s.StartsWith("*"))
                {
                    return Expression.Call(prop, typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }), Expression.Constant(s.TrimStart('*'), s.GetType()));
                }

                if (s.EndsWith("*"))
                {
                    return Expression.Call(prop, typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }), Expression.Constant(s.TrimEnd('*'), s.GetType()));
                }

                //var m = System.Data.Linq.SqlClient.SqlMethods;
                return base.FindResolution(op, prop, val);
            }
            else
            {
                return base.FindResolution(op, prop, val);
            }
        }
    }
}
