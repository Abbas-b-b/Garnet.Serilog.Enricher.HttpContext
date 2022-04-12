using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Garnet.Serilog.Enricher.HttpContext.ResponseHeaders;

/// <summary>
/// Bunch of methods to register and use <see cref="GarnetResponseHeadersEnricher"/>
/// </summary>
public static class GarnetResponseHeadersEnricherDependencyInjection
{
    /// <summary>
    /// Register <see cref="GarnetResponseHeadersEnricher"/> requirements to the service collection
    /// </summary>
    /// <param name="serviceCollection">To register requirements to</param>
    /// <param name="configuration">To load <see cref="GarnetHttpContextEnricherPropertyNameConfig"/> with <paramref name="configurationPath"/></param>
    /// <param name="configurationPath">Path to load <see cref="GarnetHttpContextEnricherPropertyNameConfig"/> from <paramref name="configuration"/></param>
    /// <returns><paramref name="serviceCollection"/> after applying the configurations</returns>
    public static IServiceCollection AddGarnetResponseHeadersEnricher(this IServiceCollection serviceCollection,
        IConfiguration configuration,
        string configurationPath = "Garnet.Serilog.Enricher.HttpContext")
    {
        return serviceCollection.AddGarnetHttpContextEnricher<GarnetResponseHeadersEnricher>(configuration,
            configurationPath);
    }

    /// <summary>
    /// Register <see cref="GarnetResponseHeadersEnricher"/> requirements to the service collection
    /// </summary>
    /// <param name="serviceCollection">To register requirements to</param>
    /// <param name="propertyNameConfig">Configuration used for log event property name. Using default value if pass null</param>
    /// <returns><paramref name="serviceCollection"/> after applying the configurations</returns>
    public static IServiceCollection AddGarnetResponseHeadersEnricher(this IServiceCollection serviceCollection,
        GarnetHttpContextEnricherPropertyNameConfig propertyNameConfig = null)
    {
        return serviceCollection.AddGarnetHttpContextEnricher<GarnetResponseHeadersEnricher>(propertyNameConfig);
    }
}