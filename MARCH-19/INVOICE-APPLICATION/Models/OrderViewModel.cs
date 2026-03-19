namespace INVOICE_APPLICATION.Models;

public class OrderViewModel
{
    public int? CustomerId { get; set; }
    public List<Customer> Customers { get; set; } = [];
    public List<Product> Products { get; set; } = [];
    public Dictionary<int, int> Quantities { get; set; } = [];
    public string? ErrorMessage { get; set; }
}
