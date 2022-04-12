namespace Garnet.Serilog.Enricher.HttpContext;

/// <summary>
/// LogEvent property name config used across Garnet HttpContext enrichers
/// </summary>
public class GarnetHttpContextEnricherPropertyNameConfig
{
    /// <summary>
    /// Request Body with Default value of "RequestBody"
    /// </summary>
    public string RequestBody { get; set; } = "RequestBody";
    
    /// <summary>
    /// Response Body with Default value of "ResponseBody"
    /// </summary>
    public string ResponseBody { get; set; } = "ResponseBody";
    
    /// <summary>
    /// Request Headers with Default value of "RequestHeaders"
    /// </summary>
    public string RequestHeaders { get; set; } = "RequestHeaders";
    
    /// <summary>
    /// Response Headers with Default value of "ResponseHeaders"
    /// </summary>
    public string ResponseHeaders { get; set; } = "ResponseHeaders";
    
    /// <summary>
    /// User Claims with Default value of "UserClaims"
    /// </summary>
    public string UserClaims { get; set; } = "UserClaims";

    /// <summary>
    /// Request Url with Default value of "RequestUrl"
    /// </summary>
    public string RequestUrl { get; set; } = "RequestUrl";
}
