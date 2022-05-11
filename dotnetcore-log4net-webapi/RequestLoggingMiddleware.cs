using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetcore_log4net_webapi
{
    public class RequestLoggingMiddleware : IMiddleware
    {
        //private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestLoggingMiddleware(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<RequestLoggingMiddleware>();
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            } 
            finally
            {
                log4net.ThreadContext.Properties["resstatuscode"] = context.Response?.StatusCode;
                log4net.ThreadContext.Properties["reqmethod"] = context.Request?.Method;
                log4net.ThreadContext.Properties["isrequest"] = true;
                log4net.ThreadContext.Properties["uniqueidentifier"] = new Guid("1f33ffb6-3f3d-49b8-9594-4fb2ca4b5aa6");

                _logger.LogInformation(
                    "Request {method} {url} => {statusCode}",
                    context.Request?.Method,
                    context.Request?.Path.Value,
                    context.Response?.StatusCode);
            }
        }
    }
}
