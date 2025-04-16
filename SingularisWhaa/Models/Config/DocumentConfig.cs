using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

using SingularisWhaa.Models.Abstractions;
using SingularisWhaa.Services;

namespace SingularisWhaa.Models.Config;

public class DocumentConfig : ICategoryConfig
{
    private readonly JsonConfigManagerService parentService;
    private readonly JsonDocument targetDocument;

    public DocumentConfig(JsonConfigManagerService mainService, JsonDocument document)
    {
        parentService = mainService;
        targetDocument = document;
    }

    /// <inheritdoc/>
    public bool TryGet<T>(string propertyName, [NotNullWhen(true)] out T? value)
    {
        if (!targetDocument!.RootElement.TryGetProperty(propertyName, out JsonElement property))
        {
            value = default;
            return false;
        }

        value = property.Deserialize<T>(parentService.Options)!;
        return true;
    }

    /// <inheritdoc/>
    public T? Get<T>(string propertyName)
    {
        TryGet(propertyName, out T? value);
        return value;
    }
}
