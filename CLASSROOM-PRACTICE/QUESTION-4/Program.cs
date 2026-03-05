namespace QUESTION_4;

using System;
using System.Collections.Generic;

/* ================= ABSTRACT CLASS ================= */

public abstract class FoodItem
{
    public string Name { get; set; }
    public double Price { get; set; }

    public FoodItem(string name, double price)
    {
        Name = name;
        Price = price;
    }

    public abstract double GetFinalPrice();
}

public class VegItem : FoodItem
{
    public VegItem(string name, double price) : base(name, price)
    {
    }

    public override double GetFinalPrice()
    {
        return Price;
    }
}

public class NonVegItem : FoodItem
{
    public NonVegItem(string name, double price) : base(name, price)
    {
    }

    public override double GetFinalPrice()
    {
        return Price * 1.12;
    }
}

/* =================================================
   IMPLEMENT BELOW CLASSES

   class VegItem : FoodItem
   class NonVegItem : FoodItem

   Rules:
   - VegItem → no extra charges
   - NonVegItem → add 12% tax
   ================================================= */





class Program
{
    static void Main()
    {
        List<FoodItem> order = new List<FoodItem>
        {
            new VegItem("Paneer", 250),
            new NonVegItem("Chicken", 400),
            new VegItem("Dal", 150)
        };

        double total = 0;

        foreach (var item in order)
        {
            total += item.GetFinalPrice();
        }

        Console.WriteLine("Total Bill: " + total);
    }
}