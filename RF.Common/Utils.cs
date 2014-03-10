using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Web.UI;

namespace RF.Common
{
	/// <summary>
	/// Различные утилиты
	/// </summary>
	public static class Utils
	{
        public static bool IsCollectionEmpty<T>(IEnumerable<T> collection)
        {
            return collection == null || collection.Count() == 0;
        }
        
        public static string CreateStringTruncation(this string value, int maxSize)
		{
			if (string.IsNullOrEmpty(value) || value.Length < maxSize || maxSize <= 3)
				return value;

			return value.Substring(0, maxSize - 3) + "...";
		}

		public static string ConvertToRomeNumber(int value)
		{
			if (value > 99 && value < 1)
				throw new InvalidCastException("Не умею переводить в римские цифры числа больше 99 и меньше 1");

			string[] units = new string[] { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" };
			string[] decs = new string[] { "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" };

			int dec = value / 10;
			int unit = value - dec;
			StringBuilder sb = new StringBuilder(10);

			if (dec > 0)
				sb.Append(decs[dec - 1]);

			if (unit > 0)
				sb.Append(units[unit - 1]);

			return sb.ToString();
		}

		public static string XmlConvertToStringTyped<T>(T value)
		{
			return XmlConvertToString(value);
		}

		/// <summary>
		/// Конвертирует объект в строковое представление, специфицируемое стандартом W3C Schema 2001
		/// </summary>
		/// <param name="val">Объект, для которого необходимо получить строку в формате XML Schema</param>
		/// <returns>Строка в формате XML Schema</returns>
		public static string XmlConvertToString(object value)
		{
			if (value == null)
				return null;

			if (value == DBNull.Value)
				return null;

			if (value is DateTime)
			{
				DateTime dt = (DateTime)value;
				if (dt.Hour == 0 && dt.Minute == 0 && dt.Second == 0)
					return dt.ToString("yyyy-MM-dd");

				return XmlConvert.ToString(dt, XmlDateTimeSerializationMode.Unspecified);
			}

			if (value is TimeSpan)
				return XmlConvert.ToString((TimeSpan)value);

			if (value is Guid)
				return XmlConvert.ToString((Guid)value);

			if (value is string)
				return value.ToString();

			if (value is bool)
				return XmlConvert.ToString((bool)value);

			if (value is byte)
				return XmlConvert.ToString((byte)value);

			if (value is short)
				return XmlConvert.ToString((short)value);

			if (value is int)
				return XmlConvert.ToString((int)value);

			if (value is long)
				return XmlConvert.ToString((long)value);

			if (value is double)
				return XmlConvert.ToString(Convert.ToDecimal((double)value));

			if (value is float)
				return XmlConvert.ToString(Convert.ToDecimal((float)value));

			if (value is decimal)
				return XmlConvert.ToString((decimal)value);

			if (value is IConvertible)
				return value.ToString();

			if (value.GetType().IsArray)
			{
				Type elementType = value.GetType().GetElementType();
				if (elementType.IsArray)
					throw new NotSupportedException("Serializing of multidimensional arrays is not supported.");

				StringBuilder sb = new StringBuilder();
				Array ar = (Array)value;
				string delimiter = CultureInfo.InvariantCulture.TextInfo.ListSeparator;
				if (ar.Length == 0)
					return delimiter;//TO-DO: try to create something more elegant

				List<string> objectStrings = new List<string>();
				foreach (object o in ar)
					objectStrings.Add(XmlConvertToString(o));

				bool anyItemContainsDelimiter = false;

				foreach (string s in objectStrings)
				{
					if (s.Contains(delimiter))
					{
						anyItemContainsDelimiter = true;
						break;
					}
				}

				string realDelimiter = anyItemContainsDelimiter ? "_" + delimiter + "_" : delimiter;
				for (int i = 0; i < objectStrings.Count; i++)
				{
					if (i > 0)
						sb.Append(realDelimiter);

					string str = objectStrings[i];
					sb.Append(anyItemContainsDelimiter ? str.Replace(delimiter, delimiter + delimiter) : str);
				}


				return sb.ToString();
			}

			return value.ToString();
		}

		/// <summary>
		/// Возвращает текстовое описание перечисления. Если перечисление имеет атрибут <see cref="System.ComponentModel.DescriptionAttribute"/>,
		/// то результатом метода будет значение свойства Description. В противном случае, результатом метода будет текстовое 
		/// значение перечисления.
		/// </summary>
		/// <typeparam name="T">Тип перечисления.</typeparam>
		/// <param name="enumValue">Значение перечисления.</param>
		/// <param name="getAcronym">Если енум поддерживает сокращёный вариант дескрипшена - получить акроним.</param>
		/// <returns>Описание перечисления.</returns>
		public static string GetEnumDescription<T>(T enumValue, bool getAcronym) where T : struct
		{
			Type t = typeof(T);
			if (t.IsEnum == false)
				throw new ArgumentException("T must be enum type.");

			string name = Enum.GetName(t, enumValue);

			if (name == null)
				throw new ArgumentException("Invalid enum.");

			FieldInfo fi = t.GetField(name);

			DescriptionAttribute attr = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, (typeof(DescriptionAttribute)));
			if (attr != null && string.IsNullOrEmpty(attr.Description) == false)
				name = attr.Description;

			if (getAcronym)
			{
				AcronymDescAttribute adAttr = attr as AcronymDescAttribute;
				if (adAttr != null && !string.IsNullOrEmpty(adAttr.Acronym))
				{
					name = adAttr.Acronym;
				}
			}

			return name;
		}

		public static string GetEnumDescription<T>(T enumValue) where T : struct
		{
			return GetEnumDescription<T>(enumValue, false);
		}

		public static Dictionary<T, string> GetEnumDesctiptions<T>() where T : struct
		{
			Dictionary<T, string> result = new Dictionary<T, string>();

			foreach (T f in Enum.GetValues(typeof(T)))
				result.Add(f, GetEnumDescription<T>(f));

			return result;
		}

		public static T? GetEnumByDescription<T>(string description, bool byAcronym) where T : struct
		{
			foreach (T f in Enum.GetValues(typeof(T)))
				if (GetEnumDescription<T>(f, byAcronym) == description) return f;
			return null;
		}

		public static Dictionary<int, string> GetEnumValuesOfList<T>(params T[] list) where T : struct
		{
			Dictionary<int, string> result = new Dictionary<int, string>();
			foreach (T f in list)
				result.Add(Convert.ToInt32(f), GetEnumDescription<T>(f));
			return result;
		}

		public static Dictionary<T, string> GetEnumDescriptionsOfList<T>(params T[] list) where T : struct
		{
			Dictionary<T, string> result = new Dictionary<T, string>();
			foreach (T f in list)
				result.Add(f, GetEnumDescription<T>(f));
			return result;
		}

		public static Dictionary<int, string> GetEnumValues<T>() where T : struct
		{
			return GetEnumValuesOfList<T>((T[])Enum.GetValues(typeof(T)));
		}

		public static IEnumerable<KeyValuePair<int?, string>> GetEnumValuesWithEmpty<T>(params T[] excludes) where T : struct
		{
			List<KeyValuePair<int?, string>> result = new List<KeyValuePair<int?, string>>();
			result.Add(new KeyValuePair<int?, string>(null, ""));

			Dictionary<T, object> excludesDict = new Dictionary<T, object>();
			if (excludes != null && excludes.Length > 0)
			{
				foreach (T t in excludes)
					excludesDict.Add(t, null);
			}

			foreach (T f in Enum.GetValues(typeof(T)))
			{
				if (excludesDict.ContainsKey(f) == false)
					result.Add(new KeyValuePair<int?, string>(Convert.ToInt32(f), GetEnumDescription<T>(f)));
			}

			return result;
		}

		public static IEnumerable<KeyValuePair<int?, string>> GetEnumValues<T>(params T[] excludes) where T : struct
		{
			List<KeyValuePair<int?, string>> result = new List<KeyValuePair<int?, string>>();
			Dictionary<T, object> excludesDict = new Dictionary<T, object>();
			if (excludes != null && excludes.Length > 0)
			{
				foreach (T t in excludes)
					excludesDict.Add(t, null);
			}
			foreach (T f in Enum.GetValues(typeof(T)))
			{
				if (excludesDict.ContainsKey(f) == false)
					result.Add(new KeyValuePair<int?, string>(Convert.ToInt32(f), GetEnumDescription<T>(f)));
			}
			return result;
		}

		public static IEnumerable<KeyValuePair<int?, string>> GetEnumValues<T>(List<T> excludes) where T : struct
		{
			List<KeyValuePair<int?, string>> result = new List<KeyValuePair<int?, string>>();
			Dictionary<T, object> excludesDict = new Dictionary<T, object>();
			if (excludes != null && excludes.Count > 0)
			{
				foreach (T t in excludes)
					excludesDict.Add(t, null);
			}
			foreach (T f in Enum.GetValues(typeof(T)))
			{
				if (excludesDict.ContainsKey(f) == false)
					result.Add(new KeyValuePair<int?, string>(Convert.ToInt32(f), GetEnumDescription<T>(f)));
			}
			return result;
		}

		public static string FormatToLegal(this DateTime? dt)
		{
			if (dt == null)
				return "";

			return dt.Value.ToString("D");
		}

		public static string FormatToLegal(this DateTime dt)
		{
			return FormatToLegal((DateTime?)dt);
		}

		public static string FormatToShort(this DateTime? dt)
		{
			if (dt == null)
				return "";

			return dt.Value.ToString("d");
		}

		public static string FormatToShort(this DateTime dt)
		{
			return FormatToShort((DateTime?)dt);
		}

		/// <summary>
		/// Считывает данные из ресурса указанной сборки и возвращает их как массив байт
		/// </summary>
		/// <param name="asm">Сборка</param>
		/// <param name="resourceName">Имя ресураса</param>
		/// <returns>Массив байт, соответствующий ресурсу</returns>
		public static byte[] ReadBytesFromResource(Assembly asm, string resourceName)
		{
			if (asm == null)
				throw new ArgumentNullException("asm");

			byte[] bytes = null;
			using (Stream resourceStream = asm.GetManifestResourceStream(resourceName))
				bytes = ReadBytesFromStream(resourceStream);

			return bytes;
		}

		private static byte[] ReadBytesFromStream(Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException("stream");

			int bytesRead;
			MemoryStream ms = new MemoryStream();
			byte[] buffer = new byte[4096];
			do
			{
				bytesRead = stream.Read(buffer, 0, buffer.Length);
				ms.Write(buffer, 0, bytesRead);
			}
			while (bytesRead > 0);

			return ms.ToArray();
		}

		public static T XmlConvertFromString<T>(string s)
		{
			Type t = typeof(T);
			if (t == typeof(string))
				return (T)(object)s;

			if (t == typeof(decimal))
				return (T)(object)XmlConvert.ToDecimal(s);

			if (t == typeof(Guid))
				return (T)(object)XmlConvert.ToGuid(s);

			if (t == typeof(DateTime))
				return (T)(object)XmlConvert.ToDateTime(s, XmlDateTimeSerializationMode.Unspecified);

			if (t == typeof(int))
				return (T)(object)XmlConvert.ToInt32(s);

			if (t == typeof(long))
				return (T)(object)XmlConvert.ToInt64(s);

			if (t == typeof(bool))
				return (T)(object)XmlConvert.ToBoolean(s);

			if (t == typeof(byte))
				return (T)(object)XmlConvert.ToByte(s);

			if (t == typeof(short))
				return (T)(object)XmlConvert.ToInt16(s);

			throw new ArgumentException(string.Format("Type \"{0}\" is not supported.", t.FullName));
		}

		public static T[] XmlConvertToArray<T>(string s)
		{
			if (s == null)
				return new T[0];

			string delimiter = CultureInfo.InvariantCulture.TextInfo.ListSeparator;
			string delimiter2 = "_" + delimiter + "_";
			bool anyItemContainsDelimiter = s.Contains(delimiter2);
			string realDelimiter = anyItemContainsDelimiter ? delimiter2 : delimiter;

			string[] parts = s.Split(new string[] { realDelimiter }, StringSplitOptions.RemoveEmptyEntries);
			List<T> l = new List<T>();
			foreach (string part in parts)
			{
				string part2 = anyItemContainsDelimiter ? part.Replace(delimiter + delimiter, delimiter) : part;
				l.Add(XmlConvertFromString<T>(part2));
			}

			return l.ToArray();
		}

		public static TOutput ConvertObject<TInput, TOutput>(TInput value)
		{
			object result = ConvertObject(value, typeof(TOutput));
			if (result == null)
				return default(TOutput);

			return (TOutput)result;
		}

		public static object ConvertObject(object value, Type targetType)
		{
			if (value == null)
				return null;

			string stringValue = value as string;
			if (stringValue != null && stringValue.Length == 0 && targetType != typeof(string))
				return null;

			if (targetType == null)
				throw new ArgumentNullException("targetType");

			if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
				targetType = targetType.GetGenericArguments()[0];

			if (value.GetType().IsSubclassOf(targetType) || value.GetType() == targetType)
				return value;

			TypeConverter converter = TypeDescriptor.GetConverter(value.GetType());
			if (converter != null && converter.CanConvertTo(targetType))
				return converter.ConvertTo(value, targetType);

			converter = TypeDescriptor.GetConverter(targetType);
			if (converter != null && converter.CanConvertFrom(value.GetType()))
				return converter.ConvertFrom(value);

			if (value is IConvertible)
			{
				object convertedValue = Convert.ChangeType(value, targetType);
				if (convertedValue.GetType().IsSubclassOf(targetType) || convertedValue.GetType() == targetType)
					return convertedValue;
			}

			return null;
		}

		public static string EncodeMimeHeader(string value, Encoding encoding, int maxLength)
		{
			if (string.IsNullOrEmpty(value))
				return value;

			if (encoding == null)
				encoding = Encoding.UTF8;

			if (maxLength <= 0)
				throw new ArgumentOutOfRangeException("maxLength", maxLength, "maxLength must be positive.");

			List<string> parts = new List<string>();
			const int partLength = 30;
			int partsCount = (int)Math.Ceiling((decimal)value.Length / partLength);

			for (int i = 0; i < partsCount; i++)
				parts.Add(value.Substring(i * partLength, Math.Min(partLength, value.Length - i * partLength)));

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < parts.Count; i++)
				sb.Append("=?" + encoding.BodyName + "?B?" + Convert.ToBase64String(encoding.GetBytes(parts[i])) + "?=");

			return sb.ToString();
		}

		/// <summary>
		/// Асинхронно отправляет сообщение.
		/// </summary>
		/// <param name="message">Сообщение для отправки.</param>
		public static void SendEmailAsync(MailMessage message)//string to, string subject, string body, bool isHtml)
		{
			SmtpClient smtp = new SmtpClient();

			if (message.From != null && smtp.Host != null)
			{
				System.Threading.ThreadPool.QueueUserWorkItem(delegate(object state)
				{
					try
					{
						SmtpClient smtp2 = new SmtpClient();
						MailMessage msg2 = (MailMessage)state;
						smtp2.Send(msg2);
					}
					catch // (Exception ex) - SK:consider refactoring
					{
						//do nothing
					}
				}, message);
			}
		}

		/// <summary>
		/// Создаёт сообщение. Отправитель определяется файлом smtp.config.
		/// </summary>
		/// <param name="to">Адрес получателя</param>
		/// <param name="subject">Тема</param>
		/// <param name="body">Тело</param>
		/// <param name="isHtml">Тело html или нет</param>
		/// <returns></returns>
		public static MailMessage CreateMailMessage(string to, string subject, string body, bool isHtml)
		{
			MailMessage msg = new MailMessage();

			if (msg.From != null)
			{
				msg.To.Add(to);

				if (string.IsNullOrEmpty(subject) == false)
					msg.Subject = EncodeMimeHeader(subject, Encoding.UTF8, 100);

				msg.BodyEncoding = Encoding.UTF8;
				msg.IsBodyHtml = isHtml;
				msg.Body = body;
			}

			return msg;
		}

		public static string Generate1CClientBankExchangeFile(string content)
		{
			string nowString = DateTime.Now.ToShortDateString();
			StringBuilder f = new StringBuilder();
			f.AppendLine("1CClientBankExchange");
			f.AppendLine("ВерсияФормата=1.01");
			f.AppendLine("Кодировка=Windows");
			f.AppendLine("Отправитель=Внешняя система");
			f.AppendLine("Получатель=");
			f.AppendLine("ДатаСоздания= " + nowString);
			f.AppendLine("ВремяСоздания=" + nowString);
			f.AppendLine("ДатаНачала=" + nowString);
			f.AppendLine("ДатаКонца=" + nowString);
			f.AppendLine(content);
			f.AppendLine("Конец файла");
			return f.ToString();
		}

		public static DateTime Max(DateTime value1, DateTime value2)
		{
			return value1 >= value2 ? value1 : value2;
		}

		public static bool ListsAreEqual<T>(IList<T> value1, IList<T> value2)
		{
			if ((value1 == null && value2 == null) || object.ReferenceEquals(value1, value2))
				return true;

			if (value1 == null || value2 == null || value1.Count != value2.Count)
				return false;

			for (int i = 0; i < value1.Count; i++)
			{
				if (object.Equals(value1[i], value2[i]) == false)
					return false;
			}

			return true;
		}

		public static DataTable Read1CFile(string fileContents, string sectionStartMarker, string sectionEndMarker, string tableName)
		{
			int startIndex = fileContents.IndexOf(sectionStartMarker);
			int endIndex = fileContents.IndexOf(sectionEndMarker);

			DataTable table = new DataTable(tableName);

			while ((startIndex > 0) && (endIndex > 0))
			{
				string section = fileContents.Substring(startIndex, (endIndex - startIndex));
				StringReader stringReader = new StringReader(section);

				DataRow dr = table.NewRow();

				// пропускаем первую строку секции (там нет значимых данных, кроме маркера начала)
				string line = stringReader.ReadLine();

				while (stringReader.Peek() >= 0)
				{
					// читаем строку с парой name=value
					line = stringReader.ReadLine();

					// получим name
					string name = Regex.Replace(line, "=.*", "").Trim();

					if (!string.IsNullOrEmpty(name))
					{
						if (!table.Columns.Contains(name))
							table.Columns.Add(name);

						// получим value
						dr[name] = Regex.Replace(line, name + "=", "").Trim();
					}
				}

				table.Rows.Add(dr);

				// чтение реализовано неэффективно - на больших файлах слишком много будет 
				// перезаписывания больших кусков файла. переписать на использование индексов
				// и вычленение только одной секции
				fileContents = fileContents.Substring(endIndex + sectionEndMarker.Length);
				startIndex = fileContents.IndexOf(sectionStartMarker);
				endIndex = fileContents.IndexOf(sectionEndMarker);
			}

			return table;
		}

		public static DataTable Read1CPaymentsFile(string fileContents, string sectionStartMarker,
			string sectionEndMarker)
		{
			return Read1CFile(fileContents, sectionStartMarker, sectionEndMarker, "payments");
		}

		private static MaskDescAttribute GetEnumMaskDesc<T>(T enumValue) where T : struct
		{
			Type t = typeof(T);
			if (t.IsEnum == false)
				throw new ArgumentException("T must be enum type.");

			string name = Enum.GetName(t, enumValue);

			if (name == null)
				throw new ArgumentException("Invalid enum.");

			FieldInfo fi = t.GetField(name);

			return (MaskDescAttribute)Attribute.GetCustomAttribute(fi, (typeof(MaskDescAttribute)));
		}

		public static string GetEnumMask<T>(T enumValue) where T : struct
		{
			MaskDescAttribute attr = GetEnumMaskDesc<T>(enumValue);
			if (attr != null && string.IsNullOrEmpty(attr.Mask) == false)
				return attr.Mask;

			return string.Empty;
		}

		public static string GetEnumWatermark<T>(T enumValue) where T : struct
		{
			MaskDescAttribute attr = GetEnumMaskDesc<T>(enumValue);
			if (attr != null && string.IsNullOrEmpty(attr.Watermark) == false)
				return attr.Watermark;

			return string.Empty;
		}

		public static string GetEnumRX<T>(T enumValue) where T : struct
		{
			MaskDescAttribute attr = GetEnumMaskDesc<T>(enumValue);
			if (attr != null && string.IsNullOrEmpty(attr.RX) == false)
				return attr.RX;

			return ".*";
		}

		public static T GetObjectFromXml<T>(XmlNode el) where T : class
		{
			if (el != null)
			{
				XmlSerializer serializer = new XmlSerializer(typeof(T));
				XmlReader reader = XmlReader.Create(new StringReader(el.OuterXml));
				return (T)serializer.Deserialize(reader);
			}

			return null;
		}

		/// <summary>
		/// говнокод, вызванный говнокодом в Ajax.MaskedEditBehavior.js приводящим к постоянным проблемам с нежесткими масками типа $$$$L-WW 999999
		/// добавляем пробелы в начало строки на разницу в количестве сиволов между маской и значением
		/// </summary>
		/// <param name="mask"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string FloatingMaskAgreeValue(string mask, string value)
		{
			if (string.IsNullOrEmpty(value))
				return string.Empty;

			StringBuilder sb = new StringBuilder(Math.Max(mask.Length, value.Length));
			sb.Append(value);
			for (int i = 0; i < mask.Length - value.Length; i++)
			{
				sb.Insert(0, ' ');
			}

			return sb.ToString();
		}

		public static byte[] GetImageFromResource(Assembly asm, string imgName)
		{
			foreach (string resourceName in asm.GetManifestResourceNames())
			{
				if (resourceName.Contains(imgName))
				{
					Stream img = asm.GetManifestResourceStream(resourceName);
					int imgLen = Convert.ToInt32(img.Length);
					byte[] imgBytes = new byte[imgLen];
					img.Read(imgBytes, 0, imgLen);
					return imgBytes;
				}
			}

			return null;
		}

		public static Guid? TryParseGuid(string s)
		{
			Regex rx = new Regex(@"[a-f\d]{8}-[a-f\d]{4}-[a-f\d]{4}-[a-f\d]{4}-[a-f\d]{12}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
			return rx.IsMatch(s) ? new Guid(s) : (Guid?)null;
		}


		/// <summary>
		/// adjust InsuranceCertificate like '12345678900'
		/// </summary>
		/// <param name="ic"></param>
		/// <returns></returns>
		public static string ClearInsuranceCertificate(string ic)
		{
			Regex rx = new Regex(@"^\d{3}[-\s]*\d{3}[-\s]*\d{3}[-\s]*\d{2}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
			if (rx.IsMatch(ic))
			{
				return new Regex(@"[-\s]", RegexOptions.Compiled).Replace(ic, "");
			}

			return ic;
		}

		/// <summary>
		/// adjust InsuranceCertificate like '123-456-789 00'
		/// </summary>
		/// <param name="ic"></param>
		/// <returns></returns>
		public static string FormatInsuranceCertificate(string ic)
		{
			Regex rx = new Regex(@"^\d{3}[-\s]*\d{3}[-\s]*\d{3}[-\s]*\d{2}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
			if (rx.IsMatch(ic))
			{
				StringBuilder sb = new StringBuilder(ClearInsuranceCertificate(ic));
				sb.Insert(3, '-');
				sb.Insert(7, '-');
				sb.Insert(11, ' ');
				return sb.ToString();
			}
			return ic;
		}
	}
}