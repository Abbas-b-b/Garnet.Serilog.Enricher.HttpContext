using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http.Extensions;

namespace Garnet.Serilog.Enricher.HttpContext.Redaction;

/// <summary>
/// Configure redaction fields by URL filter
/// This only redacts fields content in json or form-encoded format
/// </summary>
public class Redaction
{
    /// <summary>
    /// Request URL to apply this redaction. Use null or empty URL to apply to all requests
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Name of fields to redact
    /// </summary>
    public List<string> Fields { get; set; }

    /// <summary>
    /// The value to replace with actual value for redaction. Default is '***REDACTED***'
    /// </summary>
    public string ReplacementValue { get; set; } = "***REDACTED***";
    
    /// <summary>
    /// Redact <see cref="Fields"/> on <paramref name="content"/>
    /// </summary>
    /// <param name="content">Content to redact <see cref="Fields"/></param>
    /// <param name="httpContext">To verify <see cref="Url"/> against</param>
    /// <returns><param name="content"> after redaction</param></returns>
    internal string Redact(string content, Microsoft.AspNetCore.Http.HttpContext httpContext)
    {
        if (Fields == null
            || Fields.Count == 0
            || string.IsNullOrWhiteSpace(content)
            || !(string.IsNullOrWhiteSpace(Url)
                 || httpContext.Request.GetDisplayUrl().Contains(Url, StringComparison.InvariantCultureIgnoreCase)))
        {
            return content;
        }

        var fields = string.Join("|", Fields);

        var pattern = $"({fields})(\"\\s*:\\s*\"(.*?[^\\\\])\"\\s*[,}}\\]]|=([^&]*?)($|&))";
        var matches = Regex.Matches(content, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);

        foreach (Match match in matches)
        {
            var valueInJson = match.Groups[3].Value;
            var valueInFormEncoded = match.Groups[4].Value;
            
            if (!string.IsNullOrEmpty(valueInJson))
            {
                content = content.Replace(valueInJson, ReplacementValue);
            }
            
            if (!string.IsNullOrEmpty(valueInFormEncoded))
            {
                content = content.Replace(valueInFormEncoded, ReplacementValue);
            }
        }

        return content;
    }
}