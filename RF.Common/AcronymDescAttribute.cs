using System;
using System.ComponentModel;

namespace RF.Common
{
	[AttributeUsage(AttributeTargets.Field)]
	public class AcronymDescAttribute : DescriptionAttribute
	{
		public string Acronym { get; private set; }

		public AcronymDescAttribute(string acronym, string desc)
			: base(desc)
		{
			Acronym = acronym;
		}
	}
}
