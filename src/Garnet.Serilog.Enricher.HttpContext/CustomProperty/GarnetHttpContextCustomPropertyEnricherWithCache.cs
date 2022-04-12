using System;
using Microsoft.AspNetCore.Http;

namespace Garnet.Serilog.Enricher.HttpContext.CustomProperty;

/// <summary>
/// Use this to enrich LogEvent with any information from HttpContext.
/// This enricher do cache LogObject per HttpContext instance
/// </summary>
public class GarnetHttpContextCustomPropertyEnricherWithCache : GarnetHttpContextEnricherBaseWithCache
{
    private readonly Func<Microsoft.AspNetCore.Http.HttpContext, object> _provideLogObject;

    /// <summary>
    /// Use this to enrich LogEvent with any information from HttpContext.
    /// This enricher do cache LogObject returned from <paramref name="logObjectProvider"/> if it returns not null and not empty result.
    /// </summary>
    /// <param name="propertyKey">Log event property name</param>
    /// <param name="serviceProvider">To get <see cref="IHttpContextAccessor"/> from</param>
    /// <param name="logObjectProvider">A method to provide object to enrich LogEvent</param>
    public GarnetHttpContextCustomPropertyEnricherWithCache(string propertyKey,
        IServiceProvider serviceProvider,
        Func<Microsoft.AspNetCore.Http.HttpContext, object> logObjectProvider)
        : base(propertyKey, (IHttpContextAccessor) serviceProvider.GetService(typeof(IHttpContextAccessor)))
    {
        _provideLogObject = logObjectProvider;
    }

    /// <inheritdoc />
    protected override object ProvideLogObject(Microsoft.AspNetCore.Http.HttpContext httpContext)
    {
        return _provideLogObject(httpContext);
    }
}