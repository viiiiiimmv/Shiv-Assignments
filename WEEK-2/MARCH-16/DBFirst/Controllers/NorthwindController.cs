using DBFirst.Data;
using Microsoft.AspNetCore.Mvc;

namespace DBFirst.Controllers;

public class NorthwindController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult SpainCustomers()
    {
        AppDbContext cnt = new AppDbContext();
        var spainCustomer = cnt.Customers.Where(x => x.Country == "Spain").Select(x => new { cid = x.CustomerId, cname = x.ContactName, comname = x.CompanyName });
        return View(spainCustomer);
    }
}