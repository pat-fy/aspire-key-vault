using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.JSInterop;

namespace AspireApp.KeyVault.Web.Components.Pages;

[Route("/secrets")]
[StreamRendering(true)]
[OutputCache(Duration = 5)]
public sealed partial class Secrets
{
    private string[]? _secrets;
    private SecretModel _model = new("", "");
    private bool _showValue = false;
    private ElementReference _valueInput;
    private bool _isCreatingSecret = false;

    [Inject] public required ApiClient ApiClient { get; set; }

    [Inject] public required ILogger<Secrets> Logger { get; set; }

    [Inject] public required IJSRuntime JavaScript { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await GetSecretsAsync();
    }

    private async Task GetSecretsAsync()
    {
        _secrets = await ApiClient.GetSecretNamesAsync();
    }

    private async Task PostSecretAsync(EditContext context)
    {
        if (context.Validate() is false)
        {
            return;
        }

        _isCreatingSecret = true;

        try
        {
            var json = await ApiClient.CreateSecretAsync(
                _model.Name,
                _model.Value);

            _model = new("", "");

            Logger.LogInformation(
                "Secret JSON: {Json}",
                json);

            await GetSecretsAsync();
        }
        finally
        {
            _isCreatingSecret = false;
        }
    }

    private async Task ToggleValueVisibility()
    {
        _showValue = _showValue is false;

        await JavaScript.InvokeVoidAsync(
            "app.toggleInputType",
            _valueInput, _showValue ? "text" : "password");
    }
}

internal sealed record class SecretModel(string Name, string Value);
