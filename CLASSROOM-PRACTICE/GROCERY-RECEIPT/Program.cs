/*
This coding question evaluates C#, abstract classes, and object-oriented programming concepts,
ideal for junior-level roles.

The problem requires implementing a GroceryReceipt class to generate invoices
based on item prices, discounts, and customer purchases.
*/

using System;
using System.Collections.Generic;

namespace GROCERY_RECEIPT
{
    public abstract class Receipt
    {
        public abstract void AddItem(string itemName, double price);
        public abstract void ApplyDiscount(double percentage);
        public abstract double GetTotal();
        public abstract void PrintReceipt();
    }

    public class GroceryReceipt : Receipt
    {
        private Dictionary<string, double> items = new Dictionary<string, double>();
        private double discountPercentage = 0;

        public override void AddItem(string itemName, double price)
        {
            // TODO: Add item to dictionary
            items.Add(itemName, price);
        }

        public override void ApplyDiscount(double percentage)
        {
            // TODO: Store discount percentage
            discountPercentage = percentage;
        }

        public override double GetTotal()
        {
            // TODO: Calculate total after discount
            return items.Values.Sum(x => x * (1-(discountPercentage / 100)));
        }

        public override void PrintReceipt()
        {
            // TODO: Print formatted receipt
            foreach (var animal in items)
            {
                Console.WriteLine($"{animal.Key}\t{animal.Value * (1 - discountPercentage/100)}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            GroceryReceipt receipt = new GroceryReceipt();

            receipt.AddItem("Milk", 50);
            receipt.AddItem("Bread", 30);
            receipt.AddItem("Eggs", 60);

            receipt.ApplyDiscount(10);

            receipt.PrintReceipt();

            Console.WriteLine("Final Total: " + receipt.GetTotal());

            Console.ReadLine();
        }
    }
}