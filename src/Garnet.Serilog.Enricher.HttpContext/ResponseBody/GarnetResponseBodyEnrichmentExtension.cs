using System;
using Garnet.Serilog.Enricher.HttpContext.Exceptions;
using Serilog;
using Serilog.Configuration;

namespace Garnet.Serilog.Enricher.HttpContext.ResponseBody;

/// <summary>
/// Facilitate configuring Serilog to use <see cref="GarnetResponseBodyEnricher"/>
/// </summary>
public static class GarnetResponseBodyEnrichmentExtension
{
    /// <summary>
    /// Enrich LogEvent with <see cref="GarnetResponseBodyEnricher"/>
    /// </summary>
    /// <param name="enrichmentConfiguration">Register <see cref="GarnetResponseBodyEnricher"/> to Serilog to be used as an enricher</param>
    /// <param name="serviceProvider">Used to get registered <see cref="GarnetResponseBodyEnricher"/> from DI</param>
    /// <returns><see cref="LoggerConfiguration"/> for the sake of method chaining</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="enrichmentConfiguration"/> is null</exception>
    /// <exception cref="GarnetEnricherNotRegisteredException">When <see cref="GarnetResponseBodyEnricher"/> is not registered in <paramref name="serviceProvider"/></exception>
    public static LoggerConfiguration WithGarnetResponseBody(this LoggerEnrichmentConfiguration enrichmentConfiguration,
        IServiceProvider serviceProvider)
    {
        return enrichmentConfiguration.WithGarnetEnricher<GarnetResponseBodyEnricher>(serviceProvider);
    }
}