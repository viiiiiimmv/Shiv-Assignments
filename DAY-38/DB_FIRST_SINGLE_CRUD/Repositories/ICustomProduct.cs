namespace DB_FIRST_SINGLE_CRUD.Repositories;
using Models;

public interface ICustomProduct
{
    Task<List<CustomProduct>> GetAllAsync();
    Task<CustomProduct?> GetByIdAsync(int id);
    Task<CustomProduct> AddAsync(CustomProduct product);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}