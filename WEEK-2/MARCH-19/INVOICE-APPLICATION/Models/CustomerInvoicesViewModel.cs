namespace INVOICE_APPLICATION.Models;

public class CustomerInvoicesViewModel
{
    public Customer Customer { get; set; } = new();
    public List<Invoice> Invoices { get; set; } = [];
}
