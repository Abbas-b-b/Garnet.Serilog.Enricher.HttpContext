using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Garnet.Serilog.Enricher.HttpContext;

/// <summary>
/// A base class for other enrichers for the sake of simplicity with some boilerplate codes
/// With LogObject caching enabled for the same HttpContext, therefore LogObject is requested once per incoming request.
/// </summary>
public abstract class GarnetHttpContextEnricherBaseWithCache : GarnetHttpContextEnricherBase
{
    private const string CacheKeyPrefix = "GARNET.SERILOG.ENRICHER_";

    private string CacheKey => CacheKeyPrefix + PropertyKey;

    /// <summary>
    /// A base class for other enrichers for the sake of simplicity with some boilerplate codes
    /// With LogObject caching enabled for the same HttpContext, therefore LogObject is requested once per incoming request.
    /// </summary>
    /// <param name="propertyKey">Log event property name</param>
    /// <param name="httpContextAccessor">To access request HttpContext to retrieve needed information</param>
    /// <param name="supportsRedaction">Controls whether this enricher supports redaction</param>
    protected GarnetHttpContextEnricherBaseWithCache(string propertyKey, IHttpContextAccessor httpContextAccessor,
        bool supportsRedaction = true)
        : base(propertyKey, httpContextAccessor, supportsRedaction)
    {
    }

    /// <summary>
    /// Enrich <paramref name="logEvent"/> by calling <see cref="GarnetHttpContextEnricherBase.ProvideLogObject"/> method if HttpContext is not null.
    /// Null or empty string result of method <see cref="GarnetHttpContextEnricherBase.ProvideLogObject"/> will be ignored.
    /// The result of <see cref="GarnetHttpContextEnricherBase.ProvideLogObject"/> is cached in HttpContext per incoming request if it return not null and not empty result
    /// </summary>
    /// <param name="logEvent">The log event to enrich.</param>
    /// <param name="propertyFactory">Factory for creating new properties to add to the event.</param>
    public override void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var httpContext = HttpContextAccessor?.HttpContext;
        
        if (httpContext is null)
        {
            return;
        }

        if (!httpContext.Items.TryGetValue(CacheKey, out var logData))
        {
            if (ShouldSkipEnrichment(httpContext))
            {
                return;
            }
            
            logData = ProvideLogObjectAndRedact(httpContext);

            if (logData is null || logData is string stringData && string.IsNullOrEmpty(stringData))
            {
                return;
            }

            httpContext.Items.TryAdd(CacheKey, logData);
        }

        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(PropertyKey, logData, true));
    }
}