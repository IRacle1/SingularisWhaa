using SingularisWhaa.Models.User;

namespace SingularisWhaa.Services.Abstractions
{
    public interface IUserCollectionService
    {
        Task<bool> CheckEmailUnique(string email);
        Task<UserDatabase?> Add(UserDto user);
        Task<IReadOnlyCollection<UserDatabase>> GetAll();
    }
}
