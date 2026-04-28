using Microsoft.EntityFrameworkCore;
using WEB_API.DTO;
using WEB_API.Models;

namespace WEB_API.Services;

public class EmployeeService : IEmployee
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EmployeeService(
        ApplicationDbContext context,
        IWebHostEnvironment env,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _env = env;
        _httpContextAccessor = httpContextAccessor;
    }

    private string GetBaseUrl()
    {
        var request = _httpContextAccessor.HttpContext!.Request;
        return $"{request.Scheme}://{request.Host}";
    }

    public async Task<List<EmployeeDto>> GetAllEmployeesAsync(int pageNumber, int pageSize)
    {
        var list = await _context.employees
            .OrderBy(e => e.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return list.Select(MapToDto).ToList();
    }

    public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
    {
        var emp = await _context.employees.FindAsync(id);
        return emp == null ? null : MapToDto(emp);
    }

    public async Task<EmployeeDto> AddEmployeeAsync(EmployeeDto dto, IFormFile? image)
    {
        var emp = new Employee
        {
            FirstName = dto.FirstName!,
            LastName = dto.LastName!,
            Email = dto.Email!,
            Age = dto.Age,
            ImagePath = "/uploads/default.jpg"
        };

        if (image != null && image.Length > 0)
            emp.ImagePath = await SaveImage(image);

        await _context.employees.AddAsync(emp);
        await _context.SaveChangesAsync();

        return MapToDto(emp);
    }

    public async Task<EmployeeDto?> UpdateEmployeeAsync(int id, EmployeeUpdateDto dto, IFormFile? image)
    {
        var emp = await _context.employees.FindAsync(id);
        if (emp == null) return null;

        emp.FirstName = dto.FirstName!;
        emp.LastName = dto.LastName!;
        emp.Email = dto.Email!;
        emp.Age = dto.Age;

        if (image != null && image.Length > 0)
        {
            DeleteImage(emp.ImagePath);
            emp.ImagePath = await SaveImage(image);
        }

        await _context.SaveChangesAsync();
        return MapToDto(emp);
    }

    public async Task<EmployeeDto?> DeleteEmployeeAsync(int id)
    {
        var emp = await _context.employees.FindAsync(id);
        if (emp == null) return null;

        DeleteImage(emp.ImagePath);
        _context.employees.Remove(emp);
        await _context.SaveChangesAsync();

        return MapToDto(emp);
    }

    public async Task<List<EmployeeBasicDto>> GetAllEmployeeBasicInfoAsync(int page, int size, string? search)
    {
        var query = _context.employees.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(e =>
                e.FirstName.Contains(search) ||
                e.LastName.Contains(search) ||
                e.Email.Contains(search));
        }

        var list = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        string baseUrl = GetBaseUrl();

        return list.Select(e => new EmployeeBasicDto
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            ImageUrl = string.IsNullOrEmpty(e.ImagePath)
                ? $"{baseUrl}/uploads/default.jpg"
                : $"{baseUrl}{e.ImagePath}"
        }).ToList();
    }

    private EmployeeDto MapToDto(Employee e)
    {
        string baseUrl = GetBaseUrl();

        return new EmployeeDto
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            Age = e.Age,
            ImagePath = string.IsNullOrEmpty(e.ImagePath)
                ? $"{baseUrl}/uploads/default.jpg"
                : $"{baseUrl}{e.ImagePath}"
        };
    }

    private async Task<string> SaveImage(IFormFile image)
    {
        if (_env.WebRootPath == null)
        {
            _env.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }

        var folder = Path.Combine(_env.WebRootPath, "uploads");

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        var name = Guid.NewGuid() + Path.GetExtension(image.FileName);
        var path = Path.Combine(folder, name);

        using var stream = new FileStream(path, FileMode.Create);
        await image.CopyToAsync(stream);

        return $"/uploads/{name}";
    }

    private void DeleteImage(string? path)
    {
        if (string.IsNullOrWhiteSpace(path) || path.Contains("default.jpg"))
            return;

        if (path.StartsWith("http"))
            path = new Uri(path).AbsolutePath;

        var full = Path.Combine(_env.WebRootPath!,
            path.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

        if (File.Exists(full))
            File.Delete(full);
    }
}