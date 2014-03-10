using System;
using System.Diagnostics;

namespace RF.Reporting
{
	[DebuggerDisplay("ReportName = {Name}, ContentType = {ContentType}, Language={LanguageCode}")]
	internal class ReportFullName
	{
		private string m_Name;
		private string m_ContentType;
		private string m_LanguageCode;

		public ReportFullName(string name, string contentType)
		{
			this.m_Name = name;
			this.m_ContentType = contentType;
		}

		public ReportFullName(string name, string contentType, string languageCode)
			: this(name, contentType)
		{
			this.m_LanguageCode = languageCode;
		}

		public string Name
		{
			get { return this.m_Name; }
		}

		public string ContentType
		{
			get { return this.m_ContentType; }
		}

		/// <summary>
		/// Код языка (2-буквенный)
		/// </summary>
		public string LanguageCode
		{
			get { return this.m_LanguageCode; }
		}

		public override bool Equals(object obj)
		{
			if (obj == null || obj is ReportFullName == false)
				return false;

			ReportFullName rfn = (ReportFullName)obj;
			return this.ContentType == rfn.ContentType && this.Name == rfn.Name && StringComparer.InvariantCultureIgnoreCase.Compare(this.LanguageCode, rfn.LanguageCode) == 0;
		}

		public override int GetHashCode()
		{
			int hashCode = 0;

			if(this.Name != null)
				hashCode += this.Name.GetHashCode();

			if(this.ContentType != null)
				hashCode += this.ContentType.GetHashCode();

			if (this.LanguageCode != null)
				hashCode += this.LanguageCode.ToLowerInvariant().GetHashCode();

			return hashCode;
		}

		public static bool operator ==(ReportFullName a, ReportFullName b)
		{
			if (object.ReferenceEquals(a, b))
				return true;

			if (((object)a == null) || ((object)b == null))
				return false;

			return a.Equals(b);
		}

		public static bool operator !=(ReportFullName a, ReportFullName b)
		{
			return !(a == b);
		}
	}
}