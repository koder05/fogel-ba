using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Xml;

namespace RF.Common
{
	public static class ConfigurationManager
	{
		public static string MainDbConnectionString
		{
			get
			{
				ConnectionStringSettings settings = System.Configuration.ConfigurationManager.ConnectionStrings["MainDb2"];
				if (settings == null || string.IsNullOrEmpty(settings.ConnectionString))
					throw new ConfigurationErrorsException("MainDb connection string is absent.");

				return settings.ConnectionString;
			}
		}

		/// <summary>
		/// Возвращает строку подключения для копии БД. От
		/// </summary>
		public static string ReportingDbConnectionString
		{
			get
			{
				ConnectionStringSettings settings = System.Configuration.ConfigurationManager.ConnectionStrings["ReportingDb"];
				if (settings == null || string.IsNullOrEmpty(settings.ConnectionString))
					throw new ConfigurationErrorsException("ReportingDb connection string is absent.");

				return settings.ConnectionString;
			}
		}


		public static string LogDbConnectionString
		{
			get
			{
				ConnectionStringSettings settings = System.Configuration.ConfigurationManager.ConnectionStrings["LogDb"];
				if (settings == null || string.IsNullOrEmpty(settings.ConnectionString))
					throw new ConfigurationErrorsException("LogDb connection string is absent.");

				return settings.ConnectionString;
			}
		}

		/// <summary>
		/// Возвращает строку подключения к БД с файлами
		/// </summary>
		public static string FilesDbConnectionString
		{
			get
			{
				ConnectionStringSettings settings = System.Configuration.ConfigurationManager.ConnectionStrings["FilesDb"];
				if (settings == null || string.IsNullOrEmpty(settings.ConnectionString))
					throw new ConfigurationErrorsException("Отсутствует строка подключения к БД с файлами.");

				return settings.ConnectionString;
			}
		}

		public static Uri AtlasProductionUri
		{
			get
			{
				string uri = System.Configuration.ConfigurationManager.AppSettings["AtlasProductionUri"];
				if (string.IsNullOrEmpty(uri))
					throw new ConfigurationErrorsException("AtlasProductionUri appsetting is absent.");

				return new Uri(uri);
			}
		}

		public static string GetConfiguratonParameter(string parameterName)
		{
			if (parameterName == null)
				throw new ArgumentNullException("parameterName");

			string result = System.Configuration.ConfigurationManager.AppSettings[parameterName];
			if (result == null)
				return "";

			return result;
		}

		public static string GetDefaultInsurerInn()
		{
			return GetConfiguratonParameter("DefaultInsurerInn");
		}

		public static Guid GetDefaultInsurerCompanyID()
		{
			return new Guid(GetConfiguratonParameter("DefaultInsurerCompanyID"));
		}

		public static string GetDefaultPfrXmlRegNumber()
		{
			return GetConfiguratonParameter("DefaultPfrXmlRegNumber");
		}

		public static string GetPfrInn()
		{
			return GetConfiguratonParameter("PFRInn");
		}

		public static Guid GetPfrInsurerID()
		{
			return new Guid(GetConfiguratonParameter("PfrInsurerID"));
		}

		public static string GetDefaultCurrencyCode()
		{
			return GetConfiguratonParameter("DefaultCurrency");
		}

        public static string GetDefaultCountryIso2Code()
        {
            return GetConfiguratonParameter("DefaultCountry");
        }

		public static string GetBikValidationServiceUriFormat()
		{
			string uriFormatStr = GetConfiguratonParameter("BikValidationServiceUri");
//			if (string.IsNullOrEmpty(uriFormatStr))
//				throw new ConfigurationErrorsException("Необходимо задать параметр BikValidationServiceUri в конфигурационной секции AppSettings.");

			return uriFormatStr;
		}

		private static int GetIntConfiguratonParameter(string parameterName, int defVal)
		{
			string value = GetConfiguratonParameter(parameterName);
			int result;

			if (!string.IsNullOrEmpty(value) && Int32.TryParse(value, out result))
				return result;

			return defVal;
		}

		public static int GetPasswordChangeRequestExpirationHours()
		{
			return GetIntConfiguratonParameter("PasswordChangeRequestExpirationHours", 12);
		}

		public static int GetMaxPasswordChangeRequestPerDay()
		{
			return GetIntConfiguratonParameter("MaxPasswordChangeRequestPerDay", 2);
		}

		public static int GetPasswordExpirationPeriodMonths()
		{
			return GetIntConfiguratonParameter("PasswordExpirationPeriodMonths", 6);
		}

		public static string GetSupportEmail()
		{
			string result = GetConfiguratonParameter("SupportEmail");
			if (string.IsNullOrEmpty(result))
				result = "support@regionfund.ru";

			return result;
		}

		public static string GetSupportPhone()
		{
			string result = GetConfiguratonParameter("SupportPhone");
			if (string.IsNullOrEmpty(result))
				result = "+7(499)134 1111";

			return result;
		}

        public static string GetRegionfundPostmailAddress()
        {
            string result = GetConfiguratonParameter("RegionfundPostmailAddress");
            if (string.IsNullOrEmpty(result))
                result = "117335, Москва г., Вавилова ул., д.79, корп.1, оф.2.";

            return result;
        }
	}
}