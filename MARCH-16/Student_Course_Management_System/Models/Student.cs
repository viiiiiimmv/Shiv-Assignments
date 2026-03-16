namespace Student_Course_Management_System.Models;

public class Student
{
    public int StudentId { get; set; }
    public string Name {get; set;} = string.Empty;
    public string Branch {get; set;} = string.Empty;
    public List<Enrollment> Enrollments { get; set; } = new();
}