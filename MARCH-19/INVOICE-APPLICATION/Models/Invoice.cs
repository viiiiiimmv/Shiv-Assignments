using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INVOICE_APPLICATION.Models;

public class Invoice
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public Customer Customer { get; set; } = new();

    public List<InvoiceLineItem> Items { get; set; } = [];

    public DateTime CreatedOn { get; set; } = DateTime.Now;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [NotMapped]
    public decimal GrandTotal => Items.Count > 0 ? Items.Sum(item => item.LineTotal) : TotalAmount;
}
