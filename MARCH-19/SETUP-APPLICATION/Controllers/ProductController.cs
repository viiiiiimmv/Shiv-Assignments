using Microsoft.AspNetCore.Mvc;
using SETUP_APPLICATION.Data;

namespace SETUP_APPLICATION.Controllers;

public class ProductController : Controller
{
    private readonly AppDbContext db;
    // GET
    public IActionResult Index()
    {
        return View();
    }
}