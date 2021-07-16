/*
 * Consumed and adapted from: https://github.com/snatch-dev/Convey/blob/master/src/Convey.WebApi/src/Convey.WebApi/Exceptions/ErrorHandlerMiddleware.cs
 */
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace DevHours.CloudNative.Api.ErrorHandling
{
    internal sealed class ErrorHandlerMiddleware : IMiddleware
    {
        private readonly IMapper mapper;
        private readonly ILogger<ErrorHandlerMiddleware> logger;

        public ErrorHandlerMiddleware(IMapper mapper,
            ILogger<ErrorHandlerMiddleware> logger)
        {
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, exception.Message);
                await HandleErrorAsync(context, exception);
            }
        }

        private async Task HandleErrorAsync(HttpContext context, Exception exception)
        {
            var exceptionResponse = mapper.Map<ExceptionResponse>(exception);
            context.Response.StatusCode = (int)(exceptionResponse?.StatusCode ?? HttpStatusCode.BadRequest);
            var response = exceptionResponse?.Response;

            if (response is null)
            {
                await context.Response.WriteAsync(string.Empty);
                return;
            }

            context.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(context.Response.Body, exceptionResponse.Response);
        }
    }
}