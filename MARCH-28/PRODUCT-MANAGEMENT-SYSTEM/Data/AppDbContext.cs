using EMPLOYEE_MANAGEMENT_SYSTEM.Models;
using Microsoft.EntityFrameworkCore;

namespace EMPLOYEE_MANAGEMENT_SYSTEM.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Product>  Products { get; set; }
}