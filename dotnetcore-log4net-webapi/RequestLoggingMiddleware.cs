using log4net;
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
        private readonly ILog _logger = log4net.LogManager.GetLogger("RequestLoggingMiddleware");

        public RequestLoggingMiddleware()
        {
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

                String message = String.Format("Request {0} {1} => {2}",
                    context.Request?.Method,
                    context.Request?.Path.Value,
                    context.Response?.StatusCode);

                _logger.Info(message);
            }
        }
    }
}
