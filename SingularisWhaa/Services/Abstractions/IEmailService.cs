using SingularisWhaa.Models.User;

namespace SingularisWhaa.Services.Abstractions
{
    public interface IEmailService
    {
        Task SendEmail(UserDatabase user, string template);
    }
}
