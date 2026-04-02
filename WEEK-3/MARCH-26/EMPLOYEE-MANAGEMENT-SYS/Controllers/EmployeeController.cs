using Microsoft.AspNetCore.Mvc;
using WEBAPI_DEMO.Models;

namespace WEBAPI_DEMO.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployee _employeeService;

    public EmployeeController(IEmployee employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<Employee>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<Employee>>> GetAll([FromQuery] EmployeeQueryParameters query)
    {
        var data = await _employeeService.GetAll(query.PageNumber, query.PageSize);
        return Ok(data);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Employee>> GetById(int id)
    {
        var employee = await _employeeService.GetById(id);

        if (employee == null)
            return NotFound();

        return Ok(employee);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(Employee), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromForm] EmployeeUpsertRequest request)
    {
        try
        {
            var employee = new Employee
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Age = request.Age
            };

            var created = await _employeeService.Create(employee, request.Image);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromForm] EmployeeUpsertRequest request)
    {
        try
        {
            var employee = new Employee
            {
                Id = id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Age = request.Age
            };

            var updated = await _employeeService.Update(id, employee, request.Image);

            if (updated == null)
                return NotFound();

            return Ok(updated);
        }
        catch (InvalidOperationException ex)
        {
            return Problem(statusCode: StatusCodes.Status400BadRequest, title: ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _employeeService.Delete(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
