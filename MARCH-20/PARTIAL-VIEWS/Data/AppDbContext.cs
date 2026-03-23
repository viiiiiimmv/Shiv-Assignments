using MARCH_20.Models;

namespace MARCH_20.Data;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Employee> NewEmployees { get; set; }
}