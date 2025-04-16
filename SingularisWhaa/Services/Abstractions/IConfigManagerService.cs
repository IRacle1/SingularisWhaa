using SingularisWhaa.Models.Abstractions;

namespace SingularisWhaa.Services.Abstractions;

/// <summary>
/// Интерфейс менеджера конфигов, поддерживающий категории.
/// </summary>
public interface IConfigManagerService
{
    /// <summary>
    /// Возвращает корневой путь к папке конфигов.
    /// </summary>
    string RootPath { get; }

    /// <summary>
    /// Получает конфиг по названию категории.
    /// </summary>
    /// <param name="category">Название категории.</param>
    /// <returns>Конфиг категории.</returns>
    ICategoryConfig GetConfig(string category);
}
