using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace Garnet.Serilog.Enricher.HttpContext.RequestUrl;

/// <summary>
/// Enrich LogEvent with HTTP request URL
/// </summary>
public class GarnetRequestUrlEnricher : GarnetHttpContextEnricherBaseWithCache
{
    /// <summary>
    /// Enrich LogEvent with HTTP request URL
    /// </summary>
    /// <param name="garnetHttpContextEnricherPropertyNameConfig">LogEvent property name configuration</param>
    /// <param name="httpContextAccessor">To access HttpContext to retrieve needed information</param>
    public GarnetRequestUrlEnricher(
        GarnetHttpContextEnricherPropertyNameConfig garnetHttpContextEnricherPropertyNameConfig,
        IHttpContextAccessor httpContextAccessor)
        : base(garnetHttpContextEnricherPropertyNameConfig.RequestUrl, httpContextAccessor)
    {
    }

    /// <summary>
    /// Provide request URL
    /// </summary>
    /// <param name="httpContext">Current request context</param>
    /// <returns>Request URL</returns>
    protected override object ProvideLogObject(Microsoft.AspNetCore.Http.HttpContext httpContext)
    {
        return httpContext.Request.GetDisplayUrl();
    }
}