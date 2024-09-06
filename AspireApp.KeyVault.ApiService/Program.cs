// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

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
