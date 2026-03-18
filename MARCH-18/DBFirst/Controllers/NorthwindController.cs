using DBFirst.Data;
using DBFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DBFirst.Controllers;

public class NorthwindController : Controller
{
    AppDbContext db = new AppDbContext();
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

    public IActionResult ProductsInCategory(string category)
    {
        if (string.IsNullOrWhiteSpace(category))
        {
            return View(new List<ProdCat>());
        }

        var res = db.Products
            .Where(x => x.Category != null && x.Category.CategoryName == category)
            .Select(p => new ProdCat
            {
                prodName = p.ProductName,
                catName = p.Category!.CategoryName
            })
            .ToList();

        return View(res);
    }

    public IActionResult CustomersByRange(string range)
    {
        if (string.IsNullOrWhiteSpace(range))
        {
            return View(new List<Customer>());
        }

        if (!int.TryParse(range, out int parsedRange))
        {
            return View(new List<Customer>());
        }

        var res = db.Customers
            .Where(x => x.Orders.Count > parsedRange)
            .Select(x => new Customer
            {
                CustomerId = x.CustomerId,
                ContactName = x.ContactName
            })
            .ToList();

        return View(res);
    }

    public async Task<IActionResult> CustomerOrderDetails(string searchString)
    {
        var res = await db.Customers
            .Join(db.Orders,
                c => c.CustomerId,
                o => o.CustomerId,
                (c, o) => new { c, o })
            .Where(x => x.c.CustomerId == searchString)
            .Select(x => new
            {
                oid = x.o.OrderId,
                odate = x.o.OrderDate,
                cmpname = x.c.CompanyName,
                cname = x.c.ContactName
            })
            .ToListAsync();

        return View(res);
    }
}