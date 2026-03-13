using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC_INTRODUCTION.Models;

namespace MVC_INTRODUCTION.Controllers;

public class HomeController : Controller
{
    
    public IActionResult About()
    {
        var age = 21;
        var name = "VIII.III.MMV";
        ViewBag.Name = name;
        ViewBag.Age = age;
        ViewData["Message"] = "MISSION SUCCESSFUL...";
        ViewData["Year"] = DateTime.Now.Year;
        return View();
    }
    
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public string MyInfo(string name, int age)
    {
        return $"CODENAME : {name}\nAGE : {age}";
    }

    public string SayMyName()
    {
        return "VIII.III.MMV";
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Cod3()
    {
        return View();
    }

    private Employee obj = new Employee()
    {
        EmployeeId = 1,
        Name = "VIII.III.MMV",
        Salary = 100000,
        ImageUrl = "/Images/SCN835.png"
    };

    private List<Employee> _employees = new List<Employee>()
    {
        new ()
        {
            EmployeeId = 1,
            Name = "VIII.III.MMV",
            Salary = 100000,
            ImageUrl = "/Images/SCN835.png"
        },
        new ()
        {
            EmployeeId = 2,
            Name = "DEVLINK",
            Salary = 120000,
            ImageUrl = "/Images/DEVLINK.png"
        },
        new ()
        {
            EmployeeId = 3,
            Name = "PROJECT0X",
            Salary = 180000,
            ImageUrl = "/Images/VIIIIIIMMV-DARK.png"
        },
        new ()
        {
            EmployeeId = 4,
            Name = "PROJECT01X",
            Salary = 140000,
            ImageUrl = "/Images/VIIIIIIMMV-LIGHT.png"
        }
    };

    public IActionResult MultipleObjectPassing()
    {
        return View(_employees);
    }
    
    public IActionResult SingleObjectPassing()
    {
        return View(obj);
    }

    public IActionResult Customs()
    {
        return View();
    }

    public IActionResult temp()
    {
        return View();
    }
}

