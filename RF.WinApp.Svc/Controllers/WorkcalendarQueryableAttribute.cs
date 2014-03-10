using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using System.Web.Http.OData.Formatter;
using System.Web.Http.OData.Properties;
using System.Web.Http.OData.Query;
using Microsoft.Data.OData;
using Microsoft.Data.Edm;
using System.Data.Objects.SqlClient;

using RF.BL;
using RF.BL.Model;
using EF;
using RF.BL.EF;
using RF.LinqExt;

namespace RF.WinApp.Svc.Controllers
{
    public class WorkcalendarQueryableAttribute : CustomQueryableAttribute
    {
        class ConditionOpRes : DefaultFilterOperatorResolver
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
        
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            string condition = HttpUtility.ParseQueryString(actionExecutedContext.Request.RequestUri.Query).Get("rf.condition");
            if (condition == "IsHoliday")
            {
                //bug in HttpRequestMessage : RequestUri setter not change Properties. Do it manually.
                //if (request.Properties.ContainsKey(System.Web.Http.Hosting.HttpPropertyKeys.RequestQueryNameValuePairsKey))
                //{
                //    request.Properties.Remove(System.Web.Http.Hosting.HttpPropertyKeys.RequestQueryNameValuePairsKey);
                //    request.GetQueryNameValuePairs();
                //}

                ObjectContent responseContent = actionExecutedContext.Response.Content as ObjectContent;
                IQueryable<WorkCalendar> query = null;
                if (responseContent != null)
                    query = responseContent.Value as IQueryable<WorkCalendar>;

                if (query != null)
                {
                    FilterParameterCollection fc = new FilterParameterCollection(typeof(WorkCalendar));
                    fc.Add("IsWorkingDay", false, OperatorType.Condition);
                    fc.OperatorActionResolver = new ConditionOpRes();
                    responseContent.Value = query.Filtering(fc);
                }
            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}