using Microsoft.AspNetCore.Mvc;
using WEB_API.DTO;
using WEB_API.Services;

namespace WEB_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmpController : ControllerBase
{
    private readonly IEmployee _employeeService;

    public EmpController(IEmployee employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeDto>>> GetAll(int page = 1, int pageSize = 5)
    {
        return Ok(await _employeeService.GetAllEmployeesAsync(page, pageSize));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDto>> GetById(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);

        if (employee == null)
            return NotFound("Employee not found");

        return Ok(employee);
    }

    [HttpGet("basic")]
    public async Task<ActionResult<List<EmployeeBasicDto>>> GetBasic(
        int page = 1,
        int pageSize = 5,
        string? search = null)
    {
        return Ok(await _employeeService.GetAllEmployeeBasicInfoAsync(page, pageSize, search));
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> Create(
        [FromForm] EmployeeDto employeeDto,
        IFormFile? image)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var added = await _employeeService.AddEmployeeAsync(employeeDto, image);
        return Ok(added);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EmployeeDto>> Update(
        int id,
        [FromForm] EmployeeUpdateDto employeeDto,
        IFormFile? image)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updated = await _employeeService.UpdateEmployeeAsync(id, employeeDto, image);

        if (updated == null)
            return NotFound("Employee not found to update");

        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<EmployeeDto>> Delete(int id)
    {
        var deleted = await _employeeService.DeleteEmployeeAsync(id);

        if (deleted == null)
            return NotFound("Employee not found to delete");

        return Ok(deleted);
    }
}