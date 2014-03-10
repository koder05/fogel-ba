using System;
using System.Net;
using System.Xml.Linq;

namespace WebApi.Svc.Exceptions
{
    /// <summary>
    /// Helper class to de-serialize DataServiceExceptions thrown by an ADO.NET Data Service
    /// </summary>
    public static class DataServiceExceptionAtomParser
    {
        /// <summary>
        /// Pass in the Exception recieved from an Execute / SaveChanges call 
        /// to rethrow the actual DataServiceException thrown by the ADO.NET Data Service
        /// </summary>
        /// <param name="dsRexception">The Exception thrown by the client library 
        /// in response to an Execute/SaveChanges call</param>
        public static void Throw(Exception dsRexception)
        {
            Exception baseException = dsRexception.GetBaseException();
            XDocument xDoc = XDocument.Parse(baseException.Message);
            if (xDoc != null)
            {
                throw ParseException(xDoc.Root, true);
            }
        }

        /// <summary>
        /// Parses the Exception object to determine if it contains a DataServiceException 
        /// and de-serializes the Exception message into a DataServiceException.
        /// </summary>
        /// <param name="dsRexception">The Exception thrown by the client library 
        /// in response to an Execute/SaveChanges call </param>
        /// <param name="dataServiceException">The DataServiceException thrown by the ADO.NET Data Service</param>
        /// <returns>true if we are able to parse the response into a DataServiceException,false if not</returns>
        public static bool TryParse(Exception dsRexception, out Exception dataServiceException)
        {
            bool parseSucceeded = false;
            try
            {
                Exception baseException = dsRexception.GetBaseException();
                XDocument xDoc = XDocument.Parse(baseException.Message);
                dataServiceException = ParseException(xDoc.Root, false);
                parseSucceeded = dataServiceException != null;
            }
            catch
            {
                dataServiceException = dsRexception;
                parseSucceeded = false;
            }
            return parseSucceeded;
        }

        #region Variables
        static readonly string DataServicesMetadataNamespace = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";
        static XName xnCode = XName.Get("code", DataServicesMetadataNamespace);
        static XName xnType = XName.Get("type", DataServicesMetadataNamespace);
        static XName xnMessage = XName.Get("message", DataServicesMetadataNamespace);
        static XName xnStackTrace = XName.Get("stacktrace", DataServicesMetadataNamespace);
        static XName xnInternalException = XName.Get("internalexception", DataServicesMetadataNamespace);
        static XName xnInnerError = XName.Get("innererror", DataServicesMetadataNamespace);
        #endregion

        private static DataServiceException ParseException(XElement errorElement, bool throwOnFailure)
        {
            switch (errorElement.Name.LocalName)
            {
                case "error":
                case "innererror":
                    DataServiceException internalException = errorElement.Element(xnInnerError) != null ? ParseException(errorElement.Element(xnInnerError), throwOnFailure) : null;
                    string message = errorElement.Element(xnMessage) != null ? errorElement.Element(xnMessage).Value.ToString() : String.Empty;
                    string stackTrace = errorElement.Element(xnStackTrace) != null ? errorElement.Element(xnStackTrace).Value.ToString() : String.Empty;
                    return new DataServiceException(message, stackTrace, internalException);

                default:
                    if (throwOnFailure)
                    {
                        throw new InvalidOperationException("Could not parse Exception");
                    }
                    else
                    {
                        return null;
                    }
            }
        }
    }
}
