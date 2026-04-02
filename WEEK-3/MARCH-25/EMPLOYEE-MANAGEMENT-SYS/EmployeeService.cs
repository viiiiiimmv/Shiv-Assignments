using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using WEBAPI_DEMO;
using WEBAPI_DEMO.Data;
using WEBAPI_DEMO.Models;

public class EmployeeService : IEmployee
{
    private static readonly string[] AllowedImageExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];
    private const long MaxImageSizeBytes = 5 * 1024 * 1024;

    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public EmployeeService(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<PagedResponse<Employee>> GetAll(int pageNumber, int pageSize)
    {
        pageNumber = Math.Max(pageNumber, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var query = _context.Employees
            .AsNoTracking()
            .OrderByDescending(employee => employee.Id);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResponse<Employee>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<Employee?> GetById(int id)
    {
        return await _context.Employees
            .AsNoTracking()
            .SingleOrDefaultAsync(employee => employee.Id == id);
    }

    public async Task<Employee> Create(Employee employee, IFormFile? image)
    {
        employee.ImagePath = await SaveImageAsync(image);
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task<Employee?> Update(int id, Employee employee, IFormFile? image)
    {
        var existing = await _context.Employees.FindAsync(id);
        if (existing == null) return null;

        existing.FirstName = employee.FirstName;
        existing.LastName = employee.LastName;
        existing.Email = employee.Email;
        existing.Age = employee.Age;
        existing.ImagePath = await SaveImageAsync(image, existing.ImagePath);

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> Delete(int id)
    {
        var emp = await _context.Employees.FindAsync(id);
        if (emp == null) return false;

        DeleteImage(emp.ImagePath);
        _context.Employees.Remove(emp);
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<string?> SaveImageAsync(IFormFile? image, string? currentImagePath = null)
    {
        if (image == null || image.Length == 0)
        {
            return currentImagePath;
        }

        var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
        if (!AllowedImageExtensions.Contains(extension))
        {
            throw new InvalidOperationException("Only JPG, PNG, GIF, and WEBP images are allowed.");
        }

        if (image.Length > MaxImageSizeBytes)
        {
            throw new InvalidOperationException("Image size must be 5 MB or smaller.");
        }

        var webRootPath = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var uploadsDirectory = Path.Combine(webRootPath, "uploads", "employees");
        Directory.CreateDirectory(uploadsDirectory);

        var fileName = $"{Guid.NewGuid():N}{extension}";
        var filePath = Path.Combine(uploadsDirectory, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        DeleteImage(currentImagePath);
        return $"/uploads/employees/{fileName}";
    }

    private void DeleteImage(string? imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath) || !imagePath.StartsWith("/uploads/employees/", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var webRootPath = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var relativePath = imagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        var filePath = Path.Combine(webRootPath, relativePath);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}
