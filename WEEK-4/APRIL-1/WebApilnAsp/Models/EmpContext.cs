using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApilnAsp.Security;

namespace WebApilnAsp.Models
{
    public class EmpContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public EmpContext(DbContextOptions<EmpContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SeedRoles(modelBuilder);
        }

        private static void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(AppRoleSeeds.Get());
        }
    }
}
