using System.Collections.Generic;
using System.Linq;
using Garnet.Serilog.Enricher.HttpContext.Configuration;
using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Garnet.Serilog.Enricher.HttpContext;

/// <summary>
/// A base class for other enrichers for the sake of simplicity with some boilerplate codes
/// Without caching LogObject in the same HttpContext, therefore LogObject is request everytime needed.
/// </summary>
public abstract class GarnetHttpContextEnricherBase : ILogEventEnricher
{
    private GarnetHttpContextEnrichmentConfiguration _configuration;

    /// <summary>
    /// Log event property name
    /// </summary>
    protected readonly string PropertyKey;

    /// <summary>
    /// To access request HttpContext to retrieve needed information
    /// </summary>
    protected readonly IHttpContextAccessor HttpContextAccessor;

    /// <summary>
    /// Controls whether this enricher supports redaction
    /// </summary>
    protected readonly bool SupportsRedaction;

    /// <summary>
    /// A base class for other enrichers for the sake of simplicity with some boilerplate codes
    /// Without caching LogObject in the same HttpContext, therefore LogObject is request everytime needed.
    /// </summary>
    /// <param name="propertyKey">Log event property name</param>
    /// <param name="httpContextAccessor">To access request HttpContext to retrieve needed information</param>
    /// <param name="supportsRedaction">Controls whether this enricher supports redaction</param>
    protected GarnetHttpContextEnricherBase(string propertyKey, IHttpContextAccessor httpContextAccessor,
        bool supportsRedaction = true)
    {
        PropertyKey = propertyKey;
        HttpContextAccessor = httpContextAccessor;
        SupportsRedaction = supportsRedaction;
    }

    /// <summary>
    /// Retrieve configuration for this Enricher
    /// </summary>
    protected GarnetHttpContextEnrichmentConfiguration Configuration
    {
        get { return _configuration ??= GarnetConfigProvider.GetConfiguration(this); }
    }

    /// <summary>
    /// Enrich <paramref name="logEvent"/> by calling <see cref="ProvideLogObject"/> method if HttpContext is not null.
    /// Null or empty string result of method <see cref="ProvideLogObject"/> will be ignored.
    /// </summary>
    /// <param name="logEvent">The log event to enrich.</param>
    /// <param name="propertyFactory">Factory for creating new properties to add to the event.</param>
    public virtual void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var httpContext = HttpContextAccessor?.HttpContext;

        if (httpContext is null || ShouldSkipEnrichment(httpContext))
        {
            return;
        }

        var logData = ProvideLogObjectAndRedact(httpContext);

        if (logData is null || logData is string stringData && string.IsNullOrEmpty(stringData))
        {
            return;
        }

        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(PropertyKey, logData, true));
    }

    /// <summary>
    /// Check skip enrichment based on <see cref="GarnetHttpContextEnrichmentConfiguration.RequestFilter"/>
    /// </summary>
    /// <param name="httpContext">HttpContext to test the filter</param>
    /// <returns>True if enrichment should be skipped otherwise False</returns>
    protected virtual bool ShouldSkipEnrichment(Microsoft.AspNetCore.Http.HttpContext httpContext)
    {
        if (Configuration.RequestFilter is null)
        {
            return false;
        }
        
        return !Configuration.RequestFilter(httpContext);
    }

    /// <summary>
    /// This is called by <see cref="Enrich"/> to get log object to enrich LogEvent.
    /// This method won't get called if HttpContext is null
    /// </summary>
    /// <param name="httpContext">Current request context</param>
    /// <returns>Any object that wished to enrich LogEvent</returns>
    protected abstract object ProvideLogObject(Microsoft.AspNetCore.Http.HttpContext httpContext);

    /// <summary>
    /// This is called by <see cref="Enrich"/> to get log object to enrich LogEvent after applying the redactions (<see cref="ProvideLogObject"/>).
    /// This method won't get called if HttpContext is null
    /// </summary>
    /// <param name="httpContext">Current request context</param>
    /// <returns>Any object that wished to enrich LogEvent</returns>
    protected virtual object ProvideLogObjectAndRedact(Microsoft.AspNetCore.Http.HttpContext httpContext)
    {
        var logData = ProvideLogObject(httpContext);

        if (logData is null)
        {
            return null;
        }
        
        if (SupportsRedaction && Configuration.Redactions is not null && Configuration.Redactions.Count > 0)
        {
            logData = logData switch
            {
                string strData =>
                    Configuration.Redactions.Aggregate(strData, (str, redaction) => redaction.Redact(str, httpContext)),

                Dictionary<string, string> dictionaryData =>
                    Configuration.Redactions.Aggregate(dictionaryData,
                        (dictionary, redaction) => redaction.Redact(dictionary, httpContext)),

                _ => logData
            };
        }

        return logData;
    }
}