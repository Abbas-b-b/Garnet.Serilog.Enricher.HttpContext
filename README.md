# Garnet Serilog HttpContext Enricher [![Nuget](https://img.shields.io/nuget/vpre/Garnet.Serilog.Enricher.HttpContext?style=for-the-badge)](https://www.nuget.org/packages/Garnet.Serilog.Enricher.HttpContext/) [![Nuget](https://img.shields.io/nuget/dt/Garnet.Serilog.Enricher.HttpContext?style=for-the-badge)](https://www.nuget.org/packages/Garnet.Serilog.Enricher.HttpContext/)

Serilog comprehensive HttpContext enrichers like request body, response body, request and response headers, user claims, ... with filter and redaction support

---

## Installation

    dotnet add package Garnet.Serilog.Enricher.HttpContext

---

## List of Enrichers

* Request Body
* Request Headers
* Request Url
* Response Body
* Response Headers
* User Claims
* Custom Property<small>*: Enrich with any information from HttpContext*</small>
* Custom Property With Cache<small>*: Like Custom Property with caching the result*</small>

---

## Usage

First, enrichers and their requirements need to be registered

```C#
services.AddAllGarnetHttpContextEnrichers() //Add all enrichers with default configuration

// ---OR---
//You can add each individual enricher
services.AddGarnetRequestBodyEnricher();
services.AddGarnetRequestHeadersEnricher();
services.AddGarnetRequestUrlEnricher();
services.AddGarnetResponseBodyEnricher();
services.AddGarnetResponseHeadersEnricher();
services.AddGarnetUserClaimsEnricher();
```
Other overloads of these methods can be used to set configuration or read configuration from ```appsettings```.

**For RequestBody and ResponseBody enrichers to work you need to use their ASP middlewares**

```C#
app //IApplicantBuilder
  .UseGarnetRequestBodyEnricherMiddleware()
  .UseGarnetResponseBodyEnricherMiddleware()
  ...;
```
**The order of using middlewares is important to have proper log information. 
For example if you have a middleware that produces custom responses on exception occurrence and you want to log the exact response,
you should register the GarnetResponseBody middleware before that. [More information](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/)**

Now let ```Serilog``` to use Garnet enrichers all at once or one by one as you need.

```C#
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .Enrich.WithAllGarnetHttpContextEnrichers(serviceProvider) // <---
            // ---OR---
            .Enrich.WithGarnetRequestBody(serviceProvider)
            .Enrich.WithGarnetRequestHeaders(serviceProvider)
            .Enrich.WithGarnetRequestUrl(serviceProvider)
            .Enrich.WithGarnetResponseBody(serviceProvider)
            .Enrich.WithGarnetResponseHeaders(serviceProvider)
            .Enrich.WithGarnetUserClaims(serviceProvider);
            // --------
            .WriteTo.Console())
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
```

You can use custom enrichers to add more information to ```LogEvent``` from ```HttpContext```

```C#
...
.Enrich.With(new GarnetHttpContextCustomPropertyEnricher("ResponseStatusCode", 
    serviceProvider,
    httpContext => httpContext.Response.StatusCode)) //<--- provide result
...;

// ---OR---

// The following custom enricher caches the provided result for further events in a same instanse of the HttpContext (in a same incoming request)
...
.Enrich.With(new GarnetHttpContextCustomPropertyEnricherWithCache("RequestMethod", 
    serviceProvider,
    httpContext => httpContext.Request.Method)); //<--- provide result
...;
```

## Redaction and Filter
Redaction can be applied to the Request or Response content for request to predefined URLs or all. For now it only redact fields of content in `Json` or `form-urlencoded` format.

Every enrichment can be filtered for every request by using `HttpContext`.

```C#
serviceCollection.AddAllGarnetHttpContextEnrichers(configuration: new GarnetHttpContextEnrichmentConfiguration
{
    //Ignore enrichment on request to swagger
    RequestFilter = context =>
        !context.Request.Path.Value.Contains("swagger", StringComparison.InvariantCultureIgnoreCase),
        
    //Redact password field on login
    Redactions = new List<Redaction>
    {
        new()
        {
            Url = "connect/token",
            Fields = new List<string> { "password" },
        }
    }
});
```