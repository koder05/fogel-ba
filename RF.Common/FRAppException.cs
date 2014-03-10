using System;
using System.Runtime.Serialization;

namespace RF.Common
{
	/// <summary>
	/// Базовый класс для программных исключений Domain Model
	/// </summary>
	public class RFAppException : ApplicationException
	{
		public RFAppException()
			: base()
		{
		}

		public RFAppException(string message)
			: base(message)
		{
		}

		public RFAppException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected RFAppException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}

	public class RFInvalidOperationException : InvalidOperationException
	{
		public RFInvalidOperationException()
			: base()
		{
		}

		public RFInvalidOperationException(string message)
			: base(message)
		{
		}

		public RFInvalidOperationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected RFInvalidOperationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
