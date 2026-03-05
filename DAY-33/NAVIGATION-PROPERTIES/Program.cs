namespace NAVIGATION_PROPERTIES;

public  class Customer
    {
        public int CustomerID { set; get; }
        public string FirstName { get; set; }
        public string LastName { set; get; }
        public string EmailAdress { set; get; }

        public List<Invoice> InvoiceList { set; get; } = new List<Invoice>();
    }
   public class Invoice
    {
        public int InvoiceID { set; get; }
        public int CustomerID { set; get; }
        public DateTime InvoiceDate { set; get; }
        public DateTime DueDate { get; set; }
        public bool? IsPaid { set; get; }
        public Decimal Amount { set; get; }
        public Customer Customer { set; get; }
    }

    public class Repository
    {
        public static List<Invoice> RetriveInvoices(int customerid)
        {
            List<Invoice> inv_list = new List<Invoice>
            {
              new Invoice { InvoiceID = 1, CustomerID =101, InvoiceDate = new DateTime(2013, 6, 20),
                  DueDate = new DateTime(2013, 8, 29), IsPaid = true,Amount=34590 },
              new Invoice { InvoiceID = 2, CustomerID =101, InvoiceDate = new DateTime(2012, 6, 20),
                  DueDate = new DateTime(2012, 8, 29), IsPaid = false,Amount=44000 },
              new Invoice { InvoiceID = 3, CustomerID =102, InvoiceDate = new DateTime(2016, 6, 20),
                  DueDate = new DateTime(2016, 8, 29), IsPaid = true,Amount=34590 },
              new Invoice { InvoiceID = 4, CustomerID =103, InvoiceDate = new DateTime(2013, 6, 20),
                  DueDate = new DateTime(2013, 8, 29), IsPaid = false,Amount=45890 },
              new Invoice { InvoiceID = 5, CustomerID =104, InvoiceDate = new DateTime(2010, 6, 20),
                  DueDate = new DateTime(2010, 8, 29), IsPaid = false,Amount=45678 },
              new Invoice { InvoiceID = 6, CustomerID =105, InvoiceDate = new DateTime(2021, 6, 20),
                  DueDate = new DateTime(2021, 8, 29), IsPaid = true,Amount=36590 },
            };

            List<Invoice> filteredList = inv_list.Where(i => i.CustomerID == customerid).ToList();
            return filteredList;
        }
        public static List<Customer> custretrive()
        {
            List<Customer> custlist = new List<Customer>()
            {
                new Customer {CustomerID=101,FirstName="mahesh",LastName="kumar" ,
                 EmailAdress="mahesh@gmail.com",InvoiceList=RetriveInvoices(101)},
                 new Customer {CustomerID=102,FirstName="suresh",LastName="kumar" ,
                 EmailAdress="suresh@gmail.com",InvoiceList=RetriveInvoices(102)},
                 new Customer {CustomerID=103,FirstName="sudha",LastName="kumar" ,
                 EmailAdress="sudha@gmail.com",InvoiceList=RetriveInvoices(103)},
                 new Customer {CustomerID=104,FirstName="suresh",LastName="kumar" ,
                 EmailAdress="suresh@gmaillcom",InvoiceList=RetriveInvoices(104)},
                 new Customer {CustomerID=105,FirstName="rajesh",LastName="kumar" ,
                 EmailAdress="rajesh@gmail.com",InvoiceList=RetriveInvoices(105)}
            };
            return custlist;
        }

    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }