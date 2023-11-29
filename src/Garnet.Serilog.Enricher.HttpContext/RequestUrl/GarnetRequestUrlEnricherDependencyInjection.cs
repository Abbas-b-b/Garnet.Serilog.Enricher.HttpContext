using Garnet.Serilog.Enricher.HttpContext.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Garnet.Serilog.Enricher.HttpContext.RequestUrl;

/// <summary>
/// Bunch of methods to register and use <see cref="GarnetRequestUrlEnricher"/>
/// </summary>
public static class GarnetRequestUrlEnricherDependencyInjection
{
    /// <summary>
    /// Register <see cref="GarnetRequestUrlEnricher"/> requirements to the service collection
    /// </summary>
    /// <param name="serviceCollection">To register requirements to</param>
    /// <param name="propertyNameConfig">Configuration used for log event property name. Using default value if pass null</param>
    /// <param name="configuration">Configuration and limitations for enrichment</param>
    /// <returns><paramref name="serviceCollection"/> after applying the configurations</returns>
    public static IServiceCollection AddGarnetRequestUrlEnricher(this IServiceCollection serviceCollection,
        GarnetHttpContextEnricherPropertyNameConfig propertyNameConfig = null,
        GarnetHttpContextEnrichmentConfiguration configuration = null)
    {
        return serviceCollection.AddGarnetHttpContextEnricher<GarnetRequestUrlEnricher>(propertyNameConfig,
            configuration);
    }
}