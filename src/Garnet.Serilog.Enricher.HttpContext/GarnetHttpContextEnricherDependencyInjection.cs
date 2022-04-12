using Garnet.Serilog.Enricher.HttpContext.RequestBody;
using Garnet.Serilog.Enricher.HttpContext.RequestHeaders;
using Garnet.Serilog.Enricher.HttpContext.RequestUrl;
using Garnet.Serilog.Enricher.HttpContext.ResponseBody;
using Garnet.Serilog.Enricher.HttpContext.ResponseHeaders;
using Garnet.Serilog.Enricher.HttpContext.UserClaims;
using Microsoft.Extensions.Configuration;
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
    /// <param name="configuration">To load <see cref="GarnetHttpContextEnricherPropertyNameConfig"/> with <paramref name="configurationPath"/></param>
    /// <param name="configurationPath">Path to load <see cref="GarnetHttpContextEnricherPropertyNameConfig"/> from <paramref name="configuration"/></param>
    /// <returns><paramref name="serviceCollection"/> after applying the configurations</returns>
    public static IServiceCollection AddAllGarnetHttpContextEnrichers(this IServiceCollection serviceCollection,
        IConfiguration configuration,
        string configurationPath = "Garnet.Serilog.Enricher.HttpContext")
    {
        return serviceCollection
            .AddGarnetRequestBodyEnricher(configuration, configurationPath)
            .AddGarnetRequestHeadersEnricher(configuration, configurationPath)
            .AddGarnetRequestUrlEnricher(configuration, configurationPath)
            .AddGarnetResponseBodyEnricher(configuration, configurationPath)
            .AddGarnetResponseHeadersEnricher(configuration, configurationPath)
            .AddGarnetUserClaimsEnricher(configuration, configurationPath);
    }

    /// <summary>
    /// Register all Garnet HttpContext enrichers requirements to the service collection
    /// </summary>
    /// <param name="serviceCollection">To register requirements to</param>
    /// <param name="propertyNameConfig">Configuration used for log event property name. Using default value if pass null</param>
    /// <returns><paramref name="serviceCollection"/> after applying the configurations</returns>
    public static IServiceCollection AddAllGarnetHttpContextEnrichers(this IServiceCollection serviceCollection,
        GarnetHttpContextEnricherPropertyNameConfig propertyNameConfig = null)
    {
        return serviceCollection
            .AddGarnetRequestBodyEnricher(propertyNameConfig)
            .AddGarnetRequestHeadersEnricher(propertyNameConfig)
            .AddGarnetRequestUrlEnricher(propertyNameConfig)
            .AddGarnetResponseBodyEnricher(propertyNameConfig)
            .AddGarnetResponseHeadersEnricher(propertyNameConfig)
            .AddGarnetUserClaimsEnricher(propertyNameConfig);
    }

    /// <summary>
    /// Boilerplate code for registering Garnet HttpContext enrichers requirements to the service collection
    /// </summary>
    /// <param name="serviceCollection">To register requirements to</param>
    /// <param name="configuration">To load <see cref="GarnetHttpContextEnricherPropertyNameConfig"/> with <paramref name="configurationPath"/></param>
    /// <param name="configurationPath">Path to load <see cref="GarnetHttpContextEnricherPropertyNameConfig"/> from <paramref name="configuration"/></param>
    /// <typeparam name="TEnricher">Type of Garnet HttpContext enricher to register</typeparam>
    /// <returns><paramref name="serviceCollection"/> after applying the configurations</returns>
    internal static IServiceCollection AddGarnetHttpContextEnricher<TEnricher>(this IServiceCollection serviceCollection,
        IConfiguration configuration,
        string configurationPath = "Garnet.Serilog.Enricher.HttpContext")
        where TEnricher : GarnetHttpContextEnricherBase
    {
        var propertyNameConfig =
            configuration.GetValue<GarnetHttpContextEnricherPropertyNameConfig>($"{configurationPath}");

        return serviceCollection.AddGarnetHttpContextEnricher<TEnricher>(propertyNameConfig);
    }

    /// <summary>
    /// Boilerplate code for registering Garnet HttpContext enrichers requirements to the service collection
    /// </summary>
    /// <param name="serviceCollection">To register requirements to</param>
    /// <param name="propertyNameConfig">Configuration used for log event property name. Using default value if pass null</param>
    /// <typeparam name="TEnricher">Type of Garnet HttpContext enricher to register</typeparam>
    /// <returns><paramref name="serviceCollection"/> after applying the configurations</returns>
    internal static IServiceCollection AddGarnetHttpContextEnricher<TEnricher>(this IServiceCollection serviceCollection,
        GarnetHttpContextEnricherPropertyNameConfig propertyNameConfig = null)
        where TEnricher : GarnetHttpContextEnricherBase
    {
        serviceCollection.AddSingleton(propertyNameConfig ?? new GarnetHttpContextEnricherPropertyNameConfig());
        serviceCollection.AddSingleton<TEnricher>();

        return serviceCollection;
    }
}