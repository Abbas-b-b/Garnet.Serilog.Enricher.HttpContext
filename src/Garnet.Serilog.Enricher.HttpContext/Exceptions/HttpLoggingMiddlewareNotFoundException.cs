using System;

namespace Garnet.Serilog.Enricher.HttpContext.Exceptions;

/// <summary>
/// Exception for missing Microsoft.AspNetCore.HttpLogging.HttpLoggingMiddleware reference
/// </summary>
public class HttpLoggingMiddlewareNotFoundException : Exception
{
    /// <summary>
    /// Exception for missing Microsoft.AspNetCore.HttpLogging.HttpLoggingMiddleware reference
    /// </summary>
    public HttpLoggingMiddlewareNotFoundException()
        : base("Type Microsoft.AspNetCore.HttpLogging.HttpLoggingMiddleware not found")
    {
    }
}