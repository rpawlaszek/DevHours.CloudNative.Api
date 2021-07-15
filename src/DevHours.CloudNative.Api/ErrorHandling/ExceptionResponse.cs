/*
 * Consumed and adapted from: https://github.com/snatch-dev/Convey/blob/master/src/Convey.WebApi/src/Convey.WebApi/Exceptions/ExceptionResponse.cs
 */
using System.Net;

namespace DevHours.CloudNative.Api.ErrorHandling
{
    public class ExceptionResponse
    {
        public object Response { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}