namespace CodeFirst.Models;

public class Author_1
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IList<Course_1> Courses { get; set; }
}