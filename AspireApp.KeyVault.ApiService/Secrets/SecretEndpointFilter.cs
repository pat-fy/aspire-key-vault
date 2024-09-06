// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace AspireApp.KeyVault.ApiService.Secrets;

public class SecretEndpointFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var secretRequest = context.GetArgument<CreateSecretRequest>(1);

        Lazy<Dictionary<string, string[]>> errors = new(
            valueFactory: static () => []);

        if (string.IsNullOrWhiteSpace(secretRequest.Name))
        {
            errors.Value["name"] =
            [
                "Secret 'name' cannot be null, empty or whitespace."
            ];
        }

        if (secretRequest.Name?.Length is < 3 or > 24)
        {
            errors.Value["name"] =
            [
                "Secret 'name' must be longer than 3 characters but less than 24 characters."
            ];
        }

        if (string.IsNullOrWhiteSpace(secretRequest.Value))
        {
            errors.Value["value"] =
            [
                "Secret 'value' cannot be null, empty or whitespace."
            ];
        }

        if (errors.IsValueCreated)
        {
            return Results.ValidationProblem(errors.Value);
        }

        return await next(context);
    }
}
