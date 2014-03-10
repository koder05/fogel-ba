using System;
using System.Runtime.Serialization;

namespace RF.Reporting
{
	/// <summary>
	/// Исключение, возникающее, когда не был найден шаблон отчёта
	/// </summary>
	[Serializable]
	public class ReportNotFoundException : ApplicationException
	{
		public ReportNotFoundException()
		{
		}

		public ReportNotFoundException(string message)
			: base(message)
		{
		}

		public ReportNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public ReportNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}