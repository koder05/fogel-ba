using System;
using System.Globalization;
using System.Text;
using System.Net.Mime;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Reflection;
using System.Web;
using System.ComponentModel;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Linq;

using RF.ZipCompression;

namespace RF.Reporting
{
	/// <summary>
	/// Генератор отчётов.
	/// </summary>
	[ReportProvider(typeof(AssemblyReportProvider))]
	public class ReportGenerator
	{
		private static List<IReportProvider> s_Providers = null;
		private static Dictionary<ReportFullName, XslCompiledTransform> s_TransformCache = new Dictionary<ReportFullName, XslCompiledTransform>();

		private static void InitializeProviders()
		{
			foreach (ReportProviderAttribute attr in typeof(ReportGenerator).GetCustomAttributes(typeof(ReportProviderAttribute), false))
			{
				IReportProvider provider = (IReportProvider)Activator.CreateInstance(attr.ProviderType);
				if (provider is ISupportInitialize)
				{
					ISupportInitialize supportInit = provider as ISupportInitialize;
					supportInit.BeginInit();
					supportInit.EndInit();
				}
				s_Providers.Add(provider);
			}
		}


		public static string CreateZipArchiveName(string title)
		{
			return string.Format("{0}__{1:yyyy-MM-dd}.zip", title, DateTime.Now);
		}

		/// <summary>
		/// Создаёт отчёт.
		/// </summary>
		/// <param name="parameters">Параметры отчёта.</param>
		/// <param name="tw">Объект <see cref="System.IO.TextWriter"/>, куда будет записаны результаты.</param>
		public static void GenerateReport(ReportParameters parameters, TextWriter tw)
		{
			if (parameters == null)
				throw new ArgumentNullException("parameters");

			if (tw == null)
				throw new ArgumentNullException("tw");

			if (s_Providers == null)
			{
				lock (typeof(ReportGenerator))
				{
					if (s_Providers == null)
					{
						s_Providers = new List<IReportProvider>();
						InitializeProviders();
					}
				}
			}

			XslCompiledTransform transform = GetCachedTransform(parameters.ReportName, parameters.ContentType, parameters.LanguageCode);
			if (transform == null)
			{
				XmlDocument transformXmlDoc = null;
				XmlResolver resolver = null;
				lock (s_Providers)
				{
					foreach (IReportProvider provider in s_Providers)
					{
						transformXmlDoc = provider.GetReportTemplate(parameters.ReportName, parameters.ContentType, parameters.LanguageCode);
						if (transformXmlDoc != null)
						{
							resolver = provider.GetResolver(parameters.ReportName, parameters.ContentType, parameters.LanguageCode);
							break;
						}
					}
				}

				if (transformXmlDoc == null)
					throw new ReportNotFoundException(string.Format("Report \"{0}\" with content type \"{1}\" was not found.", parameters.ReportName, parameters.ContentType));

				transform = new XslCompiledTransform(true);
				XsltSettings xsltSettings = new XsltSettings();
				xsltSettings.EnableDocumentFunction = true;
				transform.Load(new XmlNodeReader(transformXmlDoc), xsltSettings, resolver);
				StoreInCache(new ReportFullName(parameters.ReportName, parameters.ContentType, parameters.LanguageCode), transform);
			}

			CultureInfo sysCulture = null;
			CultureInfo reportCulture = null;
			if (parameters.LanguageCode != null)
				reportCulture = CultureInfo.GetCultureInfo(parameters.LanguageCode);

			try
			{
				if (reportCulture != null)
				{
					sysCulture = Thread.CurrentThread.CurrentCulture;
					Thread.CurrentThread.CurrentCulture = reportCulture;
				}

				XmlDocument inputDoc = parameters.GenerateInputDocument();
				XsltArgumentList xslArgs = new XsltArgumentList();
				xslArgs.AddExtensionObject("http://www.regionfund.ru/fol/ReportXsltHelper/", new XsltHelper());
				XPathDocument xpathDoc = new XPathDocument(new XmlNodeReader(inputDoc));
				transform.Transform(xpathDoc, xslArgs, tw);
			}
			finally
			{
				if (sysCulture != null)
					Thread.CurrentThread.CurrentCulture = sysCulture;
			}
		}

        public static string GenerateReportToRawString(ReportParameters parameters)
        {
            using (var txtstream = new StringWriter())
            {
                try
                {
                    GenerateReport(parameters, txtstream);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(string.Format("Ошибка при генерации файла отчета. " + Environment.NewLine + "Ошибка: {0}", ex.Message), ex);
                }

                return txtstream.ToString();
            }
        }


		/// <summary>
		/// Создаёт отчёт с указанными параметрами и направляет  его в текущий <see cref="System.Web.HttpResponse"/>.
		/// </summary>
		/// <param name="parameters">Параметры отчёта.</param>
		public static void GenerateReportToHttpResponse(ReportParameters parameters)
		{
			GenerateReportToHttpResponse(parameters, null, Encoding.UTF8);
		}

		/// <summary>
		/// Создаёт отчёт с указанными параметрами и направляет его в текущий <see cref="System.Web.HttpResponse"/>.
		/// </summary>
		/// <param name="parameters">Параметры отчёта.</param>
		/// <param name="overrideParametersContentType">MIME-тип для использования вместо встроенного в ReportParameters</param>
		/// <param name="encoding">Кодировка</param>
		public static void GenerateReportToHttpResponse(ReportParameters parameters, string overrideParametersContentType, Encoding encoding)
		{
			if (parameters == null)
				throw new ArgumentNullException("parameters");

			HttpContext context = HttpContext.Current;
			if (context == null)
				throw new InvalidOperationException("HttpContext is not present.");

			HttpResponse resp = context.Response;
			resp.ContentEncoding = encoding;
			resp.Clear();
			try
			{
				GenerateReport(parameters, resp.Output);
				resp.ContentType = (overrideParametersContentType != null) ? overrideParametersContentType : parameters.ContentType;
				if (string.IsNullOrEmpty(parameters.TargetFileName) == false)
					AddContentDispositionHeader(parameters.TargetFileName);
			}
			catch (Exception)
			{
				resp.Clear();
				throw;
			}
			resp.End();
		}

		private class StringWriterEx : StringWriter
		{
			private Encoding _encoding;

			public StringWriterEx(Encoding encoding)
				: base(new CultureInfo("ru-RU"))
			{
				_encoding = encoding;
			}

			public override Encoding Encoding
			{
				get
				{
					if (_encoding == null)
					{
						_encoding = base.Encoding;
					}
					return _encoding;
				}
			}
		}

		public static void GenerateReportToHttpResponseAsZipArchive(IEnumerable<ReportParameters> parametersCollection)
		{
			GenerateReportToHttpResponseAsZipArchive(parametersCollection, null);
		}

		public static void GenerateReportToHttpResponseAsZipArchive(IEnumerable<ReportParameters> parametersCollection, string zipFileName)
		{
			GenerateReportToHttpResponseAsZipArchive(parametersCollection, zipFileName, Encoding.UTF8);
		}

		public static void GenerateReportToHttpResponseAsZipArchive(IEnumerable<ReportParameters> parametersCollection, string zipFileName, Encoding encoding)
		{
			if (parametersCollection == null)
				throw new ArgumentNullException("parametersCollection");

			HttpContext context = HttpContext.Current;
			if (context == null)
				throw new InvalidOperationException("HttpContext is not present.");

			HttpResponse resp = context.Response;
			resp.Clear();
			//resp.ContentEncoding = encoding;
			try
			{
				ZipFile zf = new ZipFile();
				foreach (ReportParameters parameters in parametersCollection)
				{
					StringWriterEx sw = new StringWriterEx(encoding);
					GenerateReport(parameters, sw);
					zf.AddFileWithContent(parameters.TargetFilePathParts, sw.ToString(), encoding);
				}

				parametersCollection = null;

				byte[] zipBytes = zf.SaveToBytes();
				resp.BinaryWrite(zipBytes);
				if (context.Request.Browser.Browser == "IE")
					resp.ContentType = ContentTypes.OctetStream;

				else
					resp.ContentType = ContentTypes.Zip;

				if (string.IsNullOrEmpty(zipFileName) == false)
					AddContentDispositionHeader(zipFileName);
			}
			catch (Exception)
			{
				resp.Clear();
				throw;
			}
			resp.End();
		}

		public static void AddContentDispositionHeader(string fileName)
		{
			AddContentDispositionHeader(fileName, true);
		}

		public static void AddContentDispositionHeader(string fileName, bool asAttachment)
		{
			string header = null;
			HttpRequest req = HttpContext.Current.Request;
			HttpResponse resp = HttpContext.Current.Response;

			if (req.Browser.Browser == "IE")
			{
				ContentDisposition cd = new ContentDisposition();
				if (asAttachment == false)
					cd.Inline = true;

				cd.FileName = HttpUtility.UrlPathEncode(fileName);
				header = cd.ToString();
			}
			else
			{
				string format = "filename=\"{0}\"";
				if (asAttachment)
					format = "attachment; " + format;

				header = string.Format(format, fileName);
			}

			resp.AddHeader("Content-Disposition", header);
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		private static XslCompiledTransform GetCachedTransform(string reportName, string contentType)
		{
			ReportFullName rfn = new ReportFullName(reportName, contentType);
			if (s_TransformCache.ContainsKey(rfn))
				return s_TransformCache[rfn];

			return null;
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		private static XslCompiledTransform GetCachedTransform(string reportName, string contentType, string languageCode)
		{
			ReportFullName rfn = new ReportFullName(reportName, contentType, languageCode);
			if (s_TransformCache.ContainsKey(rfn))
				return s_TransformCache[rfn];

			return null;
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		private static void StoreInCache(ReportFullName rfn, XslCompiledTransform xslt)
		{
			s_TransformCache.Add(rfn, xslt);
		}

		public static void SendTextToResponse(string text, string contentType)
		{
			Encoding encoding = Encoding.UTF8;
			if (contentType == ContentTypes.PlainText)
				encoding = Encoding.GetEncoding(1251);

			SendTextToResponse(text, contentType, encoding, null);
		}

		private const int SendTextTimeout = 5 * 60; // Seconds

		public static void SendTextToResponse(string text, string contentType, Encoding encoding, string fileName)
		{
			HttpContext ctx = HttpContext.Current;
			ctx.Server.ScriptTimeout = SendTextTimeout;

			HttpResponse resp = ctx.Response;
			resp.Clear();
			resp.ContentType = contentType;
			resp.BufferOutput = false;
			resp.ContentEncoding = encoding;
			resp.Cache.SetCacheability(HttpCacheability.NoCache);

			if (false == string.IsNullOrEmpty(fileName))
				AddContentDispositionHeader(fileName, true);

			var bytes = encoding.GetBytes(text);
			resp.OutputStream.Write(bytes, 0, bytes.Length);
			resp.Flush();
			resp.End();
		}

		public static void SendXmlToResponse(XmlDocument doc)
		{
			SendXmlToResponse(doc, Encoding.UTF8);
		}

		public static void SendXmlToResponse(XmlDocument doc, Encoding encoding)
		{
			if (doc == null)
				throw new ArgumentNullException("doc");

			if (encoding == null)
				throw new ArgumentNullException("encoding");

			HttpContext ctx = HttpContext.Current;
			ctx.Server.ScriptTimeout = SendTextTimeout;

			HttpResponse resp = ctx.Response;
			resp.ContentType = ContentTypes.XmlText;
			resp.ContentEncoding = encoding;
			resp.Clear();
			resp.BufferOutput = false;
			resp.Cache.SetCacheability(HttpCacheability.NoCache);

			string fileName = doc.DocumentElement.LocalName + ".xml";
			AddContentDispositionHeader(fileName, true);

			TextWriter tw = new StreamWriter(resp.OutputStream, encoding);
			XmlWriterSettings xws = new XmlWriterSettings();
			xws.Encoding = encoding;
			xws.OmitXmlDeclaration = false;
			using (XmlWriter xw = XmlWriter.Create(tw, xws))
				doc.WriteContentTo(xw);

			resp.End();
		}

		public static void SendXlsToResponse(Stream xls, string fileName)
		{
			int xlsLen = Convert.ToInt32(xls.Length);
			byte[] xlsBytes = new byte[xlsLen];
			xls.Read(xlsBytes, 0, xlsLen);

			SendToResponse(xlsBytes, string.Format("{0}.xls", fileName), ContentTypes.MicrosoftExcel);
		}

		public static void SendToResponse(byte[] content, string fileName, string contentType)
		{
			if (content != null && content.Length > 0)
			{
				HttpContext context = HttpContext.Current;
				if (context == null)
					throw new InvalidOperationException("HttpContext is not present.");

				HttpResponse resp = context.Response;
				resp.Clear();
				try
				{
					resp.BinaryWrite(content);
					resp.ContentType = contentType;
					if (string.IsNullOrEmpty(fileName))
					{
						fileName = "BinaryFile";
					}
					AddContentDispositionHeader(fileName);
				}
				catch (Exception)
				{
					resp.Clear();
					throw;
				}
				resp.End();
			}
		}
	}
}