using System;
using Garnet.Serilog.Enricher.HttpContext.Exceptions;
using Serilog;
using Serilog.Configuration;

namespace Garnet.Serilog.Enricher.HttpContext.RequestUrl;

/// <summary>
/// Facilitate configuring Serilog to use <see cref="GarnetRequestUrlEnricher"/>
/// </summary>
public static class GarnetRequestUrlEnrichmentExtension
{
    /// <summary>
    /// Enrich LogEvent with <see cref="GarnetRequestUrlEnricher"/>
    /// </summary>
    /// <param name="enrichmentConfiguration">Register <see cref="GarnetRequestUrlEnricher"/> to Serilog to be used as an enricher</param>
    /// <param name="serviceProvider">Used to get registered <see cref="GarnetRequestUrlEnricher"/> from DI</param>
    /// <returns><see cref="LoggerConfiguration"/> for the sake of method chaining</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="enrichmentConfiguration"/> is null</exception>
    /// <exception cref="GarnetEnricherNotRegisteredException">When <see cref="GarnetRequestUrlEnricher"/> is not registered in <paramref name="serviceProvider"/></exception>
    public static LoggerConfiguration WithGarnetRequestUrl(this LoggerEnrichmentConfiguration enrichmentConfiguration,
        IServiceProvider serviceProvider)
    {
        return enrichmentConfiguration.WithGarnetEnricher<GarnetRequestUrlEnricher>(serviceProvider);
    }
}