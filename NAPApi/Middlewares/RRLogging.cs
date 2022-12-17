using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using NAPApi.Help;
using System.Threading.Tasks;

namespace NAPApi.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class RRLogging
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RRLogging> logger;

        public RRLogging(RequestDelegate next, ILogger<RRLogging> logger)
        {
            _next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            // define variable to save loge
            string data = "";
            // init body request
            var request = await ReadBodyFromRequest(httpContext.Request);
            data += httpContext.Request.Path +"/"+httpContext.Request.Path.ToString() + "\n";
            logger.LogInformation("Request: {request}", request);
            data += "Request Data: " + request + "\n";
            // Temporarily replace the HttpResponseStream, which is a write-only stream, with a MemoryStream to capture it's value in-flight.
            var originalResponseBody = httpContext.Response.Body;
            using var newResponseBody = new MemoryStream();
            httpContext.Response.Body = newResponseBody;

            // Call the next middleware in the pipeline
            await _next(httpContext);

            // init response
            newResponseBody.Seek(0, SeekOrigin.Begin);
            var responseBodyText = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
            
            logger.LogInformation("Response: {response}", responseBodyText);
            data += "StatusCode: " + httpContext.Response.StatusCode + "\n";
            data += "response Data: " + responseBodyText + "\n\t\t****\n ";
            newResponseBody.Seek(0, SeekOrigin.Begin);
            await newResponseBody.CopyToAsync(originalResponseBody);
            FileLogger.GetInstance().saveData(data);
        }

        private static async Task<string> ReadBodyFromRequest(HttpRequest request)
        {
            // Ensure the request's body can be read multiple times (for the next middlewares in the pipeline).
            request.EnableBuffering();

            using var streamReader = new StreamReader(request.Body, leaveOpen: true);
            var requestBody = await streamReader.ReadToEndAsync();

            // Reset the request's body stream position for next middleware in the pipeline.
            request.Body.Position = 0;
            return requestBody;
        }

    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class RRLoggingExtensions
    {
        public static IApplicationBuilder UseRRLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RRLogging>();
        }
    }
}
