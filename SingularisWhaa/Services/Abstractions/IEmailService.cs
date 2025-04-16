using SingularisWhaa.Models.User;

namespace SingularisWhaa.Services.Abstractions;

/// <summary>
/// Интерфейс ответственный за отправку Email'ов.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Отправляет Email человеку, по нужному шаблону.
    /// </summary>
    /// <param name="user">Пользователь, которому нужно отправить сообщение.</param>
    /// <param name="template">Название шаблона</param>
    /// <returns><see cref="Task"/> отображающая задачу отправки сообщения.</returns>
    Task SendEmail(UserDatabase user, string template);
}
