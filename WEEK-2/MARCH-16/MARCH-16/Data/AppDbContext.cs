namespace MARCH_16.Data;

using Microsoft.EntityFrameworkCore;
using MARCH_16.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> _products { get; set; }  // replace Dog with your model name
}