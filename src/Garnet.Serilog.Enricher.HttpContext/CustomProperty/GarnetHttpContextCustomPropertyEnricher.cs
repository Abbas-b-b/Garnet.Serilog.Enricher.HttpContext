using System;
using Microsoft.AspNetCore.Http;

namespace Garnet.Serilog.Enricher.HttpContext.CustomProperty;

/// <summary>
/// Use this to enrich LogEvent with any information from HttpContext.
/// This enricher does not cache LogObject
/// </summary>
public class GarnetHttpContextCustomPropertyEnricher : GarnetHttpContextEnricherBase
{
    private readonly Func<Microsoft.AspNetCore.Http.HttpContext, object> _provideLogObject;

    /// <summary>
    /// Use this to enrich LogEvent with any information from HttpContext.
    /// This enricher does not cache LogObject returned from <paramref name="logObjectProvider"/>,
    /// So <paramref name="logObjectProvider"/> get called every log event
    /// </summary>
    /// <param name="propertyKey">Log event property name</param>
    /// <param name="serviceProvider">To get <see cref="IHttpContextAccessor"/> from</param>
    /// <param name="logObjectProvider">A method to provide object to enrich LogEvent</param>
    public GarnetHttpContextCustomPropertyEnricher(string propertyKey,
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