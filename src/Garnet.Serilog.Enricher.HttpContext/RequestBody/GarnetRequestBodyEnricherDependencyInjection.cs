using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Garnet.Serilog.Enricher.HttpContext.RequestBody;

/// <summary>
/// Bunch of methods to register and use <see cref="GarnetRequestBodyEnricher"/>
/// </summary>
public static class GarnetRequestBodyEnricherDependencyInjection
{
    /// <summary>
    /// Register <see cref="GarnetRequestBodyEnricher"/> requirements to the service collection
    /// </summary>
    /// <param name="serviceCollection">To register requirements to</param>
    /// <param name="configuration">To load <see cref="GarnetHttpContextEnricherPropertyNameConfig"/> with <paramref name="configurationPath"/></param>
    /// <param name="configurationPath">Path to load <see cref="GarnetHttpContextEnricherPropertyNameConfig"/> from <paramref name="configuration"/></param>
    /// <returns><paramref name="serviceCollection"/> after applying the configurations</returns>
    public static IServiceCollection AddGarnetRequestBodyEnricher(this IServiceCollection serviceCollection,
        IConfiguration configuration,
        string configurationPath = "Garnet.Serilog.Enricher.HttpContext")
    {
        return serviceCollection.AddGarnetHttpContextEnricher<GarnetRequestBodyEnricher>(configuration,
            configurationPath);
    }

    /// <summary>
    /// Register <see cref="GarnetRequestBodyEnricher"/> requirements to the service collection
    /// </summary>
    /// <param name="serviceCollection">To register requirements to</param>
    /// <param name="propertyNameConfig">Configuration used for log event property name. Using default value if pass null</param>
    /// <returns><paramref name="serviceCollection"/> after applying the configurations</returns>
    public static IServiceCollection AddGarnetRequestBodyEnricher(this IServiceCollection serviceCollection,
        GarnetHttpContextEnricherPropertyNameConfig propertyNameConfig = null)
    {
        return serviceCollection.AddGarnetHttpContextEnricher<GarnetRequestBodyEnricher>(propertyNameConfig);
    }
    
    /// <summary>
    /// Use a middleware to capture request body to enrich event log
    /// Pay a close attention to the order of the middlewares
    /// </summary>
    /// <param name="applicationBuilder">Used to register request body enricher middleware</param>
    /// <returns><paramref name="applicationBuilder"/> after registering the middleware</returns>
    public static IApplicationBuilder UseGarnetRequestBodyEnricherMiddleware(
        this IApplicationBuilder applicationBuilder)
    {
        return applicationBuilder.UseMiddleware<GarnetRequestBodyEnricherMiddleware>();
    }
}