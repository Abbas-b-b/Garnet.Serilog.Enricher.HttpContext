using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Garnet.Serilog.Enricher.HttpContext.ResponseBody;

/// <summary>
/// A middleware to capture response body to be used for enriching log event
/// </summary>
internal class GarnetResponseBodyEnricherMiddleware
{
    internal const string ResponseBodyCacheKey = "GARNET.SERILOG.RESPONSEBODY.ENRICHER.MIDDLEWARE.CACHE";

    private readonly RequestDelegate _next;

    /// <summary>
    /// A middleware to capture response body to be used for enriching log event
    /// </summary>
    /// <param name="next">Next chain</param>
    public GarnetResponseBodyEnricherMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Use middleware Invoke method to capture response body
    /// </summary>
    /// <param name="httpContext"><see cref="HttpContext"/></param>
    public async Task InvokeAsync(Microsoft.AspNetCore.Http.HttpContext httpContext)
    {
        var originalBody = httpContext.Response.Body;

        try
        {
            await using var memoryStream = new MemoryStream();

            httpContext.Response.Body = memoryStream;

            try
            {
                await _next(httpContext);
            }
            catch (Exception)
            {
                // ignored
            }

            memoryStream.Position = 0;

            var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();

            httpContext.Items.TryAdd(ResponseBodyCacheKey, responseBody);

            memoryStream.Position = 0;

            await memoryStream.CopyToAsync(originalBody);
        }
        finally
        {
            httpContext.Response.Body = originalBody;
        }
    }
}