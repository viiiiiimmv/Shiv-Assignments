using System.Diagnostics;
using INVOICE_APPLICATION.Data;
using INVOICE_APPLICATION.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace INVOICE_APPLICATION.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
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

    public IActionResult CreateUser()
    {
        return View(new Customer());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser(Customer customer)
    {
        if (!ModelState.IsValid)
        {
            return View(customer);
        }

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Users));
    }

    public async Task<IActionResult> Users()
    {
        var users = await _context.Customers
            .AsNoTracking()
            .OrderBy(customer => customer.Name)
            .ToListAsync();

        return View(users);
    }

    public async Task<IActionResult> Order(int? customerId = null)
    {
        var vm = await BuildOrderViewModelAsync(customerId);
        return View(vm);
    }

    public async Task<IActionResult> CustomerInvoices(int id)
    {
        var customer = await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(customer => customer.Id == id);

        if (customer is null)
        {
            return NotFound();
        }

        var invoices = await _context.Invoices
            .AsNoTracking()
            .Where(invoice => invoice.CustomerId == id)
            .OrderByDescending(invoice => invoice.CreatedOn)
            .ToListAsync();

        var vm = new CustomerInvoicesViewModel
        {
            Customer = customer,
            Invoices = invoices
        };

        return View(vm);
    }

    public async Task<IActionResult> Invoice(int id)
    {
        var invoice = await _context.Invoices
            .AsNoTracking()
            .Include(item => item.Customer)
            .Include(item => item.Items.OrderBy(line => line.Id))
            .FirstOrDefaultAsync(item => item.Id == id);

        if (invoice is null)
        {
            return NotFound();
        }

        return View(invoice);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PlaceOrder(int? customerId, Dictionary<int, int>? quantities)
    {
        var normalizedQuantities = NormalizeQuantities(quantities);

        if (!customerId.HasValue)
        {
            return View("Order", await BuildOrderViewModelAsync(
                customerId,
                normalizedQuantities,
                "Select a customer before generating an invoice."));
        }

        if (normalizedQuantities.Count == 0)
        {
            return View("Order", await BuildOrderViewModelAsync(
                customerId,
                normalizedQuantities,
                "Add at least one product quantity to create an invoice."));
        }

        var customer = await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == customerId.Value);

        if (customer is null)
        {
            return View("Order", await BuildOrderViewModelAsync(
                customerId,
                normalizedQuantities,
                "The selected customer could not be found. Please choose a valid customer."));
        }

        var productIds = normalizedQuantities.Keys.ToList();

        var selectedProducts = await _context.Products
            .AsNoTracking()
            .Where(product => productIds.Contains(product.Id))
            .OrderBy(product => product.Id)
            .ToListAsync();

        if (selectedProducts.Count != productIds.Count)
        {
            return View("Order", await BuildOrderViewModelAsync(
                customerId,
                normalizedQuantities,
                "One or more selected products are no longer available. Please review the order and try again."));
        }

        var invoice = new Invoice
        {
            CustomerId = customer.Id,
            CreatedOn = DateTime.Now,
            Items = selectedProducts.Select(product => new InvoiceLineItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Description = product.Description,
                Quantity = normalizedQuantities[product.Id],
                UnitPrice = decimal.Round((decimal)product.Price, 2)
            }).ToList()
        };

        invoice.TotalAmount = invoice.Items.Sum(item => item.LineTotal);

        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Invoice), new { id = invoice.Id });
    }

    private async Task<OrderViewModel> BuildOrderViewModelAsync(
        int? customerId = null,
        Dictionary<int, int>? quantities = null,
        string? errorMessage = null)
    {
        var products = await _context.Products
            .AsNoTracking()
            .OrderBy(product => product.Id)
            .ToListAsync();

        var customers = await _context.Customers
            .AsNoTracking()
            .OrderBy(customer => customer.Name)
            .ToListAsync();

        var normalizedQuantities = NormalizeQuantities(quantities);

        foreach (var product in products)
        {
            normalizedQuantities.TryAdd(product.Id, 0);
        }

        return new OrderViewModel
        {
            CustomerId = customerId,
            Customers = customers,
            Products = products,
            Quantities = normalizedQuantities,
            ErrorMessage = errorMessage
        };
    }

    private static Dictionary<int, int> NormalizeQuantities(Dictionary<int, int>? quantities)
    {
        if (quantities is null)
        {
            return [];
        }

        return quantities
            .Where(entry => entry.Value > 0)
            .ToDictionary(entry => entry.Key, entry => entry.Value);
    }
}
