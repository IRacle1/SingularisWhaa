using System.Diagnostics.CodeAnalysis;

namespace SingularisWhaa.Services.Abstractions.Config;

/// <summary>
/// Конфиг категории, взаимодействующий с <see cref="IConfigManagerService"/>
/// </summary>
public interface ICategoryConfig
{
    /// <summary>
    /// Пробует получать объект из конфига.
    /// </summary>
    /// <typeparam name="T">Тип объекта.</typeparam>
    /// <param name="propertyName">Название ключа объекта.</param>
    /// <param name="value">Возвращаемый объект.</param>
    /// <returns><see langword="true"/> - если удалось получить объект, иначе - <see langword="false"/>.</returns>
    bool TryGet<T>(string propertyName, [NotNullWhen(true)] out T? value);

    /// <summary>
    /// Получает объект из конфига.
    /// </summary>
    /// <typeparam name="T">Тип объекта.</typeparam>
    /// <param name="propertyName">Название ключа объекта.</param>
    /// <returns>Объект.</returns>
    T? Get<T>(string propertyName);
}
