using System;
using Garnet.Serilog.Enricher.HttpContext.Exceptions;
using Serilog;
using Serilog.Configuration;

namespace Garnet.Serilog.Enricher.HttpContext.RequestBody;

/// <summary>
/// Facilitate configuring Serilog to use <see cref="GarnetRequestBodyEnricher"/>
/// </summary>
public static class GarnetRequestBodyEnrichmentExtension
{
    /// <summary>
    /// Enrich LogEvent with <see cref="GarnetRequestBodyEnricher"/>
    /// </summary>
    /// <param name="enrichmentConfiguration">Register <see cref="GarnetRequestBodyEnricher"/> to Serilog to be used as an enricher</param>
    /// <param name="serviceProvider">Used to get registered <see cref="GarnetRequestBodyEnricher"/> from DI</param>
    /// <returns><see cref="LoggerConfiguration"/> for the sake of method chaining</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="enrichmentConfiguration"/> is null</exception>
    /// <exception cref="GarnetEnricherNotRegisteredException">When <see cref="GarnetRequestBodyEnricher"/> is not registered in <paramref name="serviceProvider"/></exception>
    public static LoggerConfiguration WithGarnetRequestBody(this LoggerEnrichmentConfiguration enrichmentConfiguration,
        IServiceProvider serviceProvider)
    {
        return enrichmentConfiguration.WithGarnetEnricher<GarnetRequestBodyEnricher>(serviceProvider);
    }
}