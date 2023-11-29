using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Garnet.Serilog.Enricher.HttpContext;

/// <summary>
/// To capture logs from <see cref="Microsoft.AspNetCore.HttpLogging.HttpLoggingMiddleware"/> to enrich
/// </summary>
/// <typeparam name="T">Type of logger that should be <see cref="Microsoft.AspNetCore.HttpLogging.HttpLoggingMiddleware"/></typeparam>
internal class GarnetHttpLoggingSink<T> : ILogger<T>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    internal const string RequestBodyCacheKey = "GARNET.SERILOG.REQUESTBODY.ENRICHER.MIDDLEWARE.CACHE";
    internal const string ResponseBodyCacheKey = "GARNET.SERILOG.RESPONSEBODY.ENRICHER.MIDDLEWARE.CACHE";

    public GarnetHttpLoggingSink(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        throw new NotImplementedException();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    /// <summary>
    /// Store logs in <see cref="HttpContext.Items"/> to enrich logs
    /// </summary>
    /// <param name="logLevel"></param>
    /// <param name="eventId"></param>
    /// <param name="state"></param>
    /// <param name="exception"></param>
    /// <param name="formatter"></param>
    /// <typeparam name="TState"></typeparam>
    void ILogger.Log<TState>(LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        var httpContext = _httpContextAccessor?.HttpContext;
        if (httpContext is null || eventId.Id is not (3 or 4))
        {
            return;
        }

        var body = formatter(state, exception).Remove(0, 13).Trim();

        switch (eventId.Id)
        {
            case 3: //RequestBody
                httpContext.Items.TryAdd(RequestBodyCacheKey, body);
                break;
            
            case 4: //ResponseBody
                httpContext.Items.TryAdd(ResponseBodyCacheKey, body);
                break;
        }
    }
}