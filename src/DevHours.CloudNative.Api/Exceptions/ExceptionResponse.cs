/*
 * Consumed from: https://github.com/snatch-dev/Convey/blob/master/src/Convey.WebApi/src/Convey.WebApi/Exceptions/ExceptionResponse.cs
 */
using System.Net;

namespace DevHours.CloudNative.Api.Exceptions
{
    public class ExceptionResponse
    {
        public object Response { get; }
        public HttpStatusCode StatusCode { get; }

        public ExceptionResponse(object response, HttpStatusCode statusCode)
        {
            Response = response;
            StatusCode = statusCode;
        }
    }
}