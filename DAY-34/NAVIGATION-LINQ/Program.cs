using System;
using System.Collections.Generic;
using System.Linq;

namespace NAVIGATION_LINQ
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }

        public List<Invoice> InvoiceList { get; set; } = new();
    }

    public class Invoice
    {
        public int InvoiceID { get; set; }
        public int CustomerID { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsPaid { get; set; }
        public decimal Amount { get; set; }

        public Customer Customer { get; set; }
    }

    public class Repository
    {
        public static List<Invoice> RetrieveInvoices(int customerId)
        {
            var invoices = new List<Invoice>
            {
                new Invoice
                {
                    InvoiceID = 1,
                    CustomerID = 101,
                    InvoiceDate = new DateTime(2013, 6, 20),
                    DueDate = new DateTime(2013, 8, 29),
                    IsPaid = true,
                    Amount = 34590
                },
                new Invoice
                {
                    InvoiceID = 2,
                    CustomerID = 101,
                    InvoiceDate = new DateTime(2012, 6, 20),
                    DueDate = new DateTime(2012, 8, 29),
                    IsPaid = false,
                    Amount = 44000
                },
                new Invoice
                {
                    InvoiceID = 3,
                    CustomerID = 102,
                    InvoiceDate = new DateTime(2016, 6, 20),
                    DueDate = new DateTime(2016, 8, 29),
                    IsPaid = true,
                    Amount = 34590
                },
                new Invoice
                {
                    InvoiceID = 4,
                    CustomerID = 103,
                    InvoiceDate = new DateTime(2013, 6, 20),
                    DueDate = new DateTime(2013, 8, 29),
                    IsPaid = false,
                    Amount = 45890
                },
                new Invoice
                {
                    InvoiceID = 5,
                    CustomerID = 104,
                    InvoiceDate = new DateTime(2010, 6, 20),
                    DueDate = new DateTime(2010, 8, 29),
                    IsPaid = false,
                    Amount = 45678
                },
                new Invoice
                {
                    InvoiceID = 6,
                    CustomerID = 105,
                    InvoiceDate = new DateTime(2021, 6, 20),
                    DueDate = new DateTime(2021, 8, 29),
                    IsPaid = true,
                    Amount = 36590
                }
            };

            return invoices.Where(i => i.CustomerID == customerId).ToList();
        }

        public static List<Customer> RetrieveCustomers()
        {
            var customers = new List<Customer>
            {
                new Customer
                {
                    CustomerID = 101,
                    FirstName = "Mahesh",
                    LastName = "Kumar",
                    EmailAddress = "mahesh@gmail.com"
                },
                new Customer
                {
                    CustomerID = 102,
                    FirstName = "Suresh",
                    LastName = "Kumar",
                    EmailAddress = "suresh@gmail.com"
                },
                new Customer
                {
                    CustomerID = 103,
                    FirstName = "Sudha",
                    LastName = "Kumar",
                    EmailAddress = "sudha@gmail.com"
                },
                new Customer
                {
                    CustomerID = 104,
                    FirstName = "Suresh",
                    LastName = "Kumar",
                    EmailAddress = "suresh@gmail.com"
                },
                new Customer
                {
                    CustomerID = 105,
                    FirstName = "Rajesh",
                    LastName = "Kumar",
                    EmailAddress = "rajesh@gmail.com"
                }
            };

            // Set invoices and navigation property
            foreach (var customer in customers)
            {
                customer.InvoiceList = RetrieveInvoices(customer.CustomerID);

                foreach (var invoice in customer.InvoiceList)
                {
                    invoice.Customer = customer;
                }
            }

            return customers;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var customers = Repository.RetrieveCustomers();

            Console.WriteLine("CUSTOMERS AND THEIR INVOICES");
            Console.WriteLine("=================================\n");

            var customerInvoices = customers
                .Where(c => c.InvoiceList.Any())
                .SelectMany(
                    c => c.InvoiceList,
                    (customer, invoice) => new
                    {
                        Customer = customer,
                        Invoice = invoice
                    })
                .GroupBy(x => x.Customer);

            foreach (var group in customerInvoices)
            {
                var customer = group.Key;

                Console.WriteLine(
                    $"{customer.FirstName} {customer.LastName} has {customer.InvoiceList.Count} invoice(s)");

                foreach (var item in group)
                {
                    Console.WriteLine(
                        $"InvoiceID: {item.Invoice.InvoiceID}, " +
                        $"Amount: {item.Invoice.Amount}, " +
                        $"Paid: {item.Invoice.IsPaid}");
                }

                Console.WriteLine("---------------------------------");
            }
        }
    }
}