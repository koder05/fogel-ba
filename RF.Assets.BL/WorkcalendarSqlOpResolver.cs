using System;
using System.Linq.Expressions;
using System.Data.Objects.SqlClient;

using RF.LinqExt;
using RF.BL.Model;

namespace RF.Assets.BL
{
    public class WorkcalendarSqlOpResolver : SqlOperatorResolver
    {
        public override Expression FindResolution(OperatorType op, Expression prop, Expression val)
        {
            if (op == OperatorType.Condition && Convert.ToBoolean(val.ToString()) == false)
            {
                int saturdayIndex = 6;
                int sundayIndex = 7;

                Expression<Func<WorkCalendar, bool>> foo = obj =>
                    (obj.IsWorkingDay == false && SqlFunctions.DatePart("dw", obj.Date).Value != saturdayIndex && SqlFunctions.DatePart("dw", obj.Date).Value != sundayIndex)
                        || (obj.IsWorkingDay && (SqlFunctions.DatePart("dw", obj.Date).Value == saturdayIndex || SqlFunctions.DatePart("dw", obj.Date).Value == sundayIndex));

                var main = (prop as MemberExpression).Expression as ParameterExpression;
                var modifier = new MainObjectLinkModifier(main);
                return modifier.Visit(foo.Body);
            }
            else
            {
                return base.FindResolution(op, prop, val);
            }
        }

        public override string[] GetStayConstants()
        {
            return new string[] { "dw" };
        }
    }
}
