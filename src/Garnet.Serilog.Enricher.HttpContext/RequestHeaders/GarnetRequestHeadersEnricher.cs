using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Garnet.Serilog.Enricher.HttpContext.RequestHeaders;

/// <summary>
/// Enrich LogEvent with HTTP request headers if present.
/// </summary>
public class GarnetRequestHeadersEnricher : GarnetHttpContextEnricherBaseWithCache
{
    /// <summary>
    /// Enrich LogEvent with HTTP request headers if present.
    /// </summary>
    /// <param name="garnetHttpContextEnricherPropertyNameConfig">LogEvent property name configuration</param>
    /// <param name="httpContextAccessor">To access HttpContext to retrieve needed information</param>
    public GarnetRequestHeadersEnricher(
        GarnetHttpContextEnricherPropertyNameConfig garnetHttpContextEnricherPropertyNameConfig,
        IHttpContextAccessor httpContextAccessor)
        : base(garnetHttpContextEnricherPropertyNameConfig.RequestHeaders, httpContextAccessor)
    {
    }

    /// <summary>
    /// Provide request headers or null if request has no headers
    /// </summary>
    /// <param name="httpContext">Current request context</param>
    /// <returns>Request headers or null if request has no headers</returns>
    protected override object ProvideLogObject(Microsoft.AspNetCore.Http.HttpContext httpContext)
    {
        return httpContext.Request.Headers.Any()
            ? httpContext.Request.Headers
                .ToDictionary(pair => pair.Key, pair => pair.Value.ToString())
            : null;
    }
}