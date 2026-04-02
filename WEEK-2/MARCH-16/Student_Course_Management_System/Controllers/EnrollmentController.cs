using Microsoft.AspNetCore.Mvc;
using Student_Course_Management_System.Models;

namespace Student_Course_Management_System.Controllers;

public class EnrollmentController : Controller
{
    private readonly List<Course> _courses;
    private readonly List<Student> _students;

    public EnrollmentController()
    {
        _courses = new()
        {
            new() { CourseId = 1, Title = "Data Structures", Credits = 4, Department = "CSE" },
            new() { CourseId = 2, Title = "Algorithms", Credits = 4, Department = "CSE" },
            new() { CourseId = 3, Title = "Databases", Credits = 3, Department = "CSE" },
            new() { CourseId = 4, Title = "Web Dev", Credits = 3, Department = "IT" },
            new() { CourseId = 5, Title = "OS", Credits = 4, Department = "CSE" }
        };
        
        _students = new()
        {
            new() { StudentId = 1, Name = "Alice", Branch = "CSE", Enrollments = new()
            {
                new() { CourseId = 1, Grade = "A", AttemptNumber = 1 },
                new() { CourseId = 2, Grade = "A-", AttemptNumber = 1 },
                new() { CourseId = 3, Grade = "B+", AttemptNumber = 1 }
            }},
            new() { StudentId = 2, Name = "Bob", Branch = "CSE", Enrollments = new()
            {
                new() { CourseId = 1, Grade = "B", AttemptNumber = 1 },
                new() { CourseId = 4, Grade = "A", AttemptNumber = 1 },
                new() { CourseId = 5, Grade = "B+", AttemptNumber = 1 }
            }},
            new() { StudentId = 3, Name = "Charlie", Branch = "IT", Enrollments = new()
            {
                new() { CourseId = 4, Grade = "C", AttemptNumber = 1 },
                new() { CourseId = 1, Grade = "B-", AttemptNumber = 1 }
            }},
            new() { StudentId = 4, Name = "Diana", Branch = "CSE", Enrollments = new()
            {
                new() { CourseId = 2, Grade = "A", AttemptNumber = 1 },
                new() { CourseId = 5, Grade = "F", AttemptNumber = 1 },
                new() { CourseId = 5, Grade = "B", AttemptNumber = 2 }  // Retry
            }},
            new() { StudentId = 5, Name = "Eve", Branch = "IT", Enrollments = new() }  // No courses
        };

    }
    // GET
    public IActionResult Index()
    {
        var studentCourses =
            from s in _students
            from e in s.Enrollments
            join c in _courses
                on e.CourseId equals c.CourseId
            select new
            {
                StudentName = s.Name,
                CourseTitle = c.Title,
                Credits = c.Credits,
                Grade = e.Grade,
                AttemptNumber = e.AttemptNumber
            };
        return View(studentCourses.ToList());
    }

    public IActionResult Details(int? studentId, int? id)
    {
        var selectedId = studentId ?? id;
        if (!selectedId.HasValue)
        {
            return NotFound();
        }

        var student = _students.FirstOrDefault(s => s.StudentId == selectedId.Value);
        if (student == null)
        {
            return NotFound();
        }

        var studentCourses =
            from e in student.Enrollments
            join c in _courses
                on e.CourseId equals c.CourseId
            select new
            {
                StudentId = student.StudentId,
                StudentName = student.Name,
                CourseTitle = c.Title,
                Credits = c.Credits,
                Grade = e.Grade,
                AttemptNumber = e.AttemptNumber
            };

        var records = studentCourses.ToList();

        ViewBag.StudentName = student.Name;
        ViewBag.StudentBranch = student.Branch;
        ViewBag.HasEnrollments = records.Count > 0;

        return View(records);
    }
}
