using Dog_App.Models;
using Microsoft.EntityFrameworkCore;

namespace DOG_MANAGEMENT_APP.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Dog> Dogs { get; set; }
    }
}