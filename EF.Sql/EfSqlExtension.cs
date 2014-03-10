using System;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.Objects;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

using RF.LinqExt;

namespace EF.Sql
{
    public static class EfSqlExtension
    {
        public static int GetIndexOf(this IQueryable query, FilterParameterCollection findCondition, Type type)
        {
            var minfo = typeof(EfSqlExtension).GetGenericMethod("GetIndexOf", new Type[] { typeof(IQueryable<>), typeof(FilterParameterCollection) });
            var val = minfo.MakeGenericMethod(type).Invoke(null, new object[] { query, findCondition }) as int?;
            return val ?? 0;
        }
        
        public static int GetIndexOf<T>(this IQueryable<T> query, FilterParameterCollection findCondition) where T : class, new()
        {
            return GetIndexOf<T>(query, findCondition.GetLinqCondition<T>(findCondition.PropertyNameResolver, findCondition.OperatorActionResolver));
        }
        
        public static int GetIndexOf<T>(this IQueryable<T> query, Expression<Func<T, bool>> findCondition) where T : class, new()
        {
            var q = query.Skip(0).Where(findCondition) as DbQuery<T>;
            string connectionString = RF.Common.ConfigurationManager.MainDbConnectionString;
            SqlCommand command = q.ToSqlCommand();
            string cmd = command.CommandText;
            StringBuilder sb = new StringBuilder(cmd.Length + 19 * 6);
            var selectExprs = cmd.Split(new string[] { "SELECT" }, StringSplitOptions.RemoveEmptyEntries);
            bool needInsert = true;
            for (var i = 0; i < selectExprs.Count(); i++)
            {
                string selectExpr = selectExprs[i];
                if (selectExpr.Contains("[row_number]") == false && needInsert)
                {
                    sb.AppendFormat("select {0} [row_number], {1}", i == 0 ? "top 1" : "", selectExpr);
                    //sb.Append("select * from" + selectExpr.Split(new string[] { "FROM" }, StringSplitOptions.None)[1]);
                }
                else
                {
                    //sb.Append("select *, row_number()" + selectExpr.Split(new string[] { "row_number()" }, StringSplitOptions.None)[1]);
                    sb.AppendFormat("select {0}", selectExpr);
                    needInsert = false;
                }
            }

            command.CommandText = sb.ToString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (command)
                {
                    command.Connection = connection;
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read() && reader[0] != null)
                        {
                            return Convert.ToInt32(reader[0]) - 1;
                        }
                    }
                }
            }

            return -1;
        }

        public static int GetIndexOf<T>(this DbSet<T> dbset, FilterParameterCollection filters, SortParameterCollection orderBy, Expression<Func<T, bool>> findCondition) where T : class, new()
        {
            return dbset.Filtering(filters).Sorting(orderBy).GetIndexOf(findCondition);
        }

        public static SqlCommand ToSqlCommand<T>(this DbQuery<T> query)
        {
            SqlCommand command = new SqlCommand();
            command.CommandText = query.ToString();
            var objectQuery = query.ToObjectQuery();
            foreach (var param in objectQuery.Parameters)
            {
                command.Parameters.AddWithValue(param.Name, param.Value);
            }
            return command;
        }


        public static string GetConnectionString(this DbContext db)
        {
            ObjectContext context = ((IObjectContextAdapter)db).ObjectContext;
            //http://geekswithblogs.net/rgupta/archive/2010/06/23/entity-framework-v4-ndash-tips-and-tricks.aspx
            string contextConnString = context.Connection.ConnectionString;
            Regex rx = new Regex("provider connection string=\\\"(?<cs>.+)\\\"(;|$)", RegexOptions.IgnoreCase);
            Match m = rx.Match(contextConnString);
            Group g = m.Groups["cs"];
            return g.Value;
            //metadata=reader://6272df86-5101-4949-832b-e9ebc1224ae8;provider=System.Data.SqlClient;provider connection string="Initial Catalog=FundDb;Data Source=localhost; Integrated Security=True";
        }

        private static ObjectQuery<T> ToObjectQuery<T>(this DbQuery<T> query)
        {
            var internalQuery = query.GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(field => field.Name == "_internalQuery")
                .Select(field => field.GetValue(query))
                .First();

            var objectQuery = internalQuery.GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(field => field.Name == "_objectQuery")
                .Select(field => field.GetValue(internalQuery))
                .Cast<ObjectQuery<T>>()
                .First();

            return objectQuery;
        }

        private static DbContext GetDbContext<T>(this DbSet<T> dbset) where T : class, new()
        {
            object internalSet = dbset.GetType().GetField("_internalSet", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(dbset);
            object internalContext = internalSet.GetType().GetProperty("InternalContext").GetValue(internalSet, null);
            object newContext = internalContext.GetType().GetProperty("Owner").GetValue(internalContext, null);

            return newContext as DbContext;
        }
    }
}
