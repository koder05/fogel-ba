using System;

namespace RF.Reporting
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
	public class ReportProviderAttribute : Attribute
	{
		private Type m_Type;

		public ReportProviderAttribute(Type type)
		{
			if (type == null)
				throw new ArgumentNullException("type");

			this.m_Type = type;
		}

		public Type ProviderType
		{
			get { return this.m_Type; }
		}
	}
}