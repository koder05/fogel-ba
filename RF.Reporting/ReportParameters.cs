using System;
using System.IO.Compression;
using System.Data;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Drawing;

using RF.Common;
//using RF.Interfaces;
//using RF.ObjectModel;

namespace RF.Reporting
{
	/// <summary>
	/// Параметры отчёта
	/// </summary>
	public class ReportParameters
	{
		private class BinaryData
		{
			private byte[] m_Data;
			private IDictionary<string, object> m_Metadata;

			public BinaryData(byte[] data)
				: this(data, null)
			{
			}

			public BinaryData(byte[] data, IDictionary<string, object> metadata)
			{
				this.m_Data = data;
				this.m_Metadata = metadata;
			}

			public byte[] Data
			{
				get { return this.m_Data; }
			}
			public IDictionary<string, object> Metadata
			{
				get { return this.m_Metadata; }
			}
		}

		private Dictionary<string, object> m_Properties = new Dictionary<string, object>();
		private List<DataView> m_DataViews = new List<DataView>();
		private List<XmlElement> m_XmlElements = new List<XmlElement>();
		private Dictionary<string, BinaryData> m_ByteArrays = new Dictionary<string, BinaryData>();
		private string m_ReportName;
		private string m_ContentType;
		private string[] m_TargetFilePathParts;
		private string m_LanguageCode;
		private bool m_PlaceUserInfo;

		/// <summary>
		/// Создаёт новый экземпляр параметров отчёта
		/// </summary>
		/// <param name="reportName">Имя отчёта</param>
		/// <param name="contentType">MIME-тип отчёта</param>
		/// <param name="targetFilePathParts">Элементы файлового пути</param>
		public ReportParameters(string reportName, string contentType, string[] targetFilePathParts, bool placeUserInfo)
		{
			if (reportName == null)
				throw new ArgumentNullException("reportName");

			if (contentType == null)
				throw new ArgumentNullException("contentType");

			if (reportName.Length == 0)
				throw new ArgumentException("ReportName is empty string.");

			if (contentType.Length == 0)
				throw new ArgumentException("ContentType is empty string.");

			this.m_ReportName = reportName;
			this.m_ContentType = contentType;
			if (targetFilePathParts != null && targetFilePathParts.Length > 0 && targetFilePathParts[0] != null)
				this.m_TargetFilePathParts = targetFilePathParts;

			this.m_PlaceUserInfo = placeUserInfo;
		}

		public ReportParameters(string reportName, string contentType, string targetFileName) :
			this(reportName, contentType, targetFileName, false)
		{

		}

		/// <summary>
		/// Создаёт новый экземпляр параметров отчёта
		/// </summary>
		/// <param name="reportName">Имя отчёта</param>
		/// <param name="contentType">MIME-тип отчёта</param>
		/// <param name="targetFileName">Желаемое имя файла сгенерированного отчёта</param>
		public ReportParameters(string reportName, string contentType, string targetFileName, bool placeUserInfo) :
			this(reportName, contentType, new string[] { targetFileName }, placeUserInfo)
		{
			
		}

		/// <summary>
		/// Создаёт новый экземпляр параметров отчёта
		/// </summary>
		/// <param name="reportName">Имя отчёта</param>
		/// <param name="contentType">MIME-тип отчёта</param>
		public ReportParameters(string reportName, string contentType)
			: this(reportName, contentType, (string)null)
		{
		}

		/// <summary>
		/// Добавляет объект <see cref="System.Data.DataView"/> к параметрам отчёта. Имя таблицы берётся из <see cref="System.Data.DataTable"/>
		/// соответствуюшего заданному <see cref="System.Data.DataView"/>
		/// </summary>
		/// <param name="dv">Объект <see cref="System.Data.DataView"/></param>
		public void AddDataView(DataView dv)
		{
			if (dv.Table == null || string.IsNullOrEmpty(dv.Table.TableName))
				throw new ArgumentException("Specified System.Data.DataView has not corresponding System.Data.DataTable.");

			AddDataView(dv, dv.Table.TableName);
		}

		/// <summary>
		/// Добавляет объект <see cref="System.Data.DataView"/> к параметрам отчёта.
		/// </summary>
		/// <param name="dv">Объект <see cref="System.Data.DataView"/></param>
		/// <param name="tableName">Имя таблицы</param>
		public void AddDataView(DataView dv, string tableName)
		{
			if (dv == null || dv.Count == 0)
				return;

			if (string.IsNullOrEmpty(tableName) == false)
				dv.Table.TableName = tableName;

			this.m_DataViews.Add(dv);
		}

		/// <summary>
		/// Добавляет объект <see cref="System.Data.DataView"/> к параметрам отчёта.
		/// </summary>
		/// <param name="dv">Объект <see cref="System.Data.DataView"/></param>
		/// <param name="correspondingType">Объект <see cref="System.Type"/>, соответствующий строкам в <see cref="System.Data.DataView"/></param>
		public void AddDataView(DataView dv, Type correspondingType)
		{
			if (correspondingType == null)
				throw new ArgumentNullException("correspondingType");

			this.AddDataView(dv, correspondingType.Name);
		}

		/// <summary>
		/// Добавляет объект <see cref="System.Xml.XmlElement"/> к параметрам отчёта
		/// </summary>
		/// <param name="element">Объект <see cref="System.Xml.XmlElement"/></param>
		public void AddXmlElement(XmlElement element)
		{
			if (element == null)
				throw new ArgumentNullException("element");

			XmlDocument doc = new XmlDocument();
			doc.AppendChild(doc.ImportNode(element, true));
			this.m_XmlElements.Add(doc.DocumentElement);
		}

		/// <summary>
		/// Имя отчёта
		/// </summary>
		public string ReportName
		{
			get { return this.m_ReportName; }
		}

		/// <summary>
		/// MIME-тип отчёта
		/// </summary>
		public string ContentType
		{
			get { return this.m_ContentType; }
		}

		/// <summary>
		/// Желаемое имя файла сгенерированного отчёта
		/// </summary>
		public string[] TargetFilePathParts
		{
			get { return this.m_TargetFilePathParts; }
			set { this.m_TargetFilePathParts = value; }
		}

		public string TargetFileName
		{
			get
			{
				if (this.m_TargetFilePathParts != null && this.m_TargetFilePathParts.Length > 0)
					return this.m_TargetFilePathParts[this.m_TargetFilePathParts.Length - 1];

				return null;
			}
		}

		/// <summary>
		/// 4-буквенный ISO код языка отчета (ru-RU, kk-KZ)
		/// </summary>
		public string LanguageCode
		{
			get { return this.m_LanguageCode; }
			set { this.m_LanguageCode = value; }
		}

		/// <summary>
		/// Устанавливает или получает дополнительные свойства
		/// </summary>
		/// <param name="propertyName">Имя свойства</param>
		/// <returns>Значение свойства</returns>
		public object this[string propertyName]
		{
			get
			{
				if (propertyName == null)
					throw new ArgumentNullException("propertyName");

				return this.m_Properties[propertyName];
			}
			set
			{
				if (propertyName == null)
					throw new ArgumentNullException("propertyName");

				this.m_Properties[propertyName] = value;
			}
		}

		public void AddBinary(string name, byte[] bytes)
		{
			if (name == null)
				throw new ArgumentNullException("name");

			this.m_ByteArrays.Add(name, new BinaryData(bytes));
		}

		public void AddImage(string name, byte[] bytes)
		{
			if (name == null)
				throw new ArgumentNullException("name");

			IDictionary<string, object> metadata = null;
			if (bytes != null)
			{
				Size imageSize = GetImageSize(bytes);
				metadata = new Dictionary<string, object>();
				metadata.Add("Width", imageSize.Width);
				metadata.Add("Height", imageSize.Height);
			}

			this.m_ByteArrays.Add(name, new BinaryData(bytes, metadata));
		}

		private Size GetImageSize(byte[] imageBytes)
		{
			if (imageBytes == null)
				throw new ArgumentNullException("imageBytes");

			Image img = null;
			MemoryStream ms = new MemoryStream(imageBytes);
			try
			{
				img = Image.FromStream(ms);
			}
			catch (ArgumentException)
			{
				ms.Position = 0;
				img = Image.FromStream(new GZipStream(ms, CompressionMode.Decompress));
			}

			Size size = img.Size;
			img.Dispose();

			return size;
		}

		internal XmlDocument GenerateInputDocument()
		{
			XmlDocument doc = new XmlDocument();
			doc.AppendChild(doc.CreateElement("Report"));
			foreach (KeyValuePair<string, object> keyValue in m_Properties)
				doc.DocumentElement.SetAttribute(keyValue.Key, Utils.XmlConvertToString(keyValue.Value));

			doc.DocumentElement.SetAttribute("CurrentDate", Utils.XmlConvertToString(DateTime.Now));

            //if (this.m_PlaceUserInfo && System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated)
            //{
            //    User usr = IoC.Resolve<IUsersRepository>().GetCurrentUser();
            //    doc.DocumentElement.SetAttribute("CurrentUser", usr.FullName);
            //    foreach (Contact cnt in IoC.Resolve<IContactRepository>().GetContactList(usr.Pers.ID, 0, int.MaxValue, "IsDefault"))
            //    {
            //        if (cnt.Presentation is Phone && cnt.UsageType == (int)PhoneType.WorkPhone)
            //        {
            //            doc.DocumentElement.SetAttribute("CurrentUserPhone", cnt.Presentation.ContactStringPresentation);
            //            break;
            //        }
            //    }
            //}

			foreach (DataView dv in this.m_DataViews)
			{
				DataTable dt = dv.ToTable();

				foreach (DataRow dr in dt.Rows)
				{
					XmlElement rowEl = doc.CreateElement(dt.TableName);
					doc.DocumentElement.AppendChild(rowEl);

					foreach (DataColumn column in dt.Columns)
						rowEl.SetAttribute(column.ColumnName, Utils.XmlConvertToString(dr[column]));
				}
			}

			foreach (XmlElement el in this.m_XmlElements)
				doc.DocumentElement.AppendChild(doc.ImportNode(el, true));

			if (this.m_ByteArrays.Count > 0)
			{
				XmlElement byteArraysContainer = doc.CreateElement("BinaryData");
				doc.DocumentElement.AppendChild(byteArraysContainer);

				foreach (KeyValuePair<string, BinaryData> entry in this.m_ByteArrays)
				{
					XmlElement binaryContainer = doc.CreateElement(entry.Key);
					byteArraysContainer.AppendChild(binaryContainer);
					string base64 = "";
					if (entry.Value.Data != null)
						base64 = Convert.ToBase64String(entry.Value.Data, Base64FormattingOptions.InsertLineBreaks);

					if (entry.Value.Metadata != null)
					{
						foreach (KeyValuePair<string, object> metaKeyValue in entry.Value.Metadata)
							binaryContainer.SetAttribute(metaKeyValue.Key, Utils.XmlConvertToString(metaKeyValue.Value));
					}

					binaryContainer.InnerText = base64;
				}
			}

			return doc;
		}
	}
}