using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Garnet.Serilog.Enricher.HttpContext.ResponseHeaders;

/// <summary>
/// Enrich LogEvent with HTTP response headers if present.
/// </summary>
public class GarnetResponseHeadersEnricher : GarnetHttpContextEnricherBase
{
    /// <summary>
    /// Enrich LogEvent with HTTP response headers if present.
    /// </summary>
    /// <param name="garnetHttpContextEnricherPropertyNameConfig">LogEvent property name configuration</param>
    /// <param name="httpContextAccessor">To access HttpContext to retrieve needed information</param>
    public GarnetResponseHeadersEnricher(
        GarnetHttpContextEnricherPropertyNameConfig garnetHttpContextEnricherPropertyNameConfig,
        IHttpContextAccessor httpContextAccessor)
        : base(garnetHttpContextEnricherPropertyNameConfig.ResponseHeaders, httpContextAccessor)
    {
    }

    /// <summary>
    /// Provide response headers or null if response has no headers
    /// </summary>
    /// <param name="httpContext">Current request context</param>
    /// <returns>Response headers or null if request has no headers</returns>
    protected override object ProvideLogObject(Microsoft.AspNetCore.Http.HttpContext httpContext)
    {
        return httpContext.Response.Headers.Any()
            ? httpContext.Response.Headers
                .ToDictionary(pair => pair.Key, pair => pair.Value.ToString())
            : null;
    }
}