using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEBAPI_DEMO.Models;

namespace WEBAPI_DEMO.Pages;

public class IndexModel : PageModel
{
    private const int DashboardPageSize = 5;
    private readonly IEmployee _employeeService;

    public IndexModel(IEmployee employeeService)
    {
        _employeeService = employeeService;
    }

    [BindProperty(SupportsGet = true)]
    public int PageNumber { get; set; } = 1;

    [BindProperty(SupportsGet = true)]
    public int? EditId { get; set; }

    [BindProperty]
    public EmployeeInputModel Input { get; set; } = new();

    public IReadOnlyList<Employee> Employees { get; private set; } = [];
    public int TotalEmployees { get; private set; }
    public int TotalPages { get; private set; }
    public string? CurrentImagePath { get; private set; }
    public bool IsEditing => Input.Id > 0;
    public bool HasCurrentImage => !string.IsNullOrWhiteSpace(CurrentImagePath);

    [TempData]
    public string? StatusMessage { get; set; }

    [TempData]
    public string? StatusTone { get; set; }

    public async Task OnGetAsync()
    {
        await LoadEmployeesAsync();
        await LoadEditFormAsync();
    }

    public async Task<IActionResult> OnPostSaveAsync(int pageNumber = 1)
    {
        PageNumber = Math.Max(pageNumber, 1);

        if (!ModelState.IsValid)
        {
            await LoadEmployeesAsync();
            await LoadExistingImageAsync(Input.Id);
            return Page();
        }

        try
        {
            var employee = new Employee
            {
                Id = Input.Id,
                FirstName = Input.FirstName?.Trim(),
                LastName = Input.LastName?.Trim(),
                Email = Input.Email?.Trim(),
                Age = Input.Age
            };

            if (Input.Id > 0)
            {
                var updated = await _employeeService.Update(Input.Id, employee, Input.Image);

                if (updated == null)
                {
                    SetStatus("Employee not found.", "danger");
                    return RedirectToPage(new { pageNumber = PageNumber });
                }

                SetStatus("Employee updated successfully.", "success");
            }
            else
            {
                await _employeeService.Create(employee, Input.Image);
                SetStatus("Employee added successfully.", "success");
            }

            return RedirectToPage(new { pageNumber = PageNumber });
        }
        catch (InvalidOperationException exception)
        {
            ModelState.AddModelError("Input.Image", exception.Message);
            await LoadEmployeesAsync();
            await LoadExistingImageAsync(Input.Id);
            return Page();
        }
    }

    public IActionResult OnPostCancelEdit(int pageNumber = 1)
    {
        return RedirectToPage(new { pageNumber = Math.Max(pageNumber, 1) });
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id, int pageNumber = 1)
    {
        pageNumber = Math.Max(pageNumber, 1);

        var deleted = await _employeeService.Delete(id);

        if (!deleted)
        {
            SetStatus("Employee not found.", "danger");
            return RedirectToPage(new { pageNumber });
        }

        var response = await _employeeService.GetAll(pageNumber, DashboardPageSize);
        if (pageNumber > 1 && response.Items.Count == 0)
        {
            pageNumber--;
        }

        SetStatus("Employee removed successfully.", "success");
        return RedirectToPage(new { pageNumber });
    }

    public string GetInitials(Employee employee)
    {
        return GetInitials(employee.FirstName, employee.LastName);
    }

    private async Task LoadEmployeesAsync()
    {
        var response = await _employeeService.GetAll(PageNumber, DashboardPageSize);
        if (response.TotalPages > 0 && response.Items.Count == 0 && response.PageNumber > response.TotalPages)
        {
            response = await _employeeService.GetAll(response.TotalPages, DashboardPageSize);
        }

        Employees = response.Items;
        TotalEmployees = response.TotalCount;
        TotalPages = response.TotalPages;
        PageNumber = response.TotalPages == 0 ? 1 : response.PageNumber;
    }

    private async Task LoadEditFormAsync()
    {
        if (!EditId.HasValue)
        {
            return;
        }

        var employee = await _employeeService.GetById(EditId.Value);
        if (employee == null)
        {
            StatusMessage = "Employee not found for editing.";
            StatusTone = "danger";
            return;
        }

        Input = new EmployeeInputModel
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            Age = employee.Age
        };

        CurrentImagePath = employee.ImagePath;
    }

    private async Task LoadExistingImageAsync(int employeeId)
    {
        if (employeeId <= 0)
        {
            CurrentImagePath = null;
            return;
        }

        var employee = await _employeeService.GetById(employeeId);
        CurrentImagePath = employee?.ImagePath;
    }

    private void SetStatus(string message, string tone)
    {
        StatusMessage = message;
        StatusTone = tone;
    }

    private static string GetInitials(string? firstName, string? lastName)
    {
        var first = string.IsNullOrWhiteSpace(firstName) ? "E" : firstName.Trim()[0].ToString().ToUpperInvariant();
        var last = string.IsNullOrWhiteSpace(lastName) ? string.Empty : lastName.Trim()[0].ToString().ToUpperInvariant();
        return string.Concat(first, last);
    }

    public sealed class EmployeeInputModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter your firstname")]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your lastname")]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Please enter email id")]
        [EmailAddress(ErrorMessage = "Please enter valid email id")]
        public string? Email { get; set; }

        [Range(1, 100, ErrorMessage = "Please enter your age between 1 to 100 only")]
        public int Age { get; set; }

        public IFormFile? Image { get; set; }
    }
}
