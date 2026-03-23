using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using STATE_MANAGEMENT.Data;
using STATE_MANAGEMENT.Models;

namespace STATE_MANAGEMENT.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    private int a = 0;

    [HttpPost]
    public IActionResult SetA()
    {
        a = 10;
        ViewBag.AValue = "A has been set to 10 ";
        return View("Index");
    }
    [HttpPost]
    public IActionResult GetA()
    {
        ViewBag.AValue = $"A is currently :{a}";
        return View("Index");
    }

    public IActionResult Index()
    {
        TempData["myKey"] = "DATA FROM INDEX METHOD...";
        return View();
    }

    public IActionResult Index2()
    {
        ViewBag.MyKey = TempData["myKey"];
        TempData.Keep("myKey");
        return View();
    }

    public IActionResult Index3()
    {
        ViewBag.MyKey = TempData["myKey"];
        TempData.Keep("myKey");
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
}