using System;
using System.ComponentModel;

namespace RF.Common
{
	[AttributeUsage(AttributeTargets.Field)]
	public class MaskDescAttribute : Attribute
	{
		public string Mask { get; private set; }
		public string Watermark { get; private set; }
		public string RX { get; private set; }

		public MaskDescAttribute(string mask, string watermark, string rx)
		{
			Mask = mask;
			Watermark = watermark;
			RX = rx;
		}
	}
}

