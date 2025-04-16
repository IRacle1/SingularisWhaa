using System.Diagnostics.CodeAnalysis;

namespace SingularisWhaa.Services.Abstractions;

/// <summary>
/// Интерфейс ответственный за работу с Cron выражениями.
/// </summary>
public interface ICronParserService
{
    /// <summary>
    /// Возвращает следующее ближайшее время исполнения cron выражения.
    /// </summary>
    /// <param name="cronExpr">Cron выражение.</param>
    /// <param name="localTime">Нужно ли возвращать результат в местром времени.</param>
    /// <returns>Объект <see cref="DateTimeOffset"/>, <see langword="null"/> если выражение инвалидно или дата выходит за рамки.</returns>
    DateTimeOffset? NextOccurrence(string cronExpr, bool localTime);

    /// <summary>
    /// Пробует получить следующее ближайшее время исполнения cron выражения.
    /// </summary>
    /// <param name="cronExpr">Cron выражение.</param>
    /// <param name="localTime">Нужно ли возвращать результат в местром времени.</param>
    /// <param name="dateTime">Возвращаемый объект <see cref="DateTimeOffset"/>.</param>
    /// <returns><see langword="false"/> если выражение инвалидно или дата выходит за рамки, иначе - <see langword="true"/>.</returns>
    bool TryNextOccurrence(string cronExpr, bool localTime, [NotNullWhen(true)] out DateTimeOffset? dateTime);

    /// <summary>
    /// Возвращает <see cref="TimeSpan"/> до ближайшего времени исполнения cron выражения.
    /// </summary>
    /// <param name="cronExpr">Cron выражение.</param>
    /// <returns>Объект <see cref="TimeSpan"/>, <see langword="null"/> если выражение инвалидно или дата выходит за рамки.</returns>
    TimeSpan? NextOccurrenceDelta(string cronExpr);

    /// <summary>
    /// Пробует получить <see cref="TimeSpan"/> до ближайшего времени исполнения cron выражения.
    /// </summary>
    /// <param name="cronExpr">Cron выражение.</param>
    /// <param name="timeSpan">Возвращаемый объект <see cref="TimeSpan"/>.</param>
    /// <returns><see langword="false"/> если выражение инвалидно или дата выходит за рамки, иначе - <see langword="true"/>.</returns>
    bool TryNextOccurrenceDelta(string cronExpr, [NotNullWhen(true)] out TimeSpan? timeSpan);
}
