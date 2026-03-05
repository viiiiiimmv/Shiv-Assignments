using Code_First_Approach.Data;
using Code_First_Approach.Models;
using Microsoft.EntityFrameworkCore;

namespace Code_First_Approach;

public class ProductRepository : IProductRepository
{
    
    private readonly AppDbContext _dbContext;

    public ProductRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Product>> GetAllAsync()
    {
        return await _dbContext.Products.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _dbContext.Products.FindAsync(id);
    }

    public async Task<Product?> AddAsync(Product product)
    {
        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();
        return product;
    }

    public async Task UpdateAsync(Product product)
    {
        // * Approach - 1
        var productToUpdate = await GetByIdAsync(product.Id);
        productToUpdate!.Name = product.Name;
        productToUpdate.Price = product.Price;
        productToUpdate.CategoryId = product.CategoryId;
        await _dbContext.SaveChangesAsync();

        // * Approach - 2
        // _dbContext.Products.Update(product);
        // await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var productToDelete = await GetByIdAsync(id);
        _dbContext.Products.Remove(productToDelete!);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Product>> GetByCategoryAsync(int categoryId)
    {
        return await _dbContext.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
    }
}