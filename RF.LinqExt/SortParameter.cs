using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace RF.LinqExt
{
	public class SortParameter
	{
		public static readonly ListSortDirection DefaultOperator = ListSortDirection.Ascending;

		private string m_ColumnName = "";
		private ListSortDirection m_SortDirection = DefaultOperator;

        public SortParameter(string columnName, ListSortDirection dir)
            : this(null, columnName, dir)
        {
        }
        
        public SortParameter(Type type, string columnName)
			: this(type, columnName, DefaultOperator)
		{
		}

		public SortParameter(Type type, string columnName, ListSortDirection dir)
		{
			if (columnName == null)
				throw new ArgumentNullException("columnName");

			this.Type = type;
			this.m_ColumnName = columnName;
			this.m_SortDirection = dir;
		}

		public string ColumnName
		{
			get { return this.m_ColumnName; }
			set { this.m_ColumnName = value; }
		}

		public ListSortDirection SortDirection
		{
			get { return this.m_SortDirection; }
			set { this.m_SortDirection = value; }
		}

		public Type Type { get; set; }

		public LambdaExpression GetLinqSortExpr<T>() where T : class, new()
		{
			ParameterExpression mainObject = Expression.Parameter(typeof(T), "obj");
			Expression propVal = mainObject;
			Type objType = typeof(T);

			string[] propTree = this.ColumnName.Split('.');

			foreach (string prop in propTree)
			{
				PropertyInfo pi = objType.GetProperty(prop, BindingFlags.Instance | BindingFlags.Public);
				if (pi != null)
				{
					propVal = Expression.Property(propVal, pi);
					objType = pi.PropertyType;
				}
			}

			if (propVal == mainObject)
			{
				propVal = Expression.Constant(1);
				objType = typeof(int);
			}

			Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), objType);
			LambdaExpression lambda = Expression.Lambda(delegateType, propVal, mainObject);

			return lambda;
		}

        public override int GetHashCode()
        {
            return this.Type.GetHashCode() ^ this.ColumnName.GetHashCode() ^ this.SortDirection.GetHashCode();
        }
	}
}
