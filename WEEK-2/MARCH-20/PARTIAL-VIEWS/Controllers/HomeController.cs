using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MARCH_20.Models;

namespace MARCH_20.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Index2()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    List<Employee> _employees = new List<Employee>
    {
        new () { EmpId = 1, EmpName = "Arjun Sharma", Email = "arjun.sharma@company.com", Description = "Senior Software Engineer" },
        new () { EmpId = 2, EmpName = "Priya Mehta", Email = "priya.mehta@company.com", Description = "UI/UX Designer" },
        new () { EmpId = 3, EmpName = "Rohan Verma", Email = "rohan.verma@company.com", Description = "DevOps Engineer" },
        new () { EmpId = 4, EmpName = "Sneha Kapoor", Email = "sneha.kapoor@company.com", Description = "Business Analyst" },
        new () { EmpId = 5, EmpName = "Karan Patel", Email = "karan.patel@company.com", Description = "QA Engineer" },
    };

    public IActionResult DisplayEmployees()
    {
        return View(_employees);
    }
}