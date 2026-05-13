using System;
using System.Runtime.Serialization;

namespace OfficeConnect_Web.Controllers
{
    [Serializable]
    internal class WebFaultException : Exception
    {
        private Exception ex;

        public WebFaultException()
        {
        }

        public WebFaultException(Exception ex)
        {
            this.ex = ex;
        }

        public WebFaultException(string message) : base(message)
        {
        }

        public WebFaultException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WebFaultException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}