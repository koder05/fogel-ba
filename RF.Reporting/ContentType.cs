using System;
using System.ComponentModel;
using System.Net.Mime;

namespace RF.Reporting
{
	public enum ContentType : byte
	{
		[Description("")]
		Unknown = 0,

		[Description(MediaTypeNames.Text.Html)]
		Html = 1,

		[Description(MediaTypeNames.Text.Plain)]
		PlainText = 2,

		[Description(MediaTypeNames.Application.Rtf)]
		RichTextFormat = 3,

		[Description("application/vnd.ms-excel")]
		MicrosoftExcel = 4,

		[Description("application/msword")]
		MicrosoftWord = 5,

		[Description(MediaTypeNames.Application.Zip)]
		Zip = 6,

		[Description(MediaTypeNames.Application.Octet)]
		OctetStream = 7,

		[Description("image/x-emz")]
		ImageEmz = 8,

		[Description(MediaTypeNames.Text.Xml)]
		XmlText = 9
	}
}
