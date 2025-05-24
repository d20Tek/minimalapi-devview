# MinimalApi-DevView

## Introduction
**MinimalApi.DevView** is a lightweight, zero-config diagnostic toolkit for **.NET Minimal API** projects. Designed for **development-time use**, it provides runtime insights into your app’s metadata, routes, and requests — all accessible via a simple `/dev` endpoint. It's like a plug-and-play *developer control panel* for Minimal APIs.

### Features
- `/dev/info` – App metadata: environment, version, uptime, etc.
- `/dev/routes` – Lists all mapped endpoints and their handlers
- **Request/response logging** – Method, path, status code, and duration
- **Auto-disabled in production**
- Fully configurable base path and options in your MinimalApi app.config file

---

## Installation
Install via NuGet:

```cmd
PM > Install-Package D20Tek.MinimalApi.DevView
```

To install in the Visual Studio UI, go to the Tools menu > "Manage NuGet Packages". Then search for D20Tek.MinimalApi.DevView, and install whichever package version you require from there.

## Usage
In your Program.cs, enable MinimalApi.DevView only in development:

```csharp
using D20Tek.MinimalApi.DevView;
...

builder.Services.AddDevView(builder.Configuration);
...

if (app.Environment.IsDevelopment())
{
    app.UseDevView();
}
```
This automatically:
- Maps /dev/info and /dev/routes
- Adds middleware for request/response logging
- Reads configuration from DI and environment
- Will not leak internal diagnostics in production or staging environments unless explicitly configured

You can override the default settings via appsettings.Development.json in your MinimalApi project:
```json
{
  "DevView": {
    "BasePath": "/my-dev",
    "LogLevel": "Information"
  }
}
```

### Endpoints
With DevView added, your MinimalApi project will now have two additional routes when you run the service. These routes are available to help you understand your service Api.

/dev/info - returns app metadata:
```json
{
  "AppName": "MyApi",
  "Environment": "Development",
  "Version": "1.0.0",
  "StartTime": "2025-05-20T12:34:56Z",
  "UptimeSeconds": 132
}
```

/dev/routes - lists all mapped Minimal API endpoints:
```json
[
  {
    "Method": "GET",
    "Pattern": "/users/{id}",
    "Handler": "UserEndpoints.GetById",
    "Produces": [ "application/json" ]
  },
  {
    "Method": "POST",
    "Pattern": "/auth/login",
    "Handler": "AuthEndpoints.Login"
  }
]
```

/dev/deps - lists all services registered with the WebApi dependency injection container:
/dev/routes - lists all mapped Minimal API endpoints:
```json
[
  {
    "serviceType": "IHostingEnvironment",
    "implementation": "Microsoft.Extensions.Hosting.Internal.HostingEnvironment",
    "lifetime": "Singleton",
    "assemblyName": "Microsoft.Extensions.Hosting"
  },
  {
    "serviceType": "IHostEnvironment",
    "implementation": "Microsoft.Extensions.Hosting.Internal.HostingEnvironment",
    "lifetime": "Singleton",
    "assemblyName": "Microsoft.Extensions.Hosting"
  },
  {
    "serviceType": "HostBuilderContext",
    "implementation": "Microsoft.Extensions.Hosting.HostBuilderContext",
    "lifetime": "Singleton",
    "assemblyName": "Microsoft.Extensions.Hosting.Abstractions"
  },
  ...
]
```

### Logging
By default, DevView registers a request/response logger to help with debugging your service. The logging output looks like this:
```bash
--> GET /users/5
<-- 200 OK (112ms)
```

## Samples
For more detailed examples on how to use D20Tek.MinimalApi.DevView, please review the following samples:

* [Sample.WebApi](samples/Sample.WebApi) - A simple Minimal WepApi project that implements the necessary calls to MinimalApi.DevView. This service has endpoints for weather forecasts and task CRUD operations.

## Feedback
If you use this library and have any feedback, bugs, or suggestions, please file them in the Issues section of this repository. I'm still in the process of building the library and samples, so any suggestions that would make it more useable are welcome.
