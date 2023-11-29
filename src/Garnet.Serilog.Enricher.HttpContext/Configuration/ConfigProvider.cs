using System;
using System.Collections.Generic;

namespace Garnet.Serilog.Enricher.HttpContext.Configuration;

/// <summary>
/// Stores and retrieve configuration related to each Enricher 
/// </summary>
internal static class GarnetConfigProvider
{
    private static readonly Dictionary<Type, GarnetHttpContextEnrichmentConfiguration> Configs = new();

    public static void AddConfiguration<TEnricher>(GarnetHttpContextEnrichmentConfiguration config)
    {
        var type = typeof(TEnricher);

        if (Configs.ContainsKey(type))
        {
            Configs[type] = config;
        }
        
        Configs.Add(type, config);
    }

    public static GarnetHttpContextEnrichmentConfiguration GetConfiguration(GarnetHttpContextEnricherBase enricher)
    {
        var type = enricher.GetType();

        if (Configs.TryGetValue(type, out var config))
        {
            return config;
        }

        return new GarnetHttpContextEnrichmentConfiguration();
    }
}