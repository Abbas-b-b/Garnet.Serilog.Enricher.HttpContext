using Garnet.Serilog.Enricher.HttpContext.Configuration;
using Garnet.Serilog.Enricher.HttpContext.Exceptions;
using Garnet.Serilog.Enricher.HttpContext.ResponseBody;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Garnet.Serilog.Enricher.HttpContext.RequestBody;

/// <summary>
/// Bunch of methods to register and use <see cref="GarnetRequestBodyEnricher"/>
/// </summary>
public static class GarnetRequestBodyEnricherDependencyInjection
{
    internal static bool Added;
    internal static bool Used;

    /// <summary>
    /// Register <see cref="GarnetRequestBodyEnricher"/> requirements to the service collection
    /// </summary>
    /// <param name="serviceCollection">To register requirements to</param>
    /// <param name="propertyNameConfig">Configuration used for log event property name. Using default value if pass null</param>
    /// <param name="configuration">Configuration and limitations for enrichment</param>
    /// <returns><paramref name="serviceCollection"/> after applying the configurations</returns>
    public static IServiceCollection AddGarnetRequestBodyEnricher(this IServiceCollection serviceCollection,
        GarnetHttpContextEnricherPropertyNameConfig propertyNameConfig = null,
        GarnetHttpContextEnrichmentConfiguration configuration = null)
    {
        configuration ??= new GarnetHttpContextEnrichmentConfiguration();

        var loggingMiddleware = typeof(HttpLoggingOptions)
            .Assembly
            .GetType("Microsoft.AspNetCore.HttpLogging.HttpLoggingMiddleware");
        if (loggingMiddleware is null)
        {
            throw new HttpLoggingMiddlewareNotFoundException();
        }

        serviceCollection.AddHttpLogging(options =>
        {
            options.RequestBodyLogLimit = configuration.RequestBodySizeLimitInBytes;
            options.ResponseBodyLogLimit = configuration.ResponseBodySizeLimitInBytes;
            options.LoggingFields = GarnetResponseBodyEnricherDependencyInjection.Added
                ? HttpLoggingFields.RequestBody | HttpLoggingFields.ResponseBody
                : HttpLoggingFields.RequestBody;
        });


        var loggerService = typeof(ILogger<>).MakeGenericType(loggingMiddleware);
        var loggerImpl = typeof(GarnetHttpLoggingSink<>).MakeGenericType(loggingMiddleware);
        serviceCollection.AddSingleton(loggerService, loggerImpl);

        Added = true;

        return serviceCollection.AddGarnetHttpContextEnricher<GarnetRequestBodyEnricher>(propertyNameConfig,
            configuration);
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
        var requestBodyEnricher = applicationBuilder.ApplicationServices.GetService<GarnetRequestBodyEnricher>();
        var responseBodyEnricher = applicationBuilder.ApplicationServices.GetService<GarnetResponseBodyEnricher>();
        var requestBodyConfiguration = GarnetConfigProvider.GetConfiguration(requestBodyEnricher);
        var responseBodyConfiguration = GarnetConfigProvider.GetConfiguration(responseBodyEnricher);

        if (GarnetResponseBodyEnricherDependencyInjection.Used && responseBodyConfiguration == requestBodyConfiguration)
        {
            return Return(applicationBuilder);
        }

        if (requestBodyConfiguration.RequestFilter is not null)
        {
            applicationBuilder.UseWhen(requestBodyConfiguration.RequestFilter, builder => builder.UseHttpLogging());
        }
        else
        {
            return Return(applicationBuilder.UseHttpLogging());
        }

        return Return(applicationBuilder);

        IApplicationBuilder Return(IApplicationBuilder appBuilder)
        {
            Used = true;
            return appBuilder;
        }
    }
}