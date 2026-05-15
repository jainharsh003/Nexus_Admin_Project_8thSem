using Microsoft.EntityFrameworkCore;
using UserDetails.Models;

namespace UserDetails.Data
{
    public class UserDetailsDbContext : DbContext
    {
        public UserDetailsDbContext(DbContextOptions<UserDetailsDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserDetailsEntity> UserDetails { get; set; }
        public DbSet<EmploymentDetails> EmploymentDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserDetailsEntity>(entity =>
            {
                entity.ToTable("UserDetails");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.FatherName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.MotherName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Gender)
                      .IsRequired()
                      .HasMaxLength(10);

                entity.Property(e => e.Field)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Age)
                      .IsRequired();

                entity.Property(e => e.DOB)
                      .IsRequired();

                // 🔗 External relation to LoginSignup service
                entity.Property(e => e.UserId)
                      .IsRequired();
            });
        }
    }
}