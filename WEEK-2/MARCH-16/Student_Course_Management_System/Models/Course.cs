namespace Student_Course_Management_System.Models;

public class Course
{
    public int CourseId {get; set;}
    public string Title { get; set; }
    public int Credits { get; set; }
    public string Department { get; set; } = string.Empty;
    public List<Enrollment> Enrollments { get; set; } = new ();
}