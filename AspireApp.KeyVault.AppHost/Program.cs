// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

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
