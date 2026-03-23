using CodeFirst.Data;
using CodeFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Controllers;

public class EmployeeController : Controller
{
    private readonly EventContext _context;

    public EmployeeController(EventContext context)
    {
        _context = context;
    }
    
    // GET
    public async Task<IActionResult> Index()
    {
        var res = await _context.Employees.ToListAsync();
        return View(res);
    }

    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Employee employee)
    {
        if (ModelState.IsValid)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var emp = await _context.Employees
            .FirstOrDefaultAsync(x => x.Id == id);

        if (emp == null)
            return NotFound();

        return View(emp);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var emp = await _context.Employees
            .FirstOrDefaultAsync(x => x.Id == id);

        if (emp == null)
            return NotFound();

        return View(emp);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Employee emp)
    {
        if (id != emp.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Employees.Update(emp);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Employees.Any(e => e.Id == emp.Id))
                    return NotFound();
            }

            return RedirectToAction("Index");
        }

        return View(emp);
    }
    
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var emp = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == id);

        if (emp == null)
            return NotFound();

        return View(emp);
    }
    
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var emp = await _context.Employees.FindAsync(id);

        if (emp != null)
        {
            _context.Employees.Remove(emp);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }
    
}