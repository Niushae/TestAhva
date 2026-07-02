using Microsoft.EntityFrameworkCore;
using TestAhva.Models;

namespace TestAhva.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ProfileDetails> ProfileDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map relationships and constraints explicitly
            modelBuilder.Entity<User>()
                .HasIndex(u => new { u.DocumentType, u.DocumentNumber })
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasOne(u => u.ProfileDetail)
                .WithOne(p => p.User)
                .HasForeignKey<ProfileDetails>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
