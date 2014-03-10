using System;
using System.Xml;

namespace RF.Reporting
{
	/// <summary>
	/// Интерфейс провайдера шаблонов для отчётов.
	/// </summary>
	public interface IReportProvider
	{
		/// <summary>
		/// Возвращает XSLT-шаблон отчёта как объект <see cref="System.Xml.XmlDocument"/> по имени отчёта или MIME-типу.
		/// </summary>
		/// <param name="reportName">Имя отчёта.</param>
		/// <param name="contentType">MIME-тип отчёта.</param>
		/// <param name="languageCode">Код языка (2 символа)</param>
		/// <returns>XSLT-шаблон отчёта, как объект <see cref="System.Xml.XmlDocument"/>.</returns>
		XmlDocument GetReportTemplate(string reportName, string contentType, string languageCode);

		XmlResolver GetResolver(string reportName, string contentType, string languageCode);
	}
}