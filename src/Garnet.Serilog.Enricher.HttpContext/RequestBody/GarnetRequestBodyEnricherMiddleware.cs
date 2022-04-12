using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Garnet.Serilog.Enricher.HttpContext.RequestBody;

/// <summary>
/// A middleware to capture request body to be used for enriching log event
/// </summary>
internal class GarnetRequestBodyEnricherMiddleware
{
    internal const string RequestBodyCacheKey = "GARNET.SERILOG.REQUESTBODY.ENRICHER.MIDDLEWARE.CACHE";

    private readonly RequestDelegate _next;

    /// <summary>
    /// A middleware to capture request body to be used for enriching log event
    /// </summary>
    /// <param name="next">Next chain</param>
    public GarnetRequestBodyEnricherMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    /// <summary>
    /// Use middleware Invoke method to capture request body
    /// </summary>
    /// <param name="httpContext"><see cref="HttpContext"/></param>
    public async Task InvokeAsync(Microsoft.AspNetCore.Http.HttpContext httpContext)
    {
        try
        {
            httpContext.Request.EnableBuffering();

            if (httpContext.Request.ContentLength is null or <= 0 ||
                !CanReadHttpContextRequestStream(httpContext.Request.Body))
            {
                return;
            }

            var position = httpContext.Request.Body.Position;

            var bodyStream = httpContext.Request.Body;

            bodyStream.Position = 0;

            var buffer = new byte[Convert.ToInt32(httpContext.Request.ContentLength)];

            await httpContext.Request.Body.ReadAsync(buffer, 0, buffer.Length);

            var requestBody = Encoding.UTF8.GetString(buffer);

            httpContext.Request.Body.Position = position;

            httpContext.Items.TryAdd(RequestBodyCacheKey, requestBody);
        }
        finally
        {
            await _next(httpContext);
        }
    }

    /// <summary>
    /// Checks whether <paramref name="stream"/> can be read
    /// </summary>
    /// <param name="stream">A stream to check readability</param>
    /// <returns>TRUE if <paramref name="stream"/> can be read, otherwise FALSE</returns>
    private static bool CanReadHttpContextRequestStream(Stream stream)
    {
        try
        {
            _ = stream.Position;

            return stream.CanRead;
        }
        catch (Exception)
        {
            return false;
        }
    }
}