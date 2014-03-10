using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.Services.Client;
using System.Web;

using RF.LinqExt;
using RF.LinqExt.Serialization;

namespace WebApi.Svc
{
    public static class DataServiceQueryExtention
    {
        public static int IndexOf<TElement>(this DataServiceQuery<TElement> q, FilterParameterCollection keyCondition)
        {
            var query = q.AddQueryOption("rf.indexof", HttpUtility.UrlEncode(keyCondition.JsonSerialize(), Encoding.GetEncoding(1251)));
            var response = query.Execute() as QueryOperationResponse<TElement>;
            int idx = -1;
            int.TryParse(response.Headers.FirstOrDefault(h => h.Key == "Index-Of-Model").Value, out idx);
            return idx;
        }

        public static int IndexOf<TElement>(this DataServiceQuery<TElement> q, Guid key)
        {
            var condition = new FilterParameterCollection();
            condition.Add("Id", key);
            return IndexOf<TElement>(q, condition);
        }

        public static int TotalCount<TElement>(this DataServiceQuery<TElement> q)
        {
            var query = q.AddQueryOption("rf.inlinecount", "allpages");
            var response = query.Execute() as QueryOperationResponse<TElement>;
            int count = -1;
            int.TryParse(response.Headers.FirstOrDefault(h => h.Key == "Total-Inline-Count").Value, out count);
            return count;
        }

        public static DataServiceQuery<TElement> AddServerCondition<TElement>(this DataServiceQuery<TElement> q, string serverConditionName)
        {
            return q.AddQueryOption("rf.condition", serverConditionName);
        }

        public static DataServiceQuery<TElement> AddFilters<TElement>(this DataServiceQuery<TElement> q, FilterParameterCollection fc)
        {
            return q.AddQueryOption("rf.filter", HttpUtility.UrlEncode(fc.JsonSerialize(), Encoding.GetEncoding(1251)));
        }

        public static DataServiceQuery<TElement> AddOrders<TElement>(this DataServiceQuery<TElement> q, SortParameterCollection sc)
        {
            return q.AddQueryOption("rf.orderby", HttpUtility.UrlEncode(sc.JsonSerialize(), Encoding.GetEncoding(1251)));
        }

        /// <summary>
        /// try get entity by Guid key from context
        /// </summary>
        public static TElement GetById<TElement>(this DataServiceQuery<TElement> q, Guid id) where TElement : class
        {
            var fi = q.Provider.GetType().GetField("Context", BindingFlags.Instance | BindingFlags.NonPublic);
            var ctx = fi.GetValue(q.Provider) as WebApiCtx;
            var entityDesc = ctx.Entities.FirstOrDefault(e => e.Identity.Contains(string.Format("{0}(guid'{1}')", q.RequestUri.PathAndQuery, id)));
            if (entityDesc != null)
                return entityDesc.Entity as TElement;
            return null;
        }
    }
}
