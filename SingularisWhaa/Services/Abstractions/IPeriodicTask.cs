namespace SingularisWhaa.Services.Abstractions;

/// <summary>
/// Интерфейс определяющий преодичную задачу.
/// </summary>
public interface IPeriodicTask
{
    /// <summary>
    /// Выполняет задачу🕊️.
    /// </summary>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> общей задачи.</param>
    /// <returns><see cref="Task"/> отображающая задачу по выполнению задачи🔥🔥🔥.</returns>
    Task DoTask(CancellationToken cancellationToken);
}
