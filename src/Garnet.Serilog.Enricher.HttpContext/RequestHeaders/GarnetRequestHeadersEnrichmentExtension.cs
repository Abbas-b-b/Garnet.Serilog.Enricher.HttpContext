using System;
using Garnet.Serilog.Enricher.HttpContext.Exceptions;
using Serilog;
using Serilog.Configuration;

namespace Garnet.Serilog.Enricher.HttpContext.RequestHeaders;

/// <summary>
/// Facilitate configuring Serilog to use <see cref="GarnetRequestHeadersEnricher"/>
/// </summary>
public static class GarnetRequestHeadersEnrichmentExtension
{
    /// <summary>
    /// Enrich LogEvent with <see cref="GarnetRequestHeadersEnricher"/>
    /// </summary>
    /// <param name="enrichmentConfiguration">Register <see cref="GarnetRequestHeadersEnricher"/> to Serilog to be used as an enricher</param>
    /// <param name="serviceProvider">Used to get registered <see cref="GarnetRequestHeadersEnricher"/> from DI</param>
    /// <returns><see cref="LoggerConfiguration"/> for the sake of method chaining</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="enrichmentConfiguration"/> is null</exception>
    /// <exception cref="GarnetEnricherNotRegisteredException">When <see cref="GarnetRequestHeadersEnricher"/> is not registered in <paramref name="serviceProvider"/></exception>
    public static LoggerConfiguration WithGarnetRequestHeaders(this LoggerEnrichmentConfiguration enrichmentConfiguration,
        IServiceProvider serviceProvider)
    {
        return enrichmentConfiguration.WithGarnetEnricher<GarnetRequestHeadersEnricher>(serviceProvider);
    }
}