using System;
using System.Diagnostics;

namespace RF.LinqExt
{
	[DebuggerDisplay("ColumnName = {ColumnName}, Operator = {Operator}")]
	public class FilterParameter
	{
		public static readonly OperatorType DefaultOperator = OperatorType.Equals;

		private string m_ColumnName;
		private OperatorType m_Operator = DefaultOperator;
		private object m_Value;
		private string m_OrGroupName = string.Empty;
		private string m_AndGroupName = string.Empty;

		public FilterParameter()
		{
		}

		public FilterParameter(string columnName, object value, OperatorType op, string orGroup, string andGroup)
		{
			if (columnName == null)
				throw new ArgumentNullException("columnName");

			this.m_ColumnName = columnName;
			this.m_Value = value;
			this.m_Operator = op;
			this.m_OrGroupName = orGroup;
			this.m_AndGroupName = andGroup;
		}

		public FilterParameter(string columnName, object value, OperatorType op, string orGroup)
		{
			if (columnName == null)
				throw new ArgumentNullException("columnName");

			this.m_ColumnName = columnName;
			this.m_Value = value;
			this.m_Operator = op;
			this.m_OrGroupName = orGroup;
		}

		public FilterParameter(string columnName, object value, OperatorType op)
		{
			if (columnName == null)
				throw new ArgumentNullException("columnName");

			this.m_ColumnName = columnName;
			this.m_Value = value;
			this.m_Operator = op;
		}

		public FilterParameter(string columnName, object value)
		{
			if (columnName == null)
				throw new ArgumentNullException("columnName");

			this.m_ColumnName = columnName;
			this.m_Value = value;
		}

		public string ColumnName
		{
			get { return this.m_ColumnName; }
			set { this.m_ColumnName = value; }
		}

		public OperatorType Operator
		{
			get { return this.m_Operator; }
			set { this.m_Operator = value; }
		}

		public object Value
		{
			get { return this.m_Value; }
			set { this.m_Value = value; }
		}

		public string OrGroupName
		{
			get { return this.m_OrGroupName; }
			set { this.m_OrGroupName = value; }
		}

		public string AndGroupName
		{
			get { return this.m_AndGroupName; }
			set { this.m_AndGroupName = value; }
		}
	}
}