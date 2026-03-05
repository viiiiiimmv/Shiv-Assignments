/*
This coding question evaluates inheritance, interfaces, and C# concepts,
ideal for junior-level roles.

The problem requires implementing a customer order reporting system
with specific methods for managing and retrieving order data.
*/
using System;
using System.Collections.Generic;
using System.Linq;

namespace CUSTOMER_ORDERS
{
    public interface IOrder
    {
        int OrderId { get; }
        string CustomerName { get; }
        double Amount { get; }
    }

    public class Order : IOrder
    {
        public int OrderId { get; private set; }
        public string CustomerName { get; private set; }
        public double Amount { get; private set; }

        public Order(int id, string customer, double amount)
        {
            OrderId = id;
            CustomerName = customer;
            Amount = amount;
        }
    }

    public class OrderManager
    {
        private List<IOrder> orders = new List<IOrder>();

        public void AddOrder(IOrder order)
        {
            // TODO: Add order
            orders.Add(order);
        }

        public void RemoveOrder(int orderId)
        {
            // TODO: Remove order by ID
            orders.RemoveAll(ord => ord.OrderId == orderId);
        }

        public double GetTotalRevenue()
        {
            // TODO: Calculate total revenue
            return orders.Sum(o => o.Amount);
        }

        public List<IOrder> GetOrdersByCustomer(string customerName)
        {
            // TODO: Return orders for given customer
            return orders.Where(o=> o.CustomerName == customerName).ToList();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            OrderManager manager = new OrderManager();

            manager.AddOrder(new Order(1, "Rahul", 500));
            manager.AddOrder(new Order(2, "Aman", 1200));
            manager.AddOrder(new Order(3, "Rahul", 300));

            Console.WriteLine("Total Revenue: " + manager.GetTotalRevenue());

            var rahulOrders = manager.GetOrdersByCustomer("Rahul");

            Console.WriteLine("Rahul's Orders:");
            foreach (var order in rahulOrders)
            {
                Console.WriteLine($"OrderId: {order.OrderId}, Amount: {order.Amount}");
            }

            Console.ReadLine();
        }
    }
}