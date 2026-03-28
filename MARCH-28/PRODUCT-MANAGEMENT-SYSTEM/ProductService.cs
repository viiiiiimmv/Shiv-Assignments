using EMPLOYEE_MANAGEMENT_SYSTEM.Data;
using EMPLOYEE_MANAGEMENT_SYSTEM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMPLOYEE_MANAGEMENT_SYSTEM;

public class ProductService : IProduct
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Product>> Get()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> GetbyId(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<Product> Create(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<bool> Update(int id, Product product)
    {
        var existing = await GetbyId(id);
        if (existing == null) return false;

        existing.Name = product.Name?.Trim();
        existing.Price = product.Price;
        existing.Category = product.Category?.Trim();

        _context.Products.Update(existing);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var existing = await GetbyId(id);
        if (existing == null) return false;

        _context.Products.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
