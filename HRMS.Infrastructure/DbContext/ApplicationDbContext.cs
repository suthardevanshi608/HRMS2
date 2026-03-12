using HRMS.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Employee> Employees { get; set; }   // ⭐ IMPORTANT

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Prevent EF from treating Id as identity column
            modelBuilder.Entity<Organization>()
                .Property(o => o.Id)
                .ValueGeneratedNever();

            // Set default value for CreatedAt
            modelBuilder.Entity<Organization>()
                .Property(o => o.CreatedAt)
                .HasDefaultValueSql("NOW()");
        }

    }

}

