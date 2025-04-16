
using Microsoft.EntityFrameworkCore;

using SingularisWhaa.Models;
using SingularisWhaa.Models.User;
using SingularisWhaa.Services.Abstractions;

namespace SingularisWhaa.Services
{
    public class EFUserCollectionService : IUserCollectionService
    {
        private readonly ApplicationContext context;

        public EFUserCollectionService(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task<UserDatabase?> Add(UserDto user)
        {
            UserDatabase userDatabase = new UserDatabase()
            {
                Name = user.Name!,
                Email = user.Email!,
                TimestampUtc = DateTimeOffset.UtcNow,
                Age = user.Age,
            };

            await context.Users.AddAsync(userDatabase);
            await context.SaveChangesAsync();

            return userDatabase;
        }

        public async Task<bool> CheckEmailUnique(string email)
        {
            return await context.Users.AllAsync(u => 
                !string.Equals(u.Email, email, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<IReadOnlyCollection<UserDatabase>> GetAll()
        {
            return await context.Users.AsNoTracking().ToListAsync();
        }
    }
}
