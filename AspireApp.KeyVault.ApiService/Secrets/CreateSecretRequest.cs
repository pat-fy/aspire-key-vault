// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

using Azure.Security.KeyVault.Secrets;

namespace AspireApp.KeyVault.ApiService.Secrets;

public sealed record class CreateSecretRequest(
    string Name,
    string Value)
{
    public static implicit operator KeyVaultSecret(
        CreateSecretRequest request) =>
        new(request.Name, request.Value);
}