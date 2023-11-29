using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Garnet.Serilog.Enricher.HttpContext.RequestBody;

/// <summary>
/// Enrich LogEvent with HTTP request body if present.
/// </summary>
public class GarnetRequestBodyEnricher : GarnetHttpContextEnricherBaseWithCache
{
    /// <summary>
    /// Enrich LogEvent with HTTP request body if present.
    /// </summary>
    /// <param name="garnetHttpContextEnricherPropertyNameConfig">LogEvent property name configuration</param>
    /// <param name="httpContextAccessor">To access HttpContext to retrieve needed information</param>
    public GarnetRequestBodyEnricher(
        GarnetHttpContextEnricherPropertyNameConfig garnetHttpContextEnricherPropertyNameConfig,
        IHttpContextAccessor httpContextAccessor)
        : base(garnetHttpContextEnricherPropertyNameConfig.RequestBody, httpContextAccessor)
    {
    }

    /// <summary>
    /// Provide request body object or null if request has no body
    /// </summary>
    /// <param name="httpContext">Current request context</param>
    /// <returns>Request body object or null if request has no body</returns>
    protected override object ProvideLogObject(Microsoft.AspNetCore.Http.HttpContext httpContext)
    {
        return httpContext.Items.TryGetValue(GarnetHttpLoggingSink<object>.RequestBodyCacheKey, out var value)
            ? Configuration.Redactions.Aggregate(value?.ToString(), (s, redaction) => redaction.Redact(s, httpContext))
            : null;
    }
}