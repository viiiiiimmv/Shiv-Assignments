using Microsoft.EntityFrameworkCore;
using Code_First_Approach.Models;

namespace Code_First_Approach.Data;

public class AppDbContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;" +
            "Database=CG_TRAINING_DB;" +
            "User Id=sa;" +
            "Password=Sql@12345;" +
            "TrustServerCertificate=True;"
        );
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed Authors
        modelBuilder.Entity<Author>().HasData(
            new Author { Id = 1, Name = "John Doe" },
            new Author { Id = 2, Name = "Jane Smith" }
        );

        // Seed Courses (with AuthorId)
        modelBuilder.Entity<Course>().HasData(
            new Course { Id = 1, Title = "C# Basics", Description = "Intro to C#",  Level = CourseLevel.Beginner, AuthorId = 1 },
            new Course { Id = 2, Title = "Advanced .NET", Description = "Deep dive into .NET",  Level = CourseLevel.Average, AuthorId = 1 },
            new Course { Id = 3, Title = "Azure Fundamentals", Description = "Cloud basics",  Level = CourseLevel.Beginner, AuthorId = 2 }
        );

        // Seed Students
        modelBuilder.Entity<Student>().HasData(
            new Student { Id = 1, Name = "Alice Johnson" },
            new Student { Id = 2, Name = "Bob Wilson" },
            new Student { Id = 3, Name = "Carol Davis" }
        );

        //// Seed Many-to-Many join table
        //modelBuilder.Entity<CourseStudent>().HasData(
        //    new StudentCourse { StudentId = 1, CourseId = 1 },  // Alice in C# Basics
        //    new StudentCourse { StudentId = 1, CourseId = 2 },  // Alice in Advanced .NET
        //    new StudentCourse { StudentId = 2, CourseId = 1 },  // Bob in C# Basics
        //    new StudentCourse { StudentId = 2, CourseId = 3 },  // Bob in Azure
        //    new StudentCourse { StudentId = 3, CourseId = 2 }   // Carol in Advanced .NET
        //);
    }
}