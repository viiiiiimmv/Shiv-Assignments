using ENTITY_FRAMEWORK_INTRO.Models;
namespace ENTITY_FRAMEWORK_INTRO;

class Program
{
    private static void Main()
    {
        var northWindUtilities = new NorthWindContext();

        BasicQueries_DataFirst(northWindUtilities);
        AlteringFetchedData(northWindUtilities);
        NameBasedFiltering(northWindUtilities);
        BeverageProducts(northWindUtilities);
        CustomersWithMoreThanTenOrders(northWindUtilities);
    }

    private static void CustomersWithMoreThanTenOrders(NorthWindContext northWindUtilities)
    {
        var customersWithMoreThanTenOrders = northWindUtilities
            .Customers
            .Where(x => x.Orders.Count > 10)
            .Select(x => new
            {
                customerName = x.ContactName,
                orderCount = x.Orders.Count
            });

        foreach (var customer in customersWithMoreThanTenOrders)
        {
            Console.WriteLine($"{customer.customerName} has placed {customer.orderCount} orders.");
        }
    }

    private static void BeverageProducts(NorthWindContext northWindUtilities)
    {
        var beverageProducts = northWindUtilities
            .Products
            .Where(x => x.Category!.CategoryName == "Beverages" || x.Category!.CategoryName == "Seafood")
            .Select(x => new
            {
                productName = x.ProductName,
                productCategory = x.Category!.CategoryName
            });
        
        Console.WriteLine("----- BEVERAGES & SEAFOOD ----");
        var count = 0;
        foreach (var beverageProduct in beverageProducts)
        { 
            Console.WriteLine($"{++count}\t{beverageProduct.productName}\t{beverageProduct.productCategory}");
        }
    }

    private static void NameBasedFiltering(NorthWindContext northWindUtilities)
    {
        var containsName = northWindUtilities
            .Customers
            .Select(x => x)
            .Where(x => x.ContactName!.Contains("carlos"))
            .ToList();

        var count = 0;
        foreach (var customer in containsName)
        {
            Console.WriteLine($"{++count}.\t{customer.ContactName}\t{customer.CompanyName}\t{customer.City}");
        }
    }

    private static void AlteringFetchedData(NorthWindContext northWindUtilities)
    {
        var categoryAbbreviations = northWindUtilities
            .Categories
            .Select(x => x.CategoryName.Substring(0, 3).ToUpper())
            .ToList();
        
        var catAbbreviations =
            from category in northWindUtilities.Categories
            select new
            {
                catAbbreviation = category.CategoryName.Substring(0, 3)
            };
        
        Console.WriteLine("---- CATEGORIES ----");
        foreach (var categoryAbbreviation in categoryAbbreviations)
        {
            Console.WriteLine($"{categoryAbbreviation}");
        }
        
        Console.WriteLine("\n---- CATEGORIES ----");
        foreach (var category in catAbbreviations)
        {
            Console.WriteLine($"{category}");
        }
    }

    private static void BasicQueries_DataFirst(NorthWindContext northWindUtilities)
    {
        // * select * from [dbo].[Customers] where Country = 'Spain'
        
        // ! Query Syntax
        var customersFromSpain = from customer in northWindUtilities.Customers
            where customer.Country == "Spain"
            select new
            {
                customer.CompanyName,
                customer.Country
            };
        
        // ! Method Syntax
        var customersFromUsa = northWindUtilities
            .Customers
            .Where(x => x.Country == "USA")
            .Select(x => new {cCompany = x.CompanyName, cCountry = x.Country});

        Console.WriteLine("---- CUSTOMERS FROM SPAIN ----");
        int count = 0;
        foreach (var customer in customersFromSpain)
        {
            Console.WriteLine($"{++count}.\t{customer.CompanyName}");
        }
        Console.WriteLine("------------------------------\n");
        
        Console.WriteLine("---- CUSTOMERS FROM USA ----");
        count = 0;
        foreach (var customer in customersFromUsa)
        {
            Console.WriteLine($"{++count}.\t{customer.cCompany}");
        }
        Console.WriteLine("------------------------------\n");
    }
    
    
    
}