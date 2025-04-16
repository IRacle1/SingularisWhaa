using System.Diagnostics.CodeAnalysis;

namespace SingularisWhaa.Models.Abstractions;

public interface ICategoryConfig
{
    bool TryGet<T>(string propertyName, [NotNullWhen(true)] out T? value);

    T? Get<T>(string propertyName);
}
