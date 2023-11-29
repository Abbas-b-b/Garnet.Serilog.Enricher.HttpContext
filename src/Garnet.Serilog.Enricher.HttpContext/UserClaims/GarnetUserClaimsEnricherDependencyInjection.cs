using Garnet.Serilog.Enricher.HttpContext.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Garnet.Serilog.Enricher.HttpContext.UserClaims;

/// <summary>
/// Bunch of methods to register and use <see cref="GarnetUserClaimsEnricher"/>
/// </summary>
public static class GarnetUserClaimsEnricherDependencyInjection
{
    /// <summary>
    /// Register <see cref="GarnetUserClaimsEnricher"/> requirements to the service collection
    /// </summary>
    /// <param name="serviceCollection">To register requirements to</param>
    /// <param name="propertyNameConfig">Configuration used for log event property name. Using default value if pass null</param>
    /// <param name="configuration">Configuration and limitations for enrichment</param>
    /// <returns><paramref name="serviceCollection"/> after applying the configurations</returns>
    public static IServiceCollection AddGarnetUserClaimsEnricher(this IServiceCollection serviceCollection,
        GarnetHttpContextEnricherPropertyNameConfig propertyNameConfig = null,
        GarnetHttpContextEnrichmentConfiguration configuration = null)
    {
        return serviceCollection.AddGarnetHttpContextEnricher<GarnetUserClaimsEnricher>(propertyNameConfig,
            configuration);
    }
}