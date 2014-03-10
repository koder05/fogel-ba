using System;
using System.Runtime.Serialization;

namespace RF.Excel
{
	[Serializable]
	public class ExcelReaderException : ApplicationException
	{
		public ExcelReaderException()
		{
		}

		public ExcelReaderException(string message)
			: base(message)
		{
		}

		public ExcelReaderException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public ExcelReaderException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
