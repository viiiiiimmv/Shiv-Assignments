using INVOICE_APPLICATION.Models;
using Microsoft.EntityFrameworkCore;

namespace INVOICE_APPLICATION.Data;

public static class AppDbSeeder
{
    private static readonly Product[] DefaultProducts =
    [
        new Product { Id = 1, Name = "Laptop", Description = "Gaming Laptop", Price = 75000 },
        new Product { Id = 2, Name = "Phone", Description = "Android Smartphone", Price = 25000 },
        new Product { Id = 3, Name = "Headphones", Description = "Noise Cancelling", Price = 5000 },
        new Product { Id = 4, Name = "Keyboard", Description = "Mechanical Keyboard", Price = 3000 },
        new Product { Id = 5, Name = "Mouse", Description = "Wireless Mouse", Price = 1500 }
    ];

    public static async Task InitializeAsync(AppDbContext context)
    {
        // This training database may already contain the app tables without
        // EF migration history, so MigrateAsync would try to recreate them.
        await context.Database.EnsureCreatedAsync();
        await EnsureInvoiceTablesExistAsync(context);

        var existingProductIds = await context.Products
            .Select(product => product.Id)
            .ToListAsync();

        var missingProducts = DefaultProducts
            .Where(product => !existingProductIds.Contains(product.Id))
            .Select(product => new Product
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            })
            .ToList();

        if (missingProducts.Count == 0)
        {
            return;
        }

        context.Products.AddRange(missingProducts);
        await context.SaveChangesAsync();
    }

    private static async Task EnsureInvoiceTablesExistAsync(AppDbContext context)
    {
        await context.Database.ExecuteSqlRawAsync("""
            IF OBJECT_ID(N'[Invoices]', N'U') IS NULL
            BEGIN
                CREATE TABLE [Invoices] (
                    [Id] int NOT NULL IDENTITY,
                    [CustomerId] int NOT NULL,
                    [CreatedOn] datetime2 NOT NULL,
                    [TotalAmount] decimal(18,2) NOT NULL,
                    CONSTRAINT [PK_Invoices] PRIMARY KEY ([Id]),
                    CONSTRAINT [FK_Invoices_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id])
                );

                CREATE INDEX [IX_Invoices_CustomerId] ON [Invoices] ([CustomerId]);
            END
            """);

        await context.Database.ExecuteSqlRawAsync("""
            IF OBJECT_ID(N'[InvoiceLineItems]', N'U') IS NULL
            BEGIN
                CREATE TABLE [InvoiceLineItems] (
                    [Id] int NOT NULL IDENTITY,
                    [InvoiceId] int NOT NULL,
                    [ProductId] int NOT NULL,
                    [ProductName] nvarchar(max) NOT NULL,
                    [Description] nvarchar(max) NOT NULL,
                    [UnitPrice] decimal(18,2) NOT NULL,
                    [Quantity] int NOT NULL,
                    CONSTRAINT [PK_InvoiceLineItems] PRIMARY KEY ([Id]),
                    CONSTRAINT [FK_InvoiceLineItems_Invoices_InvoiceId] FOREIGN KEY ([InvoiceId]) REFERENCES [Invoices] ([Id]) ON DELETE CASCADE
                );

                CREATE INDEX [IX_InvoiceLineItems_InvoiceId] ON [InvoiceLineItems] ([InvoiceId]);
            END
            """);
    }
}
