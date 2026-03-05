using Code_First_Approach.Data;
using Code_First_Approach.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Code_First_Approach;

public class ProductRepositoryV2 : IProductRepository
{
    private readonly AppDbContext _dbContext;

    public ProductRepositoryV2(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Product>> GetAllAsync()
    {
        return await _dbContext.Products.FromSqlRaw("EXEC GetAllProducts").ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        var Id = new SqlParameter("@Id", id);
        return await _dbContext
            .Products
            .FromSqlRaw("EXEC GetProductById @Id")
            .FirstOrDefaultAsync();
    }

    public async Task<Product?> AddAsync(Product product)
    {
        var parameters = new[]
        {
            new SqlParameter("@Name", product.Name),
            new SqlParameter("@Price", product.Price),
            new SqlParameter("@CategoryId", product.CategoryId),
        };
        
        var insertedProduct = await _dbContext
            .Products
            .FromSqlRaw("EXEC AddProduct @Name, @Price, @CategoryId", parameters)
            .FirstOrDefaultAsync();

        return insertedProduct;
    }

    public async Task UpdateAsync(Product product)
    {
        var updatedProduct = new[]
        {
            new SqlParameter("@Id", product.Id),
            new SqlParameter("@Name", product.Name),
            new SqlParameter("@Price", product.Price),
            new SqlParameter("@CategoryId", product.CategoryId),
        };
        
        await _dbContext
            .Database
            .ExecuteSqlRawAsync("EXEC UpdateProduct @Id, @Name, @Price, @CategoryId", updatedProduct);
    }

    public async Task DeleteAsync(int id)
    {
        var targetId  = new SqlParameter("@Id", id);
        await _dbContext.Database.ExecuteSqlRawAsync("EXEC DeleteProduct @Id", targetId);
    }

    public async Task<List<Product>> GetByCategoryAsync(int categoryId)
    {
        var search = new SqlParameter("@categoryId", categoryId);
        return await _dbContext
            .Products
            .FromSqlRaw("SELECT Id, Name, Price, CateogryId from Products WHERE CategoryId = @categoryId ",
                search)
            .ToListAsync();
    }
}