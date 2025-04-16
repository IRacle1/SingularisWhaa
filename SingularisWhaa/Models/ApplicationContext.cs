using Microsoft.EntityFrameworkCore;

using SingularisWhaa.Models.User;

namespace SingularisWhaa.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<UserDatabase> Users { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("UsersDatabaseTest");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserDatabase>().HasIndex(u => u.Email).IsUnique();
        }
    }
}
