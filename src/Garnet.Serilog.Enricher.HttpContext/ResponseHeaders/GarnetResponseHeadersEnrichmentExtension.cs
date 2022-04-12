using System;
using Garnet.Serilog.Enricher.HttpContext.Exceptions;
using Serilog;
using Serilog.Configuration;

namespace Garnet.Serilog.Enricher.HttpContext.ResponseHeaders;

/// <summary>
/// Facilitate configuring Serilog to use <see cref="GarnetResponseHeadersEnricher"/>
/// </summary>
public static class GarnetResponseHeadersEnrichmentExtension
{
    /// <summary>
    /// Enrich LogEvent with <see cref="GarnetResponseHeadersEnricher"/>
    /// </summary>
    /// <param name="enrichmentConfiguration">Register <see cref="GarnetResponseHeadersEnricher"/> to Serilog to be used as an enricher</param>
    /// <param name="serviceProvider">Used to get registered <see cref="GarnetResponseHeadersEnricher"/> from DI</param>
    /// <returns><see cref="LoggerConfiguration"/> for the sake of method chaining</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="enrichmentConfiguration"/> is null</exception>
    /// <exception cref="GarnetEnricherNotRegisteredException">When <see cref="GarnetResponseHeadersEnricher"/> is not registered in <paramref name="serviceProvider"/></exception>
    public static LoggerConfiguration WithGarnetResponseHeaders(this LoggerEnrichmentConfiguration enrichmentConfiguration,
        IServiceProvider serviceProvider)
    {
        return enrichmentConfiguration.WithGarnetEnricher<GarnetResponseHeadersEnricher>(serviceProvider);
    }
}