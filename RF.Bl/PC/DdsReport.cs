using System;
using System.Xml;
using System.Xml.Serialization;

using RF.Common.Security;

namespace RF.BL.PC
{
	[XmlRootAttribute("row")]
	public class DdsReport
	{
		public static class PolicyNames
		{
			[PolicyName(Description = "Отчет - ДДС Access", PolicyType = PolicyType.Reports)]
			public const string View = "DdsAccesReportView";
		}

		[XmlAttribute("SS_PFR")]
		public string InsCert { get; set; }

		[XmlAttribute("KodOper")]
		public string KodOper { get; set; }

		[XmlAttribute("AktivPS")]
		public int AktivPS { get; set; }

		[XmlAttribute("DataOper")]
		public DateTime DataOper { get; set; }

		[XmlAttribute("DataProv")]
		public DateTime DataProv { get; set; }

		[XmlAttribute("Summa")]
		public decimal Summa { get; set; }

        [XmlAttribute("ShortName")]
		public string NameOper { get; set; }

		//public string FullName
		//{
		//    get
		//    {
		//        StringBuilder sb = new StringBuilder(this.LeftMargin * 6);
		//        for (int i = 0; i < this.LeftMargin; i++)
		//            sb.Append("&#160;");

		//        return string.Format("{0}{1}", sb.ToString(), this.Name);
		//    }
		//}

		public string InOutString
		{
			get
			{
                if (this.AktivPS == 1)
                {
                    return "Поступление";
                }
                else if (this.AktivPS == -1)
                {
                    return "Списание";
                }

				return string.Empty;
			}
		}
	}
}



