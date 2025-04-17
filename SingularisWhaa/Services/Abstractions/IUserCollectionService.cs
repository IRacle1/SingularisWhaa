using SingularisWhaa.Models.User;

namespace SingularisWhaa.Services.Abstractions;

/// <summary>
/// Интерфейс для взаимодействия с коллекцией пользователей.
/// </summary>
public interface IUserCollectionService
{
    /// <summary>
    /// Проверяет уникальность Email'а.
    /// </summary>
    /// <param name="email">Email который нужно проверить.</param>
    /// <returns><see cref="Task"/> отображающая задачу, и возвращающая <see langword="true"/> - если этот email уникальный, иначе - <see langword="false"/>.</returns>
    /// <remarks>
    /// Само существование этого метода уже сомнительная штука.
    /// MemoryDb игнорит настройку уникальности ключа🙏.
    /// 
    /// UPD:
    /// Была мысль написать валидатор, но чет лишнее как будто.
    /// </remarks>
    Task<bool> CheckEmailUnique(string email);

    /// <summary>
    /// Добавляет нового пользователя.
    /// </summary>
    /// <param name="user">Объект <see cref="UserDto"/> пользователя.</param>
    /// <returns><see cref="Task"/> отображающая задачу, и возвращающая объект <see cref="UserDatabase"/> созданного пользователя.</returns>
    Task<UserDatabase> Add(UserDto user);

    /// <summary>
    /// Возвращает всех существующих пользователей. 
    /// </summary>
    /// <returns><see cref="Task"/> отображающая задачу, и возвращающая коллекцию <see cref="IReadOnlyCollection{T}"/> содержащую всех пользователей.</returns>
    Task<IReadOnlyCollection<UserDatabase>> GetAll();
}
