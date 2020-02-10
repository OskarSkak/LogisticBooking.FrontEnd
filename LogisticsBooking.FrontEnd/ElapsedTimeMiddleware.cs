using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace LogisticsBooking.FrontEnd
{
    public class ElapsedTimeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        
        
        public ElapsedTimeMiddleware(RequestDelegate next, ILogger<ElapsedTimeMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            var sw = new Stopwatch();
            sw.Start();

            await _next(context);
            sw.Stop();
            var isHtml = context.Response.ContentType?.ToLower().Contains("text/html");
            if (context.Response.StatusCode == 200 && isHtml.GetValueOrDefault())
            {
                
                
                using (LogContext.PushProperty("X-Correlation-ID", context.TraceIdentifier))
                {
                    _logger.LogInformation("FRONT - {path} executed in  {time}ms" ,context.Request.Path, sw.ElapsedMilliseconds);
                }
                
                
            }
        }
    }
}