using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Garnet.Serilog.Enricher.HttpContext.UserClaims;

/// <summary>
/// Enrich LogEvent with user claims if present.
/// </summary>
public class GarnetUserClaimsEnricher : GarnetHttpContextEnricherBaseWithCache
{
    /// <summary>
    /// Enrich LogEvent with user claims if present.
    /// </summary>
    /// <param name="garnetHttpContextEnricherPropertyNameConfig">LogEvent property name configuration</param>
    /// <param name="httpContextAccessor">To access HttpContext to retrieve needed information</param>
    public GarnetUserClaimsEnricher(
        GarnetHttpContextEnricherPropertyNameConfig garnetHttpContextEnricherPropertyNameConfig,
        IHttpContextAccessor httpContextAccessor)
        : base(garnetHttpContextEnricherPropertyNameConfig.UserClaims, httpContextAccessor)
    {
    }

    /// <summary>
    /// Provide user claims or null if not exist
    /// </summary>
    /// <param name="httpContext">Current request context</param>
    /// <returns>Response headers or null if request has no headers</returns>
    protected override object ProvideLogObject(Microsoft.AspNetCore.Http.HttpContext httpContext)
    {
        if (httpContext.User?.Identity?.IsAuthenticated is null or false)
        {
            return null;
        }

        return httpContext.User.Claims
            .GroupBy(claim => claim.Type)
            .ToDictionary(claims => claims.Key,
                claims => claims
                    .Select(claim => claim.Value)
                    .Aggregate((claimValue1, claimValue2) => claimValue1 + ", " + claimValue2));
    }
}