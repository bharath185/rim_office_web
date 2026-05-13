using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace OfficeConnect_Web.Controllers
{
    public class CustomApiException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }

        public CustomApiException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public CustomApiException(HttpStatusCode statusCode, string message, Exception innerException) : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}