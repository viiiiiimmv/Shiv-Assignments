using DB_FIRST_SINGLE_CRUD.Models;
using Microsoft.EntityFrameworkCore;

namespace DB_FIRST_SINGLE_CRUD.Repositories;

public class CustomProductRepository : ICustomProduct
{
    private readonly NorthWindContext _context;

    public CustomProductRepository(NorthWindContext context)
    {
        _context = context;
    }
    
    public async Task<List<CustomProduct>> GetAllAsync()
    {
       return await _context.CustomProducts.ToListAsync();
    }

    public async Task<CustomProduct?> GetByIdAsync(int id)
    {
        return await _context.CustomProducts.Where(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<CustomProduct> AddAsync(CustomProduct product)
    {
        await _context.CustomProducts.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }
    
    public async Task DeleteAsync(int id)
    {
        var product = await _context.CustomProducts.Where(p => p.Id == id).FirstOrDefaultAsync();
        if (product != null) _context.CustomProducts.Remove(product);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}