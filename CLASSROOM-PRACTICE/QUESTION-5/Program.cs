namespace QUESTION_5;
using System;
using System.Collections.Generic;

/* ================= INTERFACE ================= */

public interface IProduct
{
    string ProductId { get; }
    string Name { get; set; }
    int Quantity { get; set; }
    double Price { get; set; }
}

public interface IInventory
{
    void AddProduct(IProduct product);
    IProduct GetProduct(string productId);
    List<IProduct> GetLowStockProducts(int threshold);
}

/* =================================================
   IMPLEMENT BELOW CLASSES

   class Product : IProduct
   class Inventory : IInventory

   Requirements:
   - Store products using Dictionary<string, IProduct>
   - Low stock → Quantity < threshold
   ================================================= */
public class Product : IProduct
{
    public string ProductId { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    
    public Product(string productId, string name, int quantity, double price)
    {
        ProductId = productId;
        Name = name;
        Quantity = quantity;
        Price = price;
    }
}

public class Inventory : IInventory
{
    private Dictionary<string, IProduct> inventory = new Dictionary<string, IProduct>();


    public void AddProduct(IProduct product)
    {
        inventory[product.ProductId] = product;
    }

    public IProduct GetProduct(string productId)
    {
        return inventory[productId];
    }

    public List<IProduct> GetLowStockProducts(int threshold)
    {
        return inventory.Values.Where(p => p.Quantity < threshold).ToList();
    }
}

class Program
{
    static void Main()
    {
        IInventory inventory = new Inventory();

        inventory.AddProduct(new Product("P1", "Laptop", 5, 50000));
        inventory.AddProduct(new Product("P2", "Mouse", 50, 800));
        inventory.AddProduct(new Product("P3", "Keyboard", 3, 1500));

        var lowStock = inventory.GetLowStockProducts(10);

        Console.WriteLine("Low Stock Products:");
        foreach (var p in lowStock)
        {
            Console.WriteLine(p.Name);
        }
    }
}