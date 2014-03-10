using System;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Globalization;

using RF.Common;
using RF.LinqExt;

namespace RF.Geo.Sql
{
    internal static class DbHelper
    {
        internal static SqlConnection CreateMainDbConnection()
        {
            ConnectionStringSettings settings = System.Configuration.ConfigurationManager.ConnectionStrings["OPS"];
            if (settings == null || string.IsNullOrEmpty(settings.ConnectionString))
                throw new ConfigurationErrorsException("OPS connection string is absent.");

            string cs = settings.ConnectionString;
            SqlConnection con = new SqlConnection(cs);
            return con;
        }

        internal static IEnumerable<T> Select<T>(string cmdText, int pageIndex, int pageSize, string orderBy, params SqlParameter[] parameters) where T : class
        {
            return CreateCommand(CreateMainDbConnection(), cmdText, parameters).AddPaging(pageIndex, pageSize, orderBy).Select<T>();
        }

        internal static IEnumerable<T> Select<T>(string cmdText, params SqlParameter[] parameters) where T : class
        {
            return CreateCommand(CreateMainDbConnection(), cmdText, parameters).Select<T>();
        }

        internal static IEnumerable<T> Select<T>(this SqlCommand cmd) where T : class
        {
            var doc = cmd.SelectRawXml(null, null);
            foreach (XmlNode el in doc.SelectNodes("//row"))
                yield return Utils.GetObjectFromXml<T>(el);
        }

        internal static IEnumerable<T> SelectComplex<T>(string cmdText, int pageIndex, int pageSize, string orderBy, params SqlParameter[] parameters) where T : class
        {
            var doc = CreateCommand(CreateMainDbConnection(), cmdText, parameters).AddPaging(pageIndex, pageSize, orderBy).SelectExplicitXml(null);
            foreach (XmlNode el in doc.SelectNodes("/root/row"))
                yield return Utils.GetObjectFromXml<T>(el);
        }

        internal static IEnumerable<T> SelectComplex<T>(string cmdText, params SqlParameter[] parameters) where T : class
        {
            var doc = CreateCommand(CreateMainDbConnection(), cmdText, parameters).SelectExplicitXml(null);
            foreach (XmlNode el in doc.SelectNodes("/root/row"))
                yield return Utils.GetObjectFromXml<T>(el);
        }

        internal static int SelectCount(string cmdText, params SqlParameter[] parameters)
        {
            return CreateCommand(CreateMainDbConnection(), cmdText, parameters).SelectCount();
        }

        internal static int SelectCount(this SqlCommand cmd)
        {
            cmd.CommandText = EnvelopeExpression(cmd.CommandText, "count(*)");
            var o = ExecuteScalar(cmd);
            return (int)o;
        }

        public static SqlCommand CreateCommand(string sqlStatement, params SqlParameter[] parameters)
        {
            return CreateCommand(CreateMainDbConnection(), sqlStatement, parameters);
        }

        private static SqlCommand CreateCommand(SqlConnection con, string sqlStatement, params SqlParameter[] parameters)
        {
            if (con == null)
                throw new ArgumentNullException("con");

            SqlCommand cmd = new SqlCommand(sqlStatement, con);
            cmd.CommandTimeout = 60;
            FillParameters(cmd, parameters);
            return cmd;
        }

        internal static void Execute(this SqlCommand cmd)
        {
            OpenConnection(cmd.Connection);
            try
            {
                cmd.ExecuteNonQuery();
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        internal static object ExecuteScalar(SqlCommand cmd)
        {
            OpenConnection(cmd.Connection);
            object result = null;
            try
            {
                result = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return result;
        }

        internal static XmlDocument GetXml(this SqlCommand cmd)
        {
            XmlDocument doc = new XmlDocument();
            OpenConnection(cmd.Connection);

            try
            {
                using (XmlReader reader = cmd.ExecuteXmlReader())
                {
                    doc.Load(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return doc;
        }


        internal static XmlDocument SelectRawXml(this SqlCommand cmd, string rootName, string rowName)
        {
            Args.IsNotNull(cmd, "cmd");
            cmd.CommandText += string.Format(" for xml raw ('{0}'), root ('{1}'), BINARY BASE64"
                , string.IsNullOrEmpty(rowName) ? "row" : rowName
                , string.IsNullOrEmpty(rootName) ? "root" : rootName);

            return cmd.GetXml();
        }

        internal static XmlDocument SelectExplicitXml(this SqlCommand cmd, string rootName)
        {
            Args.IsNotNull(cmd, "cmd");
            cmd.CommandText += string.Format(" for xml explicit, root ('{0}')", string.IsNullOrEmpty(rootName) ? "root" : rootName);
            return cmd.GetXml();
        }

        internal static SqlParameter CreateParameter(string parameterName, object value)
        {
            return new SqlParameter(parameterName, value ?? DBNull.Value);
        }

        internal static SqlCommand AddParameter(this SqlCommand cmd, string parameterName, object value)
        {
            var par = CreateParameter(parameterName, value);
            cmd.Parameters.Add(par);
            return cmd;
        }

        public static DataView SelectDataView(SqlCommand cmd)
        {
            DataTable dt = SelectDataTable(cmd);
            return dt.DefaultView;
        }

        public const string CommonTableSeparator = "/*EndCommonTable*/";
        internal static string EnvelopeExpression(string cmdText, string envelope)
        {
            string[] split = cmdText.Split(new string[] { CommonTableSeparator }, StringSplitOptions.None);
            string cte = split.Length > 1 ? split[0] + CommonTableSeparator : "";
            string select = split.Length > 1 ? split[1] : cmdText;
            return string.Format("{0} select {1} from ({2})q", cte, envelope, select);
        }

        public static List<T> SelectScalarList<T>(this SqlCommand cmd)
        {
            return SelectScalarEnumerable<T>(cmd).ToList();
        }

        public static List<T> SelectScalarList<T>(string cmdText, params SqlParameter[] parameters)
        {
            return SelectScalarList<T>(CreateCommand(cmdText, parameters));
        }

        public static SqlCommand ExpandParameterToList<T>(this SqlCommand cmd, string parameterName, IEnumerable<T> parameterValues)
        {
            if (cmd == null)
                throw new ArgumentNullException("cmd");

            if (cmd.CommandText.Contains(parameterName) == false)
                throw new ArgumentException(string.Format("CommandText of SqlCommand must reference to parameter \"{0}\".", parameterName));

            List<T> valueList = new List<T>(parameterValues);
            if (valueList.Count == 0)
                throw new ArgumentException("Empty parameterValues are not permitted.", "parameterValues");

            if (valueList.Count == 1)
                cmd.Parameters.Add(CreateParameter(parameterName, valueList[0]));

            else
            {
                List<string> expandedParameters = new List<string>();
                for (int i = 0; i < valueList.Count; i++)
                    expandedParameters.Add(parameterName + "_" + i.ToString());

                bool isGuid = (typeof(T) == typeof(Guid));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < expandedParameters.Count; i++)
                {
                    if (i > 0)
                        sb.Append(", ");

                    if (isGuid)
                        sb.Append(CreateSqlLiteral(valueList[i]));

                    else
                        sb.Append(expandedParameters[i]);
                }

                string expandedParametersString = sb.ToString();

                Regex equalRegex = new Regex(string.Format(@"=\s*(?<par>{0})", parameterName));
                cmd.CommandText = equalRegex.Replace
                (
                    cmd.CommandText,
                    delegate(Match match)
                    {
                        return string.Format(" in ({0})", expandedParametersString);
                    }
                );

                Regex notEqualRegex = new Regex(string.Format(@"((!=)|(<>))\s*(?<par>{0})", parameterName));
                cmd.CommandText = notEqualRegex.Replace
                (
                    cmd.CommandText,
                    delegate(Match match)
                    {
                        return string.Format(" not in ({0})", expandedParametersString);
                    }
                );

                if (isGuid == false)
                {
                    for (int i = 0; i < expandedParameters.Count; i++)
                        cmd.Parameters.Add(CreateParameter(expandedParameters[i], valueList[i]));
                }
            }
            return cmd;
        }

        public static SqlCommand AddFiltering(this SqlCommand cmd, FilterParameterCollection filters)
        {
            if (cmd == null)
                throw new ArgumentNullException("cmd");

            if (filters == null || filters.Count == 0)
                return cmd;

            cmd.CommandText = EnvelopeExpression(cmd.CommandText, "*") + " where 1=1";

            StringBuilder sb = new StringBuilder();

            Dictionary<FilterParameter, string> filterParameter_SqlPars = new Dictionary<FilterParameter, string>();
            const string parameterPrefix = "@";

            Dictionary<string, int> columnStats = new Dictionary<string, int>();
            Dictionary<string, int> columnCurStats = new Dictionary<string, int>();
            Dictionary<string, int> orGroupStats = new Dictionary<string, int>();
            Dictionary<string, int> andGroupStats = new Dictionary<string, int>();

            foreach (FilterParameter fp in filters.Where(f => f.Operator != OperatorType.Top))
            {
                if (columnStats.ContainsKey(fp.ColumnName) == false)
                    columnStats.Add(fp.ColumnName, 0);

                columnStats[fp.ColumnName]++;

                if (!string.IsNullOrEmpty(fp.AndGroupName))
                {
                    if (andGroupStats.ContainsKey(fp.AndGroupName) == false)
                        andGroupStats.Add(fp.AndGroupName, 1);
                    andGroupStats[fp.AndGroupName]++;
                }
                if (!string.IsNullOrEmpty(fp.OrGroupName))
                {
                    if (orGroupStats.ContainsKey(fp.OrGroupName) == false)
                        orGroupStats.Add(fp.OrGroupName, 1);
                    orGroupStats[fp.OrGroupName]++;
                }
            }

            foreach (FilterParameter fp in filters.Where(f => f.Operator != OperatorType.Top))
            {
                string safeParameterBaseName = fp.ColumnName.Replace("[", "").Replace("]", "").Replace(".", "__");

                if (columnStats[fp.ColumnName] == 1)
                    filterParameter_SqlPars.Add(fp, parameterPrefix + safeParameterBaseName);

                else
                {
                    if (columnCurStats.ContainsKey(fp.ColumnName) == false)
                        columnCurStats.Add(fp.ColumnName, 0);

                    columnCurStats[fp.ColumnName]++;
                    filterParameter_SqlPars.Add(fp, parameterPrefix + safeParameterBaseName + "_" + columnCurStats[fp.ColumnName]);
                }
            }

            string orGroup = string.Empty;
            string andGroup = string.Empty;

            foreach (FilterParameter fp in filters.Where(f => f.Operator != OperatorType.Top))
            {
                OperatorType op = fp.Operator;
                string columnNameWithDelimiter = string.Empty;

                if (
                        (fp.Value == null
                        || (fp.Value.GetType() == typeof(Guid) && (Guid)fp.Value == Guid.Empty)
                        || (fp.Value.GetType().IsArray && ((Array)fp.Value).Length == 0)
                    /*HARDCODE*/
                        || (fp.Value.GetType() == typeof(bool) && (bool)fp.Value == false)
                        || (fp.Value.GetType() == typeof(string) && string.IsNullOrEmpty((string)fp.Value) && op == OperatorType.Query)
                    )
                    && op != OperatorType.IsFalse
                    && op != OperatorType.IsTrue
                    && op != OperatorType.IsNull
                    && op != OperatorType.IsNotNull
                    && op != OperatorType.EqualsWithNull
                    )
                {
                    continue;
                }
                else if (op != OperatorType.None)
                {
                    if (!string.IsNullOrEmpty(orGroup) && orGroup != fp.OrGroupName)
                    {
                        columnNameWithDelimiter += " ) ";
                        orGroup = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(andGroup) && andGroup != fp.AndGroupName)
                    {
                        columnNameWithDelimiter += " ) ";
                        andGroup = string.Empty;
                    }
                    if (string.IsNullOrEmpty(orGroup) && string.IsNullOrEmpty(andGroup))
                    {
                        columnNameWithDelimiter += " and ";
                    }
                    if (!string.IsNullOrEmpty(fp.OrGroupName) && !string.IsNullOrEmpty(fp.AndGroupName) && fp.OrGroupName != orGroup && fp.AndGroupName != andGroup)
                    {
                        columnNameWithDelimiter += " (( ";
                        orGroup = fp.OrGroupName;
                        andGroup = fp.AndGroupName;
                    }
                    else if (!string.IsNullOrEmpty(fp.OrGroupName) && !string.IsNullOrEmpty(fp.AndGroupName) && fp.OrGroupName == orGroup && fp.AndGroupName != andGroup)
                    {
                        columnNameWithDelimiter += " or ( ";
                        andGroup = fp.AndGroupName;
                    }
                    else if (!string.IsNullOrEmpty(fp.OrGroupName) && !string.IsNullOrEmpty(fp.AndGroupName) && fp.OrGroupName != orGroup && fp.AndGroupName == andGroup)
                    {
                        columnNameWithDelimiter += " and ( ";
                        orGroup = fp.OrGroupName;
                    }
                    else if (!string.IsNullOrEmpty(fp.OrGroupName) && !string.IsNullOrEmpty(fp.AndGroupName) && fp.OrGroupName == orGroup && fp.AndGroupName == andGroup)
                    {
                        //если группа And является вторичной по отношению к группе Or [например ((X and Y) or Z)]
                        if (andGroupStats[fp.AndGroupName] < orGroupStats[fp.OrGroupName])
                            columnNameWithDelimiter += " and ";
                        else
                            columnNameWithDelimiter += " or ";
                    }
                    else if (string.IsNullOrEmpty(fp.OrGroupName) && !string.IsNullOrEmpty(fp.AndGroupName) && fp.AndGroupName != andGroup)
                    {
                        columnNameWithDelimiter += " ( ";
                        andGroup = fp.AndGroupName;
                    }
                    else if (string.IsNullOrEmpty(fp.OrGroupName) && !string.IsNullOrEmpty(fp.AndGroupName) && fp.AndGroupName == andGroup)
                    {
                        columnNameWithDelimiter += " and ";
                    }
                    else if (!string.IsNullOrEmpty(fp.OrGroupName) && string.IsNullOrEmpty(fp.AndGroupName) && fp.OrGroupName != orGroup)
                    {
                        columnNameWithDelimiter += " ( ";
                        orGroup = fp.OrGroupName;
                    }
                    else if (!string.IsNullOrEmpty(fp.OrGroupName) && string.IsNullOrEmpty(fp.AndGroupName) && fp.OrGroupName == orGroup)
                    {
                        columnNameWithDelimiter += " or ";
                    }

                    columnNameWithDelimiter += fp.ColumnName + " ";
                }

                if (fp.Value != null && fp.Value is IEnumerable && fp.Value.GetType() != typeof(string))
                {
                    op = OperatorType.In;
                    IEnumerable en = (IEnumerable)fp.Value;
                    if (en.GetEnumerator().MoveNext() == false)
                        continue;

                    sb.Append(columnNameWithDelimiter);
                    int inParamIndex = 0;
                    sb.Append(" in (");
                    foreach (object o in en)
                    {
                        if (inParamIndex > 0)
                            sb.Append(",");

                        bool isGuid = (o != null && o.GetType() == typeof(Guid));
                        if (isGuid)
                            sb.Append(CreateSqlLiteral(o));

                        else
                        {
                            string baseParameter = filterParameter_SqlPars[fp];
                            string arrParameter = baseParameter + "_" + (inParamIndex + 1);
                            cmd.AddParameter(arrParameter, o);
                            sb.Append(arrParameter);
                        }

                        inParamIndex++;
                    }
                    sb.Append(")");

                    continue;
                }

                string format = "";

                if (op == OperatorType.IsFalse)
                {
                    sb.Append(columnNameWithDelimiter);
                    sb.Append("= 'false'");
                    continue;
                }

                else if (op == OperatorType.IsTrue)
                {
                    sb.Append(columnNameWithDelimiter);
                    sb.Append("= 'true'");
                    continue;
                }

                else if (op == OperatorType.IsNull)
                {
                    sb.Append(columnNameWithDelimiter);
                    sb.Append("is null");
                    continue;
                }

                else if (op == OperatorType.IsNotNull)
                {
                    sb.Append(columnNameWithDelimiter);
                    sb.Append("is not null");
                    continue;
                }

                else if (op == OperatorType.Like)
                    format = "like ('%' + {0} + '%')";

                else if (op == OperatorType.NotLike)
                    format = "not like ('%' + {0} + '%')";

                else if (op == OperatorType.StartsWith)
                    format = "like ({0} + '%')";

                else if (op == OperatorType.Equals)
                    format = "= {0}";

                else if (op == OperatorType.EqualsWithNull && fp.Value != null && fp.Value != DBNull.Value)
                    format = "= {0}";

                else if (op == OperatorType.EqualsWithNull && (fp.Value == null || fp.Value == DBNull.Value))
                    format = "is null";

                else if (op == OperatorType.NotEquals)
                    format = "!= {0}";

                else if (op == OperatorType.LessOrEquals)
                    format = "<= {0}";

                else if (op == OperatorType.MoreOrEquals)
                    format = ">= {0}";

                else if (op == OperatorType.LessThan)
                    format = "< {0}";

                else if (op == OperatorType.MoreThan)
                    format = "> {0}";

                if (op == OperatorType.LessOrEquals || op == OperatorType.LessThan)
                {
                    if (fp.Value is DateTime)
                    {
                        DateTime dt = (DateTime)fp.Value;
                        if (dt.Hour == 0 && dt.Minute == 0 && dt.Second == 0 && dt.Millisecond == 0)
                            fp.Value = dt.AddDays(1).AddSeconds(-1);
                    }
                }

                else if (op == OperatorType.Like)
                {
                    if (fp.Value is string)
                    {
                        fp.Value = ((string)fp.Value).Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]");;
                    }
                }

                string parameterName = filterParameter_SqlPars[fp];
                object val = fp.Value;
                if (val == null)
                    val = DBNull.Value;

                cmd.Parameters.AddWithValue(parameterName, val);
                sb.Append(columnNameWithDelimiter);
                sb.AppendFormat(format, parameterName);
            }

            if (!string.IsNullOrEmpty(orGroup)) sb.Append(" ) ");
            if (!string.IsNullOrEmpty(andGroup)) sb.Append(" ) ");

            if (cmd.Parameters.Count > 2100)
                throw new ArgumentException("Задано слишком много параметров. Максимальное количество параметров: 2100.");

            cmd.CommandText += sb.ToString();
            return cmd;
        }

        public static SqlCommand AddPaging(this SqlCommand cmd, int pageIndex, int pageSize, string orderBy)
        {
            Args.IsNotNull(cmd, "cmd");
            Args.IsTrue(!String.IsNullOrWhiteSpace(orderBy), "orderBy must be non-empty string.");

            pageIndex = Math.Max(pageIndex, 0);
            pageSize = Math.Max(pageSize, 1);

            int skip = pageIndex * pageSize;

            cmd.CommandText = EnvelopeExpression(cmd.CommandText, string.Format("(row_number() over (order by {0})) as RowNumber, *", orderBy));
            cmd.CommandText = EnvelopeExpression(cmd.CommandText, string.Format("top ({0}) *", pageSize)) + " where RowNumber > @skip";
            cmd.Parameters.AddWithValue("@skip", skip);
            return cmd;
        }

        private static IEnumerable<T> SelectScalarEnumerable<T>(SqlCommand cmd)
        {
            Args.IsNotNull(cmd, "cmd");
            OpenConnection(cmd.Connection);
            try
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        yield return (T)reader[0];
                }
            }
            finally
            {
                cmd.Connection.Close();
            }
        }
        
        private static DataTable SelectDataTable(SqlCommand cmd)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            OpenConnection(cmd.Connection);
            try
            {
                da.Fill(dt);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return dt;
        }

        private static void OpenConnection(SqlConnection con)
        {
            if (con.State != ConnectionState.Open)
                con.Open();
        }

        private static void FillParameters(SqlCommand cmd, SqlParameter[] parameters)
        {
            if (parameters != null && parameters.Length > 0)
                cmd.Parameters.AddRange(parameters);
        }

        private static string CreateSqlLiteral(object o)
        {
            if (o == null || o == DBNull.Value)
                return " null ";

            Type t = o.GetType();
            if (t == typeof(Guid))
                return string.Format(" '{0}' ", o);

            if
            (
                t == typeof(int) ||
                t == typeof(decimal) ||
                t == typeof(float) ||
                t == typeof(double) ||
                t == typeof(long) ||
                t == typeof(byte) ||
                t == typeof(short) ||
                t == typeof(ushort) ||
                t == typeof(uint) ||
                t == typeof(ulong)
            )
                return string.Format(NumberFormatInfo.InvariantInfo, " {0} ", o);

            if (t == typeof(string))
                return string.Format(" '{0}' ", ((string)o).Replace("'", "''"));

            if (t == typeof(char))
                return string.Format(" '{0}' ", o);

            if (t == typeof(DateTime))
                return string.Format(" cast('{0:yyyy-MM-ddTHH:mm:ss.fff}' as datetime) ", o);

            if (t.IsEnum)
            {
                Type underlyingType = Enum.GetUnderlyingType(t);
                object value = Convert.ChangeType(o, underlyingType); 
                return string.Format(NumberFormatInfo.InvariantInfo, " {0} ", value);
            }

            if (t == typeof(byte[]))
            {
                StringBuilder sb = new StringBuilder();
                byte[] bytes = (byte[])o;
                sb.Append(" 0x");
                for (int i = 0; i < bytes.Length; i++)
                    sb.AppendFormat("{0:x2}", bytes[i]);

                sb.Append(" ");

                return sb.ToString();
            }

            if (t == typeof(bool))
                return string.Format(" cast({0} as bit) ", Convert.ToByte((bool)o));

            throw new NotSupportedException(string.Format("Type \"{0}\" is not supported.", t.FullName));
        }

    }
}
