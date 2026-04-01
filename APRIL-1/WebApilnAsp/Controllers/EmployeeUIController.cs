using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using WebApilnAsp.Models;

namespace WebApilnAsp.Controllers;

public class EmployeeUIController(IHttpClientFactory httpClientFactory) : Controller
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 7)
    {
        ViewBag.PageNumber = pageNumber;
        ViewBag.PageSize = pageSize;

        var response = await CreateClient().GetAsync(BuildApiUrl(
            "api/employee",
            new Dictionary<string, string?>
            {
                ["pageNumber"] = pageNumber.ToString(),
                ["pageSize"] = pageSize.ToString()
            }));

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Unable to load employees.");
            return View(new List<Employee>());
        }

        var json = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<List<Employee>>(json, _jsonOptions) ?? new List<Employee>();
        return View(data);
    }

    public async Task<IActionResult> Details(int id)
    {
        var employee = await GetEmployeeAsync(id);
        if (employee == null) return NotFound();
        return View(employee);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Employee emp, IFormFile? image)
    {
        if (!ModelState.IsValid) return View(emp);

        using var formData = BuildEmployeeFormData(emp.FirstName, emp.LastName, emp.Email, emp.Age, emp.ImagePath, image);

        var response = await CreateClient().PostAsync(BuildApiUrl("api/employee"), formData);
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Unable to create employee.");
            return View(emp);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var employee = await GetEmployeeAsync(id);
        if (employee == null) return NotFound();
        return View(employee);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EmployeeUpdateDto dto, IFormFile? image)
    {
        if (!ModelState.IsValid) return View(MapToEmployee(id, dto));

        using var formData = BuildEmployeeFormData(dto.FirstName, dto.LastName, dto.Email, dto.Age, dto.ImagePath, image);

        var response = await CreateClient().PutAsync(BuildApiUrl($"api/employee/{id}"), formData);
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Unable to update employee.");
            return View(MapToEmployee(id, dto));
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var employee = await GetEmployeeAsync(id);
        if (employee == null) return NotFound();
        return View(employee);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var response = await CreateClient().DeleteAsync(BuildApiUrl($"api/employee/{id}"));
        if (!response.IsSuccessStatusCode) return NotFound();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Basic(int page = 1, int pageSize = 5, string? search = null)
    {
        ViewBag.PageNumber = page;
        ViewBag.PageSize = pageSize;
        ViewBag.Search = search;

        var response = await CreateClient().GetAsync(BuildApiUrl(
            "api/employee/basic",
            new Dictionary<string, string?>
            {
                ["page"] = page.ToString(),
                ["pageSize"] = pageSize.ToString(),
                ["search"] = search
            }));

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Unable to load employee data.");
            return View(new List<EmployeeDTO>());
        }

        var json = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<List<EmployeeDTO>>(json, _jsonOptions) ?? new List<EmployeeDTO>();
        return View(data);
    }

    public async Task<IActionResult> ExportExcel(string? search = null)
    {
        var response = await CreateClient().GetAsync(BuildApiUrl(
            "api/employee/export/excel",
            new Dictionary<string, string?>
            {
                ["search"] = search
            }));

        if (!response.IsSuccessStatusCode)
        {
            return RedirectToAction(nameof(Basic), new { search });
        }

        var fileBytes = await response.Content.ReadAsByteArrayAsync();
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Employees.xlsx");
    }

    private HttpClient CreateClient()
    {
        var client = _httpClientFactory.CreateClient("EmployeeAPI");
        var bearerToken = GetBearerToken();

        if (!string.IsNullOrWhiteSpace(bearerToken))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        }

        return client;
    }

    private string BuildApiUrl(string path, IDictionary<string, string?>? query = null)
    {
        var url = $"{Request.Scheme}://{Request.Host}/{path.TrimStart('/')}";
        if (query == null)
        {
            return url;
        }

        var queryValues = query
            .Where(item => !string.IsNullOrWhiteSpace(item.Value))
            .ToDictionary(item => item.Key, item => item.Value);

        return queryValues.Count == 0 ? url : QueryHelpers.AddQueryString(url, queryValues);
    }

    private async Task<Employee?> GetEmployeeAsync(int id)
    {
        var response = await CreateClient().GetAsync(BuildApiUrl($"api/employee/{id}"));
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Employee>(json, _jsonOptions);
    }

    private static MultipartFormDataContent BuildEmployeeFormData(
        string? firstName,
        string? lastName,
        string? email,
        int age,
        string? imagePath,
        IFormFile? image)
    {
        var formData = new MultipartFormDataContent
        {
            { new StringContent(firstName ?? string.Empty), "FirstName" },
            { new StringContent(lastName ?? string.Empty), "LastName" },
            { new StringContent(email ?? string.Empty), "Email" },
            { new StringContent(age.ToString()), "Age" }
        };

        if (!string.IsNullOrWhiteSpace(imagePath))
        {
            formData.Add(new StringContent(imagePath), "ImagePath");
        }

        if (image != null && image.Length > 0)
        {
            formData.Add(new StreamContent(image.OpenReadStream()), "image", image.FileName);
        }

        return formData;
    }

    private static Employee MapToEmployee(int id, EmployeeUpdateDto dto)
    {
        return new Employee
        {
            Id = id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Age = dto.Age,
            ImagePath = dto.ImagePath
        };
    }

    private string? GetBearerToken()
    {
        var authorizationHeader = Request.Headers.Authorization.ToString();
        if (authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return authorizationHeader["Bearer ".Length..].Trim();
        }

        if (Request.Cookies.TryGetValue("AuthToken", out var cookieToken)
            && !string.IsNullOrWhiteSpace(cookieToken))
        {
            return cookieToken.Trim();
        }

        return null;
    }
}
