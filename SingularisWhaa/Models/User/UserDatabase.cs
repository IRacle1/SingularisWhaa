using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

namespace SingularisWhaa.Models.User
{
    public class UserDatabase
    {
        [Key]
        public Guid Id { get; set; }

        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Age { get; set; }
        public DateTimeOffset TimestampUtc { get; set; }

        public TimeSpan DeltaTime => DateTimeOffset.UtcNow - TimestampUtc;
    }
}
