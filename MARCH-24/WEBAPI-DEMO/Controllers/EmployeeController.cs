using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEBAPI_DEMO.Data;
using WEBAPI_DEMO.Models;

namespace WEBAPI_DEMO.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public EmployeeController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<Employee>>> Get()
    {
        return Ok(await _dbContext.Employees.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> Get(int id)
    {
        var employee = await _dbContext.Employees.FindAsync(id);

        if (employee == null)
            return NotFound($"Employee with ID {id} not found.");

        return Ok(employee);
    }

    [HttpPost]
    public async Task<ActionResult<Employee>> Post([FromBody] Employee employee)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _dbContext.Employees.AddAsync(employee);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Employee>> Put(int id, [FromBody] Employee employee)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _dbContext.Employees.FindAsync(id);

        if (existing == null)
            return NotFound($"Employee with ID {id} not found.");

        existing.FirstName = employee.FirstName;
        existing.LastName = employee.LastName;
        existing.Email = employee.Email;
        existing.Age = employee.Age;

        await _dbContext.SaveChangesAsync();

        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var employee = await _dbContext.Employees.FindAsync(id);

        if (employee == null)
            return NotFound($"Employee with ID {id} not found.");

        _dbContext.Employees.Remove(employee);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}