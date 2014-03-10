using System;

namespace RF.Common.Security
{
    [AttributeUsage(AttributeTargets.Field)]
    public class PolicyNameAttribute : Attribute
    {
		private string m_Description;
		private PolicyType m_PolicyType;
		private bool m_AddToAdministratorsGroup = true;

		public string Description
		{
			get { return this.m_Description; }
			set { this.m_Description = value; }
		}

		public PolicyType PolicyType
		{
			get { return this.m_PolicyType; }
			set { this.m_PolicyType = value; }
		}

		public bool AddToAdministratorsGroup
		{
			get { return this.m_AddToAdministratorsGroup; }
			set { this.m_AddToAdministratorsGroup = value; }
		}
    }
}