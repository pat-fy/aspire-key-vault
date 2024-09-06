namespace AspireApp.KeyVault.Web;

public class ApiClient(HttpClient httpClient)
{
    public async Task<string[]> GetSecretNamesAsync(
        CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<string[]>(
            "api/secrets", cancellationToken) ?? [];
    }

    public async Task<string?> GetSecretAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<string>(
            requestUri: $"api/secrets/{name}", 
            cancellationToken);
    }

    public async Task<string> CreateSecretAsync(
        string name,
        string value,
        CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync(
            requestUri: "api/secrets",
            value: new SecretModel(name, value),
            cancellationToken: cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync(
            cancellationToken);
    }
}

public sealed record class SecretModel(string Name, string Value);
