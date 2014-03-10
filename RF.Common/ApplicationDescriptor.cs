using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace RF.Common
{
	/// <summary>
	/// Класс, описывающий конкретную сборку приложения
	/// </summary>
	public class ApplicationDescriptor
	{
		#region Private Constants
		
		private const string VersionTagName = "BuildVersion";
		private const string MajorVersionTagName = "Major";
		private const string MinorVersionTagName = "Minor";
		private const string BuildVersionTagName = "Build";
		private const string RevisionVersionTagName = "Revision";
		private const string DatabaseInfoTagName = "DatabaseInfo";

		private const string FileReadingErrorMessageFormat = "Ошибка при чтении документа из файла: {0}.";

		#endregion

		private Version _buildVersion;
		private string _databaseInfo;

		private ApplicationDescriptor(Version buildVersion): this(buildVersion, string.Empty)
		{
		}

		private ApplicationDescriptor(Version buildVersion, string databaseInfo)
		{
			_buildVersion = buildVersion;
			_databaseInfo = databaseInfo;
			BuildDate = DateTime.MinValue;
		}

		/// <summary>
		/// Версия сборки приложения
		/// </summary>
		public Version BuildVersion
		{
			get { return _buildVersion; }
		}

		/// <summary>
		/// Метаинформация о базе данных. Произвольная строка, описывающая текущую БД.
		/// </summary>
		public string DatabaseInfo
		{
			get { return _databaseInfo; }
		}

		/// <summary>
		///.Дата сборки билда. Пока не читается из xml-описания, проставляется извне.
		/// </summary>
		public DateTime BuildDate
		{
			get;
			set;
		}

		/// <summary>
		/// Разбирает ApplicationDescriptor из xml файла
		/// </summary>
		/// <param name="xmlDescriptionPath">Полный путь к xml-файлу с описанием сборки</param>
		/// <returns>Экземпляр ApplicationDescriptor</returns>
		public static ApplicationDescriptor Parse(string xmlDescriptionPath)
		{
			XmlDocument document = new XmlDocument();

			try
			{
				document.Load(xmlDescriptionPath);
			}
			catch (IOException ex)
			{
				throw new RFAppException(string.Format(FileReadingErrorMessageFormat, xmlDescriptionPath), ex);
			}
			catch (UnauthorizedAccessException ex)
			{
				throw new RFAppException(string.Format(FileReadingErrorMessageFormat, xmlDescriptionPath), ex);
			}
			catch (System.Security.SecurityException ex)
			{
				throw new RFAppException(string.Format(FileReadingErrorMessageFormat, xmlDescriptionPath), ex);
			}
			catch (XmlException ex)
			{
				throw new RFAppException(string.Format(FileReadingErrorMessageFormat, xmlDescriptionPath), ex);
			}

			return Parse(document);
		}

		/// <summary>
		/// Разбирает ApplicationDescriptor из xml-файла
		/// </summary>
		/// <param name="inputStream">Поток ввода/вывода, открытый для чтения xml-файла</param>
		/// <returns>Экземпляр ApplicationDescriptor</returns>
		public static ApplicationDescriptor Parse(Stream inputStream)
		{
			XmlDocument document = new XmlDocument();

			try
			{
				document.Load(inputStream);
			}
			catch (XmlException ex)
			{
				throw new RFAppException(string.Format(FileReadingErrorMessageFormat, "stream"), ex);
			}

			return Parse(document);
		}

		private static ApplicationDescriptor Parse(XmlDocument document)
		{
			XmlNode versionNode = GetRequiredNode(document.DocumentElement, VersionTagName);

			int major = ReadInt32FromNode(versionNode, MajorVersionTagName);
			int minor = ReadInt32FromNode(versionNode, MinorVersionTagName);
			int build = ReadInt32FromNode(versionNode, BuildVersionTagName);
			int revision = ReadInt32FromNode(versionNode, RevisionVersionTagName);

			string databaseInfo = string.Empty;
			XmlNode databaseInfoNode = GetOptionalNode(document.DocumentElement, DatabaseInfoTagName);
			if (databaseInfoNode != null)
				databaseInfo = databaseInfoNode.InnerText;

			return new ApplicationDescriptor(new Version(major, minor, build, revision), databaseInfo);
		}

		private static XmlNode GetOptionalNode(XmlNode rootNode, string xpathExpression)
		{
			return rootNode.SelectSingleNode(xpathExpression);
		}

		private static XmlNode GetRequiredNode(XmlNode rootNode, string xpathExpression)
		{
			XmlNode resultNode = GetOptionalNode(rootNode, xpathExpression);

			if (resultNode == null)
				throw new RFAppException(string.Format("Не удалось найти узел по запросу: {0}.", xpathExpression));

			return resultNode;
		}

		private static int ReadInt32FromNode(XmlNode rootNode, string xpathExpression)
		{
			XmlNode node = GetRequiredNode(rootNode, xpathExpression);
			XmlNode valueNode = node.FirstChild;

			if (valueNode == null)
				throw new RFAppException(string.Format("Не удалось получить значение узла '{0}'.", node.Name));

			string stringValue = valueNode.Value;
			try
			{
				return Int32.Parse(valueNode.Value);
			}
			catch (FormatException ex)
			{
				throw new RFAppException(string.Format("Значение узла '{0}' не является целым числом: {1}.", node.Name, stringValue), ex);
			}
		}
	}
}
