using EMPLOYEE_MANAGEMENT_SYSTEM.Models;
using Microsoft.AspNetCore.Mvc;

namespace EMPLOYEE_MANAGEMENT_SYSTEM;

public interface IProduct
{
    Task<List<Product>> Get();
    Task<Product?> GetbyId(int id);
    Task<Product> Create(Product product);
    Task<bool> Update(int id, Product product);
    Task<bool> Delete(int id);
}