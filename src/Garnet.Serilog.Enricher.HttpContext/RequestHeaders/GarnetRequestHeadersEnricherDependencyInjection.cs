using Garnet.Serilog.Enricher.HttpContext.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Garnet.Serilog.Enricher.HttpContext.RequestHeaders;

/// <summary>
/// Bunch of methods to register and use <see cref="GarnetRequestHeadersEnricher"/>
/// </summary>
public static class GarnetRequestHeadersEnricherDependencyInjection
{
    /// <summary>
    /// Register <see cref="GarnetRequestHeadersEnricher"/> requirements to the service collection
    /// </summary>
    /// <param name="serviceCollection">To register requirements to</param>
    /// <param name="configuration">Configuration and limitations for enrichment</param>
    /// <param name="propertyNameConfig">Configuration used for log event property name. Using default value if pass null</param>
    /// <returns><paramref name="serviceCollection"/> after applying the configurations</returns>
    public static IServiceCollection AddGarnetRequestHeadersEnricher(this IServiceCollection serviceCollection,
        GarnetHttpContextEnricherPropertyNameConfig propertyNameConfig = null,
        GarnetHttpContextEnrichmentConfiguration configuration = null)
    {
        return serviceCollection.AddGarnetHttpContextEnricher<GarnetRequestHeadersEnricher>(propertyNameConfig,
            configuration);
    }
}