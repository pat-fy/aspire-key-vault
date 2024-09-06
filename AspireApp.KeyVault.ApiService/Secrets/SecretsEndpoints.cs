// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

using Azure.Security.KeyVault.Secrets;
using System.Runtime.CompilerServices;

namespace AspireApp.KeyVault.ApiService.Secrets;

internal static class SecretsEndpoints
{
    internal static WebApplication MapSecretsEndpoints(
        this WebApplication app)
    {
        var secrets = app.MapGroup("api/secrets");

        secrets.MapGet(
            pattern: "/", 
            handler: GetAllSecretNamesAsync);

        secrets.MapGet(
            pattern: "/{name}",
            handler: GetSecretAsync);

        secrets.MapPost(
            pattern: "/",
            handler: CreateSecretAsync)
            .AddEndpointFilter<SecretEndpointFilter>();

        return app;
    }

    private static async IAsyncEnumerable<string> GetAllSecretNamesAsync(
        SecretClient client,
        [EnumeratorCancellation]
        CancellationToken cancellationToken)
    {
        await foreach (var secret in
            client.GetPropertiesOfSecretsAsync(cancellationToken))
        {
            yield return secret.Name;
        }
    }

    private static async Task<IResult> GetSecretAsync(
        SecretClient client,
        string name,
        CancellationToken cancellationToken)
    {
        var secret = await client.GetSecretAsync(
            name, 
            cancellationToken: cancellationToken);

        if (secret is null or { Value: null })
        {
            return Results.NotFound();
        }

        return Results.Ok(new
        {
            Name = name,
            secret.Value.Value
        });
    }

    private static async Task<IResult> CreateSecretAsync(
        SecretClient client,
        CreateSecretRequest secretRequest,
        CancellationToken cancellationToken)
    {
        var secret = await client.SetSecretAsync(
            secret: secretRequest, 
            cancellationToken);

        return Results.Ok(new
        {
            secret.Value.Id
        });
    }
}
