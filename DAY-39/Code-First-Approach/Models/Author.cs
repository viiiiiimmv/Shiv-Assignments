namespace Code_First_Approach.Models;

public class Author
{
    public int  Id { get; set; }
    public string Name { get; set; }
    public List<Course> Courses { get; set; }
}