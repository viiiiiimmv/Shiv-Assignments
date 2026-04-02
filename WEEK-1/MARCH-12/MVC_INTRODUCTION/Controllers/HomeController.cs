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
            Name = "Alice Johnson",
            Salary = 100000,
            ImageUrl = "/Images/profile.png",
            DeptId = 1
        },
        new ()
        {
            EmployeeId = 2,
            Name = "Brian Smith",
            Salary = 120000,
            ImageUrl = "/Images/profile.png",
            DeptId = 2
        },
        new ()
        {
            EmployeeId = 3,
            Name = "Clara Nguyen",
            Salary = 180000,
            ImageUrl = "/Images/profile.png",
            DeptId = 3
        },
        new ()
        {
            EmployeeId = 4,
            Name = "David Patel",
            Salary = 140000,
            ImageUrl = "/Images/profile.png",
            DeptId = 3
        },
        new ()
        {
            EmployeeId = 5,
            Name = "Emma Rodriguez",
            Salary = 95000,
            ImageUrl = "/Images/profile.png",
            DeptId = 1
        },
        new ()
        {
            EmployeeId = 6,
            Name = "Frank Lee",
            Salary = 110000,
            ImageUrl = "/Images/profile.png",
            DeptId = 1
        },
        new ()
        {
            EmployeeId = 7,
            Name = "Grace Kim",
            Salary = 130000,
            ImageUrl = "/Images/profile.png",
            DeptId = 2
        },
        new ()
        {
            EmployeeId = 8,
            Name = "Henry Walker",
            Salary = 160000,
            ImageUrl = "/Images/profile.png",
            DeptId = 2
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

    public IActionResult Details(int id)
    {
        var empl = _employees.FirstOrDefault(x => x.EmployeeId == id);
        if (empl == null)
        {
            return NotFound();
        }
        return View(empl);
    }

    public IActionResult SearchEmployee(int id)
    {
        var emp = (from e1 in _employees where e1.EmployeeId == id select e1).FirstOrDefault();
        return View(emp);
    }
    
    private List<Department> deptlist = new List<Department>()
    {
        new (){DeptId=1,Name="Sales"},
        new (){DeptId=2,Name="HR"},
        new (){DeptId=3,Name="Software"}
    };

    public IActionResult Departments()
    {
        return View(deptlist);
    }

    public IActionResult VariousObjectsPassing(int empId)
    {
        var query1 = deptlist.ToList();
        var emp = _employees.Where(x => x.EmployeeId == empId).FirstOrDefault();
        var query2 = emp;
        EmpDeptModel empDeptModel = new EmpDeptModel()
        {
            _departments = query1,
            _employee = emp,
            _date = DateTime.Now,
        };
        return View(empDeptModel);
    }

    public IActionResult EmployeeDetails()
    {
        EmployeesAndDepartments obj = new EmployeesAndDepartments()
        {
            _departments = deptlist,
            _employees = _employees
        };
        return View(obj);
    }

    public IActionResult GetEmployeesByDepartment(int deptId)
    {
        return View(_employees.Where(x => x.DeptId == deptId).ToList());
    }
}

