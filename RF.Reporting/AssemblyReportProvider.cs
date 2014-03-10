using System;
using System.Net.Mime;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Reflection;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace RF.Reporting
{
	public class AssemblyReportProvider : IReportProvider, ISupportInitialize
	{
		private class AsesemblyResourceNamespace
		{
			public AsesemblyResourceNamespace(Assembly assembly, string resourceNamespace)
			{
				this.Assembly = assembly;
				this.ResourceNamespace = resourceNamespace;
			}

			public Assembly Assembly;
			public string ResourceNamespace;
		}

		private class AssemblyXmlResolver : XmlResolver
		{
			private const char Slash = '/';
			private const char Dot = '.';

			private AsesemblyResourceNamespace m_Ns;

			public AssemblyXmlResolver(AsesemblyResourceNamespace ns)
			{
				this.m_Ns = ns;
			}

			public override Uri ResolveUri(Uri baseUri, string relativeUri)
			{
				Uri newBaseUri = new Uri("resources://asm/" + m_Ns.ResourceNamespace.Replace(Dot, Slash) + Slash);
				Uri result = new Uri(newBaseUri, relativeUri);
				return result;
			}

			public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
			{
				Assembly asm = this.m_Ns.Assembly;
				string path = absoluteUri.GetComponents(UriComponents.Path, UriFormat.Unescaped).Replace(Slash, Dot);
				return asm.GetManifestResourceStream(path);
			}

			public override System.Net.ICredentials Credentials
			{
				set { throw new NotImplementedException(); }
			}
		}

		private static readonly Regex s_ResourceNameRegex = new Regex(@"(?<ReportName>[^\.]+)\.(?<Alias>html|txt|rtf|excel|word|xml)\.((?<Language>[a-z]{2}_[A-Z]{2})\.)?xslt$");

		private Dictionary<ReportFullName, XmlDocument> m_Reports = null;
		private Dictionary<ReportFullName, AsesemblyResourceNamespace> m_ReportNamespaces = null;

		public AssemblyReportProvider()
		{
		}

		void ISupportInitialize.BeginInit()
		{
		}

		void ISupportInitialize.EndInit()
		{
			this.Initialize();
		}

		private void Initialize()
		{
			List<Assembly> reportAssemblies = new List<Assembly>();
			Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly asm in loadedAssemblies)
			{
				if(asm.IsDefined(typeof(ReportAssemblyAttribute), false))
					reportAssemblies.Add(asm);
			}
		
			Dictionary<string, string> mappings = new Dictionary<string,string>();
			Dictionary<ReportFullName, XmlDocument> reports = new Dictionary<ReportFullName, XmlDocument>();
			Dictionary<ReportFullName, AsesemblyResourceNamespace> namespaces = new Dictionary<ReportFullName, AsesemblyResourceNamespace>();

			mappings.Add("html", ContentTypes.Html);
			mappings.Add("txt", ContentTypes.PlainText);
			mappings.Add("rtf", MediaTypeNames.Application.Rtf);
			mappings.Add("excel", ContentTypes.MicrosoftExcel);
			mappings.Add("word", ContentTypes.MicrosoftWord);
			mappings.Add("xml", ContentTypes.XmlText);

			foreach (Assembly asm in reportAssemblies)
			{
				foreach (string resourceName in asm.GetManifestResourceNames())
				{
					Match m = s_ResourceNameRegex.Match(resourceName);

					if(m.Success == false)
						continue;

					string alias = m.Groups["Alias"].Value.ToLowerInvariant();

					if(mappings.ContainsKey(alias) == false)
						continue;

					string contentType = mappings[alias];
					string reportName = m.Groups["ReportName"].Value;
					string languageCode = null;
					Group langGroup = m.Groups["Language"];
					if (langGroup.Success)
						languageCode = langGroup.Value.Replace("_", "-");

					XmlDocument reportDoc = new XmlDocument();
					try
					{
						reportDoc.Load(asm.GetManifestResourceStream(resourceName));
					}
					catch (Exception exc)
					{
						throw new Exception(String.Format("Не удалось загрузить шаблон отчёта {0} ({1})", reportName, exc.Message, exc));
					}
					reports.Add(new ReportFullName(reportName, contentType, languageCode), reportDoc);
					string resourceNamespace = resourceName.Substring(0, resourceName.IndexOf(reportName) - 1);
					namespaces.Add(new ReportFullName(reportName, contentType, languageCode), new AsesemblyResourceNamespace(asm, resourceNamespace));
				}
			}

			this.m_Reports = reports;
			this.m_ReportNamespaces = namespaces;
		}

		private static void VerifyReportNameAndContentType(string reportName, string contentType)
		{
			if (reportName == null)
				throw new ArgumentNullException("reportName");

			if (contentType == null)
				throw new ArgumentNullException("contentType");
		}

		private T GetReportItem<T>(Dictionary<ReportFullName, T> dict, string reportName, string contentType, string languageCode)
		{
			ReportFullName rfn = new ReportFullName(reportName, contentType, languageCode);
			T res;
			if (dict.TryGetValue(rfn, out res))
				return res;

			rfn = new ReportFullName(reportName, contentType);
			return dict[rfn];
		}

		public XmlDocument GetReportTemplate(string reportName, string contentType, string languageCode)
		{
			VerifyReportNameAndContentType(reportName, contentType);

			XmlDocument result = GetReportItem<XmlDocument>(this.m_Reports, reportName, contentType, languageCode);;
			return result;
		}

		public XmlResolver GetResolver(string reportName, string contentType, string languageCode)
		{
			VerifyReportNameAndContentType(reportName, contentType);
			AsesemblyResourceNamespace arns = GetReportItem<AsesemblyResourceNamespace>(this.m_ReportNamespaces, reportName, contentType, languageCode);
			return new AssemblyXmlResolver(arns);
		}
	}
}