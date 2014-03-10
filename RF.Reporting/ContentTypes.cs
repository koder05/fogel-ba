using System;

using RF.Common;
//using RF.Interfaces;

namespace RF.Reporting
{
	/// <summary>
	/// Стандартные MIME-типы
	/// </summary>
	public static class ContentTypes
	{
		public static string Html = Utils.GetEnumDescription<ContentType>(ContentType.Html);
		public static string PlainText = Utils.GetEnumDescription<ContentType>(ContentType.PlainText);
		public static string RichTextFormat = Utils.GetEnumDescription<ContentType>(ContentType.RichTextFormat);
		public static string MicrosoftExcel = Utils.GetEnumDescription<ContentType>(ContentType.MicrosoftExcel);
		public static string MicrosoftWord = Utils.GetEnumDescription<ContentType>(ContentType.MicrosoftWord);
		public static string Zip = Utils.GetEnumDescription<ContentType>(ContentType.Zip);
		public static string OctetStream = Utils.GetEnumDescription<ContentType>(ContentType.OctetStream);
		public static string ImageEmz = Utils.GetEnumDescription<ContentType>(ContentType.ImageEmz);
		public static string XmlText = Utils.GetEnumDescription<ContentType>(ContentType.XmlText);
	}
}