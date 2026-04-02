using Microsoft.EntityFrameworkCore;

namespace CRUD_OPERATIONS.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Dog> Dogs { get; set; }
    }
}