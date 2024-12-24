using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Garnet.Serilog.Enricher.HttpContext.ResponseBody;

/// <summary>
/// Enrich LogEvent with HTTP response body if present.
/// </summary>
public class GarnetResponseBodyEnricher : GarnetHttpContextEnricherBaseWithCache
{
    /// <summary>
    /// Enrich LogEvent with HTTP response body if present.
    /// </summary>
    /// <param name="garnetHttpContextEnricherPropertyNameConfig">LogEvent property name configuration</param>
    /// <param name="httpContextAccessor">To access HttpContext to retrieve needed information</param>
    public GarnetResponseBodyEnricher(
        GarnetHttpContextEnricherPropertyNameConfig garnetHttpContextEnricherPropertyNameConfig,
        IHttpContextAccessor httpContextAccessor)
        : base(garnetHttpContextEnricherPropertyNameConfig.ResponseBody, httpContextAccessor)
    {
    }

    /// <summary>
    /// Provide response body object or null if response has no body or response body is not available at time of calling this method
    /// </summary>
    /// <param name="httpContext">Current request context</param>
    /// <returns>Response body object or null if request has no body or response body is not available at time of calling this method</returns>
    protected override object ProvideLogObject(Microsoft.AspNetCore.Http.HttpContext httpContext)
    {
        return httpContext.Items.TryGetValue(GarnetHttpLoggingSink<object>.ResponseBodyCacheKey, out var value)
            ? value
            : null;
    }
}