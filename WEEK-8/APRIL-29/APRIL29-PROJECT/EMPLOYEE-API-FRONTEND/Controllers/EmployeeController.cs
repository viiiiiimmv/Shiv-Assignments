using EMPLOYEE_API_FRONTEND.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace EMPLOYEE_API_FRONTEND.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EmployeeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient GetClient()
        {
            return _httpClientFactory.CreateClient("EmployeeApi");
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 5, string? search = null)
        {
            var client = GetClient();

            string url = string.IsNullOrWhiteSpace(search)
                ? $"Emp/basic?page={page}&pageSize={pageSize}"
                : $"Emp/basic?page={page}&pageSize={pageSize}&search={search}";

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Unable to load employee data.";
                return View(new List<EmployeeBasicDto>());
            }

            var json = await response.Content.ReadAsStringAsync();

            var employees = JsonSerializer.Deserialize<List<EmployeeBasicDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.Search = search;

            return View(employees ?? new List<EmployeeBasicDto>());
        }

        public async Task<IActionResult> Details(int id)
        {
            var client = GetClient();

            var response = await client.GetAsync($"Emp/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var json = await response.Content.ReadAsStringAsync();

            var employee = JsonSerializer.Deserialize<EmployeeDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(employee);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeDto employeeDto, IFormFile? image)
        {
            if (!ModelState.IsValid)
            {
                return View(employeeDto);
            }

            var client = GetClient();

            using var formData = new MultipartFormDataContent();

            formData.Add(new StringContent(employeeDto.FirstName ?? ""), "FirstName");
            formData.Add(new StringContent(employeeDto.LastName ?? ""), "LastName");
            formData.Add(new StringContent(employeeDto.Email ?? ""), "Email");
            formData.Add(new StringContent(employeeDto.Age.ToString()), "Age");

            if (image != null && image.Length > 0)
            {
                var imageContent = new StreamContent(image.OpenReadStream());
                imageContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
                formData.Add(imageContent, "image", image.FileName);
            }

            var response = await client.PostAsync("Emp", formData);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error = "Unable to add employee.";
            return View(employeeDto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var client = GetClient();

            var response = await client.GetAsync($"Emp/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var json = await response.Content.ReadAsStringAsync();

            var employee = JsonSerializer.Deserialize<EmployeeDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (employee == null)
            {
                return NotFound();
            }

            var updateDto = new EmployeeUpdateDto
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Age = employee.Age,
                ImagePath = employee.ImagePath
            };

            ViewBag.EmployeeId = id;

            return View(updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EmployeeUpdateDto employeeDto, IFormFile? image)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.EmployeeId = id;
                return View(employeeDto);
            }

            var client = GetClient();

            using var formData = new MultipartFormDataContent();

            formData.Add(new StringContent(employeeDto.FirstName ?? ""), "FirstName");
            formData.Add(new StringContent(employeeDto.LastName ?? ""), "LastName");
            formData.Add(new StringContent(employeeDto.Email ?? ""), "Email");
            formData.Add(new StringContent(employeeDto.Age.ToString()), "Age");

            if (!string.IsNullOrWhiteSpace(employeeDto.ImagePath))
            {
                formData.Add(new StringContent(employeeDto.ImagePath), "ImagePath");
            }

            if (image != null && image.Length > 0)
            {
                var imageContent = new StreamContent(image.OpenReadStream());
                imageContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
                formData.Add(imageContent, "image", image.FileName);
            }

            var response = await client.PutAsync($"Emp/{id}", formData);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.EmployeeId = id;
            ViewBag.Error = "Unable to update employee.";
            return View(employeeDto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var client = GetClient();

            var response = await client.GetAsync($"Emp/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var json = await response.Content.ReadAsStringAsync();

            var employee = JsonSerializer.Deserialize<EmployeeDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(employee);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = GetClient();

            var response = await client.DeleteAsync($"Emp/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error = "Unable to delete employee.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Export(string? search = null)
        {
            var client = GetClient();

            string url = string.IsNullOrWhiteSpace(search)
                ? "Emp/export/excel"
                : $"Emp/export/excel?search={search}";

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Unable to export employee data.";
                return RedirectToAction(nameof(Index));
            }

            var fileBytes = await response.Content.ReadAsByteArrayAsync();

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Employees.xlsx"
            );
        }
    }
}
