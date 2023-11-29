using System;
using System.Collections.Generic;

namespace Garnet.Serilog.Enricher.HttpContext.Configuration;

/// <summary>
/// Enrichment configuration and limitation based on HttpContext
/// </summary>
public class GarnetHttpContextEnrichmentConfiguration
{
    /// <summary>
    /// RequestBody with size less than this value will be enriched. Default is 128KB
    /// </summary>
    public int RequestBodySizeLimitInBytes { get; set; } = 128 * 1024; // 128KB
    
    /// <summary>
    /// ResponseBody with size less than this value will be enriched. Default is 128KB
    /// </summary>
    public int ResponseBodySizeLimitInBytes { get; set; } = 128 * 1024; // 128KB
    
    /// <summary>
    /// Filter out Enrichment based on condition on HttpContext.
    /// </summary>
    public Func<Microsoft.AspNetCore.Http.HttpContext, bool> RequestFilter { get; set; }
    
    /// <summary>
    /// To redact fields in RequestBody or ResponseBody
    /// </summary>
    public List<Redaction.Redaction> Redactions { get; set; }
}