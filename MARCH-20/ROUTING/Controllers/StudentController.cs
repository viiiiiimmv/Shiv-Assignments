using Microsoft.AspNetCore.Mvc;
using ROUTING.Models;

namespace ROUTING.Controllers;

public class StudentController : Controller
{
    List<Student> _students = new List<Student>()
    {
        new() { Id = 1, Name = "Robert",  Email = "robert@company.com",  Class = 9  },
        new() { Id = 2, Name = "Priya",   Email = "priya@company.com",   Class = 9  },
        new() { Id = 3, Name = "Arjun",   Email = "arjun@company.com",   Class = 10 },
        new() { Id = 4, Name = "Sneha",   Email = "sneha@company.com",   Class = 10 },
        new() { Id = 5, Name = "Karan",   Email = "karan@company.com",   Class = 11 },
        new() { Id = 6, Name = "Meera",   Email = "meera@company.com",   Class = 11 },
        new() { Id = 7, Name = "Rohan",   Email = "rohan@company.com",   Class = 9  },
        new() { Id = 8, Name = "Anjali",  Email = "anjali@company.com",  Class = 10 },
    };
    // GET
    [Route("studs")]
    public IActionResult Index()
    {
        return View(_students);
    }

    [Route("studs/{id}")]
    public IActionResult Details(int id)
    {
        var student = _students.FirstOrDefault(x => x.Id == id);
        return View(student);
    }

    [Route("studsfew")]
    public IActionResult StudentClass()
    {
        var res = _students
            .Select(s => new Student
            {
                Id = s.Id,
                Class = s.Class,
                Name = s.Name
            })
            .ToList();
        
        return View(res);
    }
}