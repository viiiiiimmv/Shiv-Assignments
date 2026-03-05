using Code_First_Approach;
using Code_First_Approach.Data;
using Code_First_Approach.Models;
using Microsoft.EntityFrameworkCore;

// * Creating context / connection with the database
var context = new AppDbContext();

// * Creating a new category
var electronics = await context
    .Categories
    .FirstOrDefaultAsync(c => c.Name == "Electronics");

// await context.Categories.AddAsync(electronics);
// await context.SaveChangesAsync();

// * Adding data to the 'Product' table
// await context.Products.AddRangeAsync(
//     new Product
//     {
//         Name = "MacBook Air",
//         Price = 100000.00M,
//         category = electronics,
//     },
//     new Product
//     {
//         Name = "Magic Mouse",
//         Price = 12000.00M,
//         category = electronics,
//     }
// );
//
// await context.SaveChangesAsync();
// Console.WriteLine("Data Added Successfully!!");

// * Update command

var macbook = await context.Products.FirstOrDefaultAsync(p => p.Name == "MacBook Air");
// macbook?.Price = 110000.00M;
// await context.SaveChangesAsync();
// Console.WriteLine($"Price update for {macbook?.Name} and updated to {macbook?.Price}");

// * Removing Products
// context.Products.Remove(macbook);


// * Querying Author with Courses

// var authors = await context.Authors.Include(x => x.Courses).ToListAsync();
// foreach (var author in authors)
// {
//     Console.WriteLine($"Author ID : {author.Id} \nAuthor Name : {author.Name}");
//     Console.WriteLine("Courses Taught : ");
//     foreach (var course in author.Courses)
//     {
//         Console.WriteLine($"Course ID : {course.Id}\nCourse Name : {course.Title}\nCourse Description : {course.Description}\n");
//     }
//     Console.WriteLine("*********************************************");
// }

var productRepository = new ProductRepository(context);

// * Adding Products
// var newProduct = new Product {Name = "MacBook Pro", Price = 200000.00M, category = electronics};
// await productRepository.AddAsync(newProduct);
//
// Console.WriteLine("New Product Added Successfully . . .");

// * Fetching Products
// var getMacPro =  await productRepository.GetByIdAsync(1002);
// Console.WriteLine(getMacPro.Name+"\t"+getMacPro.Price);

// await context.Products.AddRangeAsync(
//     new Product
//     {
//         Name = "MacBook Pro 14-inch",
//         Price = 199000.00M,
//         category = electronics,
//     },
//     new Product
//     {
//         Name = "iPad Pro 12.9-inch",
//         Price = 139900.00M,
//         category = electronics,
//     },
//     new Product
//     {
//         Name = "AirPods Pro (2nd Gen)",
//         Price = 24900.00M,
//         category = electronics,
//     }
// );
//
// await context.SaveChangesAsync();
// Console.WriteLine("Data Added Successfully!!");

// * Updating Products
// var productToUpdate =  await productRepository.GetByIdAsync(1);
// productToUpdate!.Name = "MacBook Air M5";
// productToUpdate.Price = 120000.00M;
// await productRepository.UpdateAsync(productToUpdate);
// Console.WriteLine("Product updated successfully");

// * Deleting Products
// await productRepository.DeleteAsync(5);
// Console.WriteLine("Product Deleted");