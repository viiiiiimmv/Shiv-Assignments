using Microsoft.EntityFrameworkCore;
using WEB_APPLICATION.Models;

namespace WEB_APPLICATION.Data;

public class AzureContext : DbContext
{
    public AzureContext(DbContextOptions<AzureContext> options) : base(options) { }

    public DbSet<Person> Persons { get; set; } = null!;
}