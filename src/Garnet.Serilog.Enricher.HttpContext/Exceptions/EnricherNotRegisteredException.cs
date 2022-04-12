using System;

namespace Garnet.Serilog.Enricher.HttpContext.Exceptions;

/// <summary>
/// Exception when the enricher could not be found in the DI container
/// </summary>
public class GarnetEnricherNotRegisteredException : Exception
{
    /// <summary>
    /// The <paramref name="enricherType"/> could not be found in the DI container
    /// </summary>
    /// <param name="enricherType">Type of the enricher trying to find in the DI container</param>
    public GarnetEnricherNotRegisteredException(string enricherType)
        : base($"Enricher of type {enricherType} not found in DI")
    {
    }
}