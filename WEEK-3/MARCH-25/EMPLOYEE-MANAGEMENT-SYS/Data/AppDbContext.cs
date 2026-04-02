using WEBAPI_DEMO.Models;

namespace WEBAPI_DEMO.Data;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Employee>  Employees { get; set; }
}
