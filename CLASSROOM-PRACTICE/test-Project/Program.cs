namespace test_Project;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

abstract class GroceryReceiptBase2
{
    public Dictionary<string, decimal> Prices { get; set; }
    public Dictionary<string, int> Discounts { get; set; }
    public GroceryReceiptBase2(Dictionary<string, decimal> prices, Dictionary<string, int> discounts)
    {
        Prices = prices;
        Discounts = discounts;
    }
    public abstract IEnumerable<(string fruit, decimal price, decimal total)>
        Calculate(List<Tuple<string, int>> shoppingList);
}
class GroceryReceipt2: GroceryReceiptBase2
{
    public GroceryReceipt2(Dictionary<string, decimal> prices, Dictionary<string, int> discounts) : base(prices, discounts)
    {
    }

    public override IEnumerable<(string fruit, decimal price, decimal total)> Calculate(List<Tuple<string, int>> shoppingList)
    {
        for (int i = 0; i < shoppingList.Count; i++)
        {
            
        }

        return null;
    }
}

class Solution2
{
    public static void Main(string[] args)
    {
        List<Tuple<string, int>> boughtItems = new List<Tuple<string, int>>();
        Dictionary<string, decimal> prices = new Dictionary<string, decimal>();
        Dictionary<string, int> discounts = new Dictionary<string, int>();
        int n = Convert.ToInt32(Console.ReadLine().Trim());
        for (int i = 0; i < n; i++)
        {
            var a = Console.ReadLine().Trim().Split(' ');
            prices.Add(a[0], Convert.ToDecimal(a[1]));
        }
        int m = Convert.ToInt32(Console.ReadLine().Trim());
        for (int i = 0; i < m; i++)
        {
            var a = Console.ReadLine().Trim().Split(' ');
            discounts.Add(a[0], Convert.ToInt32(a[1]));
        }
        int b = Convert.ToInt32(Console.ReadLine().Trim());
        for (int i = 0; i < b; i++)
        {
            var a = Console.ReadLine().Trim().Split(' ');
            boughtItems.Add(new Tuple<string, int>(a[0], Convert.ToInt32(a[1])));
        }
        var g = new GroceryReceipt2(prices, discounts);
        var result = g.Calculate(boughtItems);
        foreach (var x in result)
        {
            Console.WriteLine($"{x.fruit} {x.price.ToString("0.0", new NumberFormatInfo() {
                NumberDecimalSeparator = "." })} {x.total.ToString("0.0", new NumberFormatInfo() { NumberDecimalSeparator
                = "." })}");
        }
    }
}