using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApilnAsp.Models;
using WebApilnAsp.Security;

namespace WebApilnAsp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = AppPolicies.EmployeeRead)]
public class EmployeeController(IEmployee employeeService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 5)
    {
        var data = await employeeService.GetAllEmployeesAsync(pageNumber, pageSize);
        return Ok(data);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetById(int id)
    {
        var employee = await employeeService.GetEmployeeByIdAsync(id);

        if (employee == null)
            return NotFound("Employee not found");

        return Ok(employee);
    }

    [HttpPost]
    [Authorize(Policy = AppPolicies.EmployeeWrite)]
    public async Task<IActionResult> Create([FromForm] Employee emp, IFormFile? image)
    {
        var created = await employeeService.AddEmployeeAsync(emp, image);
        return Ok(created);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = AppPolicies.EmployeeWrite)]
    public async Task<ActionResult<Employee>> Update(int id, [FromForm] EmployeeUpdateDto employeeDto, IFormFile? image)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var employee = new Employee
        {
            Id = id,
            FirstName = employeeDto.FirstName,
            LastName = employeeDto.LastName,
            Email = employeeDto.Email,
            Age = employeeDto.Age,
            ImagePath = employeeDto.ImagePath
        };

        var updated = await employeeService.UpdateEmployeeAsync(employee, image);

        if (updated == null)
            return NotFound("Employee not found to update");

        return Ok(updated);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = AppPolicies.EmployeeWrite)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await employeeService.DeleteEmployeeAsync(id);

        if (deleted == null)
            return NotFound("Employee not found");

        return Ok("Employee deleted successfully");
    }
    
    
    [HttpGet("basic")]
    public async Task<ActionResult<List<EmployeeDTO>>> GetBasicEmployeeList(
        int page = 1, int pageSize = 5, string? search = null)
    {
        var result = await employeeService.GetAllEmployeeBasicInfoAsync(page, pageSize, search);
        return Ok(result);
    }

    [HttpGet("export/excel")]
    public async Task<IActionResult> ExportToExcel(string? search = null)
    {
        var employees = await employeeService.GetAllEmployeeBasicInfoAsync(1, int.MaxValue, search);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Employees");

        worksheet.Cell(1, 1).Value = "First Name";
        worksheet.Cell(1, 2).Value = "Last Name";
        worksheet.Cell(1, 3).Value = "Email";
        worksheet.Cell(1, 4).Value = "Image URL";

        int row = 2;
        foreach (var emp in employees)
        {
            worksheet.Cell(row, 1).Value = emp.FirstName;
            worksheet.Cell(row, 2).Value = emp.LastName;
            worksheet.Cell(row, 3).Value = emp.Email;
            worksheet.Cell(row, 4).Value = emp.ImageUrl;
            row++;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Seek(0, SeekOrigin.Begin);

        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Employees.xlsx");
    }
}
