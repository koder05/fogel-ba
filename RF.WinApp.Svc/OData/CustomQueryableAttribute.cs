using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

using RF.BL.Model;
using EF.Sql;
using RF.LinqExt;
using RF.LinqExt.Serialization;

namespace RF.WinApp.Svc.Controllers
{
    public class CustomQueryableAttribute : QueryableAttribute
    {
        public CustomQueryableAttribute()
            : base()
        {
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            //HttpConfiguration configuration = request.GetConfiguration();
            //HttpActionDescriptor actionDescriptor = actionExecutedContext.ActionContext.ActionDescriptor;
            //IEnumerable query = responseContent.Value as IEnumerable;
            //TypeHelper.GetImplementedIEnumerableType(originalQueryType);

            //IEdmModel model = configuration.GetEdmModel();
            //if (model == null || model.GetEdmType(elementClrType) == null) model = actionDescriptor.GetEdmModel(elementClrType);
            //ODataQueryContext queryContext = new ODataQueryContext(model, elementClrType);
            //ODataQueryOptions queryOptions = new ODataQueryOptions(queryContext, request);
            //var RawValues = new ODataRawQueryOptions();

            if (actionExecutedContext.Response == null)
                return;

            ObjectContent responseContent = actionExecutedContext.Response.Content as ObjectContent;

            string filter = HttpUtility.ParseQueryString(actionExecutedContext.Request.RequestUri.Query, System.Text.Encoding.GetEncoding(1251)).Get("rf.filter");
            if (string.IsNullOrWhiteSpace(filter) == false)
            {
                FilterParameterCollection fc = JsonSerialization.FilterParameterCollectionJsonDeserialize(filter);
                IQueryable q = responseContent.Value as IQueryable;
                responseContent.Value = q.Filtering(fc, q.ElementType);
            }

            string sort = HttpUtility.ParseQueryString(actionExecutedContext.Request.RequestUri.Query).Get("rf.orderby");
            if (string.IsNullOrWhiteSpace(sort) == false)
            {
                SortParameterCollection sc = JsonSerialization.SortParameterCollectionJsonDeserialize(sort);
                IQueryable q = responseContent.Value as IQueryable;
                responseContent.Value = q.Sorting(sc, q.ElementType);
                base.EnsureStableOrdering = false;
            }

            base.OnActionExecuted(actionExecutedContext);

            IQueryable query = responseContent.Value as IQueryable;
            if (query != null && ResponseIsValid(actionExecutedContext.Response))
            {
                //visit to make parametrized EF queries
                //var modifier = new RF.LinqExt.ConstantModifier();
                //modifier.StayConstants = new string[] { "dw" };
                //Expression expr = modifier.Visit(query.Expression);
                //IQueryable ret = query.Provider.CreateQuery(expr);
                //responseContent.Value = ret;

                //actionExecutedContext.Response.TryGetContentValue(out responseObject);
                string indexofcond = HttpUtility.ParseQueryString(actionExecutedContext.Request.RequestUri.Query).Get("rf.indexof");
                if (string.IsNullOrWhiteSpace(indexofcond) == false)
                {
                    FilterParameterCollection fc = JsonSerialization.FilterParameterCollectionJsonDeserialize(indexofcond);
                    var q = PrepareInlineResult(responseContent);
                    var val = q.GetIndexOf(fc, q.ElementType);
                    actionExecutedContext.Response.Headers.Add("Index-Of-Model", val.ToString());
                }

                string inlinecountval = HttpUtility.ParseQueryString(actionExecutedContext.Request.RequestUri.Query).Get("rf.inlinecount");
                if (inlinecountval == "allpages")
                {
                    var q = PrepareInlineResult(responseContent);
                    var minfo = typeof(Queryable).GetGenericMethod("Count", new Type[] { typeof(IQueryable<>) });
                    var val = minfo.MakeGenericMethod(q.ElementType).Invoke(null, new object[] { q });
                    actionExecutedContext.Response.Headers.Add("Total-Inline-Count", val.ToString());
                }

                //break further processing
                UriBuilder builder = new UriBuilder(actionExecutedContext.Request.RequestUri.GetLeftPart(UriPartial.Authority));
                builder.Path = VirtualPathUtility.ToAbsolute(actionExecutedContext.Request.RequestUri.AbsolutePath);
                actionExecutedContext.Request.RequestUri = builder.Uri;
            }
        }

        private bool ResponseIsValid(HttpResponseMessage response)
        {
            if (response == null || response.StatusCode != HttpStatusCode.OK || !(response.Content is ObjectContent)) return false;
            return true;
        }

        /// <summary>
        /// set empty List<> to switch off rather database reading
        /// </summary>
        /// <param name="responseContent"></param>
        /// <returns></returns>
        private IQueryable PrepareInlineResult(ObjectContent responseContent)
        {
            var q = responseContent.Value as IQueryable;
            
            Type listType = typeof(List<>).MakeGenericType(q.ElementType);
            var list = Activator.CreateInstance(listType);
            responseContent.Value = (list as IEnumerable).AsQueryable();

            return q;
        }
    }
}