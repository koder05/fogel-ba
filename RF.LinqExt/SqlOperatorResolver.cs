using System;
using System.Linq.Expressions;
using System.Data.Objects.SqlClient;

namespace RF.LinqExt
{
    public class SqlOperatorResolver : DefaultFilterOperatorResolver
    {
        public override Expression FindResolution(OperatorType op, Expression prop, Expression val)
        {
            if (op == OperatorType.Like)
            {
                string s = val.ToString().Trim('"').Replace('?', '_').Replace('*', '%');
                return Expression.GreaterThan(Expression.Call(typeof(SqlFunctions).GetMethod("PatIndex"), Expression.Constant(s, s.GetType()), prop), Expression.Constant(0, typeof(Nullable<int>)));
            }
            else
            {
                return base.FindResolution(op, prop, val);
            }
        }
    }
}
