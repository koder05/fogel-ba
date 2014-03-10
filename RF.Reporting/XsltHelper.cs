using System;
using System.Xml.XPath;
using System.Xml;

namespace RF.Reporting
{
	/// <summary>
	/// Класс-помощник для выполнения дополнительных функций в XSLT-шаблоне
	/// </summary>
	public class XsltHelper
	{
		public string FormatNumber(string num, string format)
		{
			if (string.IsNullOrEmpty(num))
				return "";

			double d = XmlConvert.ToDouble(num);
			return d.ToString(format);
		}
		
		public string FormatDateTime(string dateTime, string format)
		{
			if(string.IsNullOrEmpty(dateTime))
				return "";

			DateTime dt = XmlConvert.ToDateTime(dateTime, XmlDateTimeSerializationMode.Unspecified);
			return dt.ToString(format);
		}

		public bool ContainsCaseInsensitive(string str, string value)
		{
			if (str == null || value == null)
				return false;

			return str.ToLower().Contains(value.ToLower());
		}

		public int GetTimeSpanInDays(string startDateStr, string endDateStr)
		{
			DateTime startDate = XmlConvert.ToDateTime(startDateStr, XmlDateTimeSerializationMode.Unspecified);
			DateTime endDate = XmlConvert.ToDateTime(endDateStr, XmlDateTimeSerializationMode.Unspecified);
			TimeSpan span = endDate - startDate;
			return (int)span.TotalDays;
		}

		public DateTime AddDays(int days, DateTime date)
		{
			return date.AddDays(days);
		}

		public string GetMoneyText(decimal moneyValue)
		{
			return RF.Common.Money.GetLiteralSum(moneyValue);
		}

        public string GetMoneyText(decimal moneyValue, string lang)
        {
            switch (lang)
            {
                case "kk-KZ":
                    return RF.Common.Money.GetLiteralSumKK(moneyValue);
                default:
                    return RF.Common.Money.GetLiteralSum(moneyValue);
            }
        }

		public string MinDate(XPathNodeIterator iter)
		{
			DateTime? min = null;
			while (iter.MoveNext())
			{
				if (string.IsNullOrEmpty(iter.Current.Value))
					continue;

				DateTime dt = XmlConvert.ToDateTime(iter.Current.Value, XmlDateTimeSerializationMode.Unspecified);
				if (dt < min || min == null)
					min = dt;
			}

            if (min == null) return string.Empty;

            return RF.Common.Utils.XmlConvertToString(min.Value); ;
		}

        public string MaxDate(XPathNodeIterator iter)
        {
            DateTime? max = null;
            while (iter.MoveNext())
            {
                if (string.IsNullOrEmpty(iter.Current.Value))
                    continue;

                DateTime dt = XmlConvert.ToDateTime(iter.Current.Value, XmlDateTimeSerializationMode.Unspecified);
                if (dt > max || max == null)
                    max = dt;
            }

            if (max == null) return string.Empty;

            return RF.Common.Utils.XmlConvertToString(max.Value); ;
        }

		public int ExpandDepth(XPathNodeIterator iter)
		{
			int result = 0;
			int curdepth = 0;
			while (iter.MoveNext())
			{
				try
				{
					curdepth = Convert.ToInt32(iter.Current.GetAttribute("Level", ""));
				}
				catch
				{
					continue;
				}
				if (curdepth > result) result = curdepth;
			}
			return result;
		}

		public string Substr(string val, int pos, int len)
		{
			if (false == string.IsNullOrEmpty(val) && pos >= 0 && len >= 0 && val.Length >= pos + len)
				return val.Substring(pos, len);

			return string.Empty;
		}
	}
}