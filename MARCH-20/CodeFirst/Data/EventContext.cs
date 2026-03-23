using CodeFirst.Models;

namespace CodeFirst.Data;

using Microsoft.EntityFrameworkCore;

public class EventContext : DbContext
{
    public EventContext(DbContextOptions<EventContext> options) : base(options) { }

    public DbSet<Course> Courses { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Author_1> newAuthors { get; set; }
    public DbSet<Course_1> newCourses { get; set; }
    public DbSet<Employee>  Employees { get; set; }
    public DbSet<UserDetail> UserDetails { get; set; }
    public DbSet<Post> Posts { get; set; }
}