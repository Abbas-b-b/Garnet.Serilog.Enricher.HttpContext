using System;
using Garnet.Serilog.Enricher.HttpContext.Exceptions;
using Garnet.Serilog.Enricher.HttpContext.RequestBody;
using Garnet.Serilog.Enricher.HttpContext.RequestHeaders;
using Garnet.Serilog.Enricher.HttpContext.RequestUrl;
using Garnet.Serilog.Enricher.HttpContext.ResponseBody;
using Garnet.Serilog.Enricher.HttpContext.ResponseHeaders;
using Garnet.Serilog.Enricher.HttpContext.UserClaims;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Configuration;

namespace Garnet.Serilog.Enricher.HttpContext;

/// <summary>
/// Facilitate configuring Serilog to use Garnet HttpContext enrichers
/// </summary>
public static class GarnetHttpContextEnrichmentExtensions
{
    /// <summary>
    /// Enrich LogEvent with all Garnet HttpContext enrichers
    /// </summary>
    /// <param name="enrichmentConfiguration">Register <see cref="GarnetResponseBodyEnricher"/> to Serilog to be used as an enricher</param>
    /// <param name="serviceProvider">Used to get registered <see cref="GarnetResponseBodyEnricher"/> from DI</param>
    /// <returns><see cref="LoggerConfiguration"/> for the sake of method chaining</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="enrichmentConfiguration"/> is null</exception>
    /// <exception cref="GarnetEnricherNotRegisteredException">When any of the Garnet HttpContext enrichers is not registered in <paramref name="serviceProvider"/></exception>
    public static LoggerConfiguration WithAllGarnetHttpContextEnrichers(
        this LoggerEnrichmentConfiguration enrichmentConfiguration,
        IServiceProvider serviceProvider)
    {
        return enrichmentConfiguration
            .WithGarnetRequestBody(serviceProvider)
            .Enrich.WithGarnetRequestHeaders(serviceProvider)
            .Enrich.WithGarnetRequestUrl(serviceProvider)
            .Enrich.WithGarnetResponseBody(serviceProvider)
            .Enrich.WithGarnetResponseHeaders(serviceProvider)
            .Enrich.WithGarnetUserClaims(serviceProvider);
    }

    /// <summary>
    /// Enrich LogEvent with a Garnet HttpContext enricher of type <typeparamref name="TEnricher"/>
    /// </summary>
    /// <param name="enrichmentConfiguration">Register <see cref="GarnetResponseBodyEnricher"/> to Serilog to be used as an enricher</param>
    /// <param name="serviceProvider">Used to get registered <see cref="GarnetResponseBodyEnricher"/> from DI</param>
    /// <typeparam name="TEnricher">Type of Garnet HttpContext enricher</typeparam>
    /// <returns><see cref="LoggerConfiguration"/> for the sake of method chaining</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="enrichmentConfiguration"/> is null</exception>
    /// <exception cref="GarnetEnricherNotRegisteredException">When <typeparamref name="TEnricher"/> is not registered in <paramref name="serviceProvider"/></exception>
    internal static LoggerConfiguration WithGarnetEnricher<TEnricher>(
        this LoggerEnrichmentConfiguration enrichmentConfiguration,
        IServiceProvider serviceProvider) where TEnricher : GarnetHttpContextEnricherBase
    {
        if (enrichmentConfiguration == null)
        {
            throw new ArgumentNullException(nameof(enrichmentConfiguration));
        }

        if (serviceProvider is null)
        {
            throw new ArgumentNullException(nameof(serviceProvider));
        }

        var enricher = serviceProvider.GetService<TEnricher>();

        if (enricher is null)
        {
            throw new GarnetEnricherNotRegisteredException(typeof(TEnricher).Name);
        }

        return enrichmentConfiguration.With(enricher);
    }
}