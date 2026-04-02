using Microsoft.AspNetCore.Http;
using WEBAPI_DEMO.Models;

namespace WEBAPI_DEMO;

public interface IEmployee
{
    Task<PagedResponse<Employee>> GetAll(int pageNumber, int pageSize);
    Task<Employee?> GetById(int id);
    Task<Employee> Create(Employee employee, IFormFile? image);
    Task<Employee?> Update(int id, Employee employee, IFormFile? image);
    Task<bool> Delete(int id);
}
