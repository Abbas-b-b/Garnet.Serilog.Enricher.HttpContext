using Garnet.Serilog.Enricher.HttpContext.Configuration;
using Garnet.Serilog.Enricher.HttpContext.RequestBody;
using Garnet.Serilog.Enricher.HttpContext.RequestHeaders;
using Garnet.Serilog.Enricher.HttpContext.RequestUrl;
using Garnet.Serilog.Enricher.HttpContext.ResponseBody;
using Garnet.Serilog.Enricher.HttpContext.ResponseHeaders;
using Garnet.Serilog.Enricher.HttpContext.UserClaims;
using Microsoft.Extensions.DependencyInjection;

namespace Garnet.Serilog.Enricher.HttpContext;

/// <summary>
/// Bunch of methods to register and use Garnet HttpContext enrichers
/// </summary>
public static class GarnetHttpContextEnricherDependencyInjection
{
    /// <summary>
    /// Register all Garnet HttpContext enrichers requirements to the service collection
    /// </summary>
    /// <param name="serviceCollection">To register requirements to</param>
    /// <param name="propertyNameConfig">Configuration used for log event property name. Using default value if pass null</param>
    /// <param name="configuration">Configuration and limitations for enrichment</param>
    /// <returns><paramref name="serviceCollection"/> after applying the configurations</returns>
    public static IServiceCollection AddAllGarnetHttpContextEnrichers(this IServiceCollection serviceCollection,
        GarnetHttpContextEnricherPropertyNameConfig propertyNameConfig = null,
        GarnetHttpContextEnrichmentConfiguration configuration = null)
    {
        configuration ??= new GarnetHttpContextEnrichmentConfiguration();
        
        return serviceCollection
            .AddGarnetRequestBodyEnricher(propertyNameConfig, configuration)
            .AddGarnetRequestHeadersEnricher(propertyNameConfig, configuration)
            .AddGarnetRequestUrlEnricher(propertyNameConfig, configuration)
            .AddGarnetResponseBodyEnricher(propertyNameConfig, configuration)
            .AddGarnetResponseHeadersEnricher(propertyNameConfig, configuration)
            .AddGarnetUserClaimsEnricher(propertyNameConfig, configuration);
    }

    /// <summary>
    /// Boilerplate code for registering Garnet HttpContext enrichers requirements to the service collection
    /// </summary>
    /// <param name="serviceCollection">To register requirements to</param>
    /// <param name="propertyNameConfig">Configuration used for log event property name. Using default value if pass null</param>
    /// <param name="configuration">Configuration and limitations for enrichment</param>
    /// <typeparam name="TEnricher">Type of Garnet HttpContext enricher to register</typeparam>
    /// <returns><paramref name="serviceCollection"/> after applying the configurations</returns>
    internal static IServiceCollection AddGarnetHttpContextEnricher<TEnricher>(this IServiceCollection serviceCollection,
        GarnetHttpContextEnricherPropertyNameConfig propertyNameConfig,
        GarnetHttpContextEnrichmentConfiguration configuration = null)
        where TEnricher : GarnetHttpContextEnricherBase
    {
        GarnetConfigProvider.AddConfiguration<TEnricher>(configuration ?? new GarnetHttpContextEnrichmentConfiguration());
        serviceCollection.AddSingleton(propertyNameConfig ?? new GarnetHttpContextEnricherPropertyNameConfig());
        serviceCollection.AddSingleton<TEnricher>();

        return serviceCollection;
    }
}