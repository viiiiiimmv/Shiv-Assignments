using DB_FIRST_SINGLE_CRUD.Models;
using DB_FIRST_SINGLE_CRUD.Repositories;

namespace DB_FIRST_SINGLE_CRUD;

class Program
{
    static async Task Main(string[] args)
    {
        var newProduct = new CustomProduct
        {
            Name = "super-widget",
            Price = 89.50M,
            Stock = 10
        };
        
        NorthWindContext _context = new NorthWindContext();
        CustomProductRepository productRepository = new CustomProductRepository(_context);
        
        // * Adding a new product
        await productRepository.AddAsync(newProduct);
        
        // * Fetching an existing product
        var getProduct = await productRepository.GetByIdAsync(3);
        Console.WriteLine(getProduct!.Name);
        Console.WriteLine(getProduct.Price);
        Console.WriteLine(getProduct.Stock);
        
        // * Updating an existing product;
        var product = await productRepository.GetByIdAsync(3);
        
        product!.Name = "super-widget-8";
        product.Price = 89.50M;
        product.Stock = 15;
        
        await productRepository.SaveChangesAsync();
        
        var product2 =  await productRepository.GetByIdAsync(3);
        Console.WriteLine(product2!.Name);
        Console.WriteLine(product2.Price);
        
        // * Deleting a product
        await productRepository.DeleteAsync(3);

    }
}