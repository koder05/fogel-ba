using System;

namespace WebApi.Svc.Exceptions
{
    /// <summary>
    /// Exception type which represents the DataServiceException thrown by the ADO.NET Data Service
    /// </summary>
    public class DataServiceException : Exception
    {
        public DataServiceException(string Message, string StackTrace, Exception InternalException)
            : base(Message, InternalException)
        {
            _stackTrace = StackTrace;
        }
        private string _stackTrace;
        public override string StackTrace
        {
            get
            {
                return _stackTrace;
            }
        }
    }
}

