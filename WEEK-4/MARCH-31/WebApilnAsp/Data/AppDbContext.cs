using Microsoft.EntityFrameworkCore;
using WebApilnAsp.Models;

namespace WebApilnAsp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Employee> _Employees { get; set; }
}