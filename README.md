# .NET Aspire—Azure Key Vault Integration

.NET Aspire provides various [integrations](https://learn.microsoft.com/dotnet/aspire/fundamentals/integrations-overview) with prominent services and platforms, such as Redis and PostgreSQL. This repo represents a demo application that shows how to integrate with [Azure Key Vault](https://learn.microsoft.com/azure/key-vault/general/overview).

## Resource integration

These packages model various services, platforms, or capabilities such as caches, databases, logging, storage, and messaging systems. They extend the `Aspire.Hosting.IDistributedApplicationBuilder` interface allowing the app host project to express resources in the distributed application builder and are tagged with `aspire`, `integration`, and `hosting`.

For this example, the [`Aspire.Hosting.Azure.KeyVault`](https://nuget.org/packages/Aspire.Hosting.Azure.KeyVault) is installed in the _*.AppHost_ project and the _Program.cs_ file adds an Azure Key Vault resource to the app model:

```csharp
var builder =
    DistributedApplication.CreateBuilder(args);

// Conditionally update the app model with secrets.
var secrets =
    builder.ExecutionContext.IsPublishMode
        ? builder.AddAzureKeyVault("secrets")
        : builder.AddConnectionString("secrets");

// Express that the API service takes a dependency on
// the "secrets" instance.
builder.AddProject<Projects.AspireApp_KeyVault_ApiService>(
            name: "apiservice")
       .WithReference(secrets);

builder.Build().Run();
```

## Client integration

These packages configure existing libraries to connect to resource-backed integrations. They extend the `Microsoft.Extensions.DependencyInjection.IServiceCollection` interface allowing client-consuming projects to use the connected resource and are tagged with `aspire`, `integration`, and `client`

In this example, the consuming project is an ASP.NET Core Minimal API that installs the [`Aspire.Azure.Security.KeyVault`](https://nuget.org/packages/Aspire.Azure.Security.KeyVault) and its _Program.cs_ looks like this:

```csharp
using AspireApp.KeyVault.ApiService.Secrets;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add Azure Key Vault secret values to app configuration.
builder.Configuration.AddAzureKeyVaultSecrets("secrets");

// Add Azure Key Vault 'SecretClient' to DI container.
builder.AddAzureKeyVaultClient("secrets");

// Add services to the container.
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapSecretsEndpoints();

app.MapDefaultEndpoints();

app.Run();
```

## See also

For more information, see [.NET Aspire Azure Key Vault integration](https://learn.microsoft.com/dotnet/aspire/security/azure-security-key-vault-integration).
