namespace Code_First_Approach.Models;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public CourseLevel Level { get; set; }
    public List<Student> Students { get; set; }
    public Author Author { get; set; }
    public int AuthorId { get; set; }
}

public enum CourseLevel
{
    Beginner = 1,
    Average = 2,
    Difficult = 3
}