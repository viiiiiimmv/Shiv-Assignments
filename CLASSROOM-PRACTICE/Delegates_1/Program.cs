namespace Delegates_1;
using System;
using System.Collections.Generic;
using System.Linq;

public abstract class Item
{
   public int Id { get; set; }
   public string Name { get; set; }
   public int Price { get; set; }
   public string Category { get; set; }

   public abstract int CalculateFinalPrice();
}

class Weapon : Item
{
   public override int CalculateFinalPrice()
   {
      return (int)(Price * 1.10);
   }
}

class Armor : Item
{
   public override int CalculateFinalPrice()
   {
      return (int)(Price * 1.05);
   }
}

class Potion : Item
{
   public override int CalculateFinalPrice()
   {
      return Price + 2;
   }
}

public delegate double DiscountRule(Item item);

public class Marketplace
{
   private List<Item> _items = new List<Item>();

   public void AddItem(Item item)
   {
      _items.Add(item);
   }

   public double ApplyDiscount(Item item, DiscountRule? drs)
   {
      double finalPrice = item.CalculateFinalPrice();

      if (drs != null)
      {
         foreach (DiscountRule dr in drs.GetInvocationList())
         {
            finalPrice -= dr(item);
         }
      }

      return finalPrice;
   }

   public List<Item> FilterItems(Func<Item, bool> filter)
   {
      return _items.Where(filter).ToList();
   }

   public void ShowMostExpensivePerCategory()
   {
      var results = _items
         .GroupBy(i => i.Category)
         .Select(i => i.OrderByDescending(x => x.Price).First());

      foreach (var result in results)
      {
         Console.WriteLine($"{result.Name}\t{result.Category}\t{result.Price}");
      }
   }

   public void ShowAveragePricePerCategory()
   {
      var results = _items
         .GroupBy(x => x.Category)
         .Select(i => new
         {
            Category = i.Key,
            AvgPrice = i.Average(x => x.Price)
         });

      foreach (var result in results)
      {
         Console.WriteLine($"{result.Category}\t{result.AvgPrice}");
      }
   }

   public void ShowTop3ExpensiveItems()
   {
      var results = _items.OrderByDescending(x => x.Price).Take(3).ToList();
      foreach (var result in results)
      {
         Console.WriteLine($"{result.Name}\t{result.Category}\t{result.Price}");
      }
   }

   public void CountItemsAbovePrice(int price)
   {
      var results = _items.Where(x => x.Price > price).ToList();
   }

   public List<Item> GetItems()
   {
      return _items;
   }
   
}
/*
 STORY:
 The Kingdom of Velmora has an online marketplace.
 Each item belongs to a category and has a base price.
 The King wants dynamic discount rules that can be changed during festivals.

 REQUIREMENTS:
 1. Create an abstract class Item with:
    - Id
    - Name
    - Price
    - Category
    - Abstract method CalculateFinalPrice()

 2. Create derived classes:
    - Weapon
    - Armor
    - Potion

 3. Create a delegate:
    public delegate double DiscountRule(Item item);

 4. Create Marketplace class:
    - List<Item>
    - Method ApplyDiscount(Item item, DiscountRule rule)
    - Support multicast discount rules

 5. Using LINQ:
    - Get most expensive item per category
    - Get average price per category
    - Get top 3 expensive items
    - Count items above given price

 6. Create method:
    List<Item> FilterItems(Func<Item, bool> filter)

 7. Demonstrate everything in Main()
*/

class Program
{
    static void Main()
    {
       Marketplace market = new Marketplace();

       var sword = new Weapon { Id = 1, Name = "Dragon Sword", Price = 120, Category = "Weapon" };
       var axe = new Weapon { Id = 2, Name = "War Axe", Price = 90, Category = "Weapon" };

       var shield = new Armor { Id = 3, Name = "Knight Shield", Price = 80, Category = "Armor" };
       var helmet = new Armor { Id = 4, Name = "Iron Helmet", Price = 60, Category = "Armor" };

       var healPotion = new Potion { Id = 5, Name = "Healing Potion", Price = 20, Category = "Potion" };
       var manaPotion = new Potion { Id = 6, Name = "Mana Potion", Price = 25, Category = "Potion" };

       market.AddItem(sword);
       market.AddItem(axe);
       market.AddItem(shield);
       market.AddItem(helmet);
       market.AddItem(healPotion);
       market.AddItem(manaPotion);

       // Discount rules
       DiscountRule festivalDiscount = item => item.Price * 0.10;
       DiscountRule vipDiscount = item => item.Price * 0.05;

       // Multicast delegate
       DiscountRule combinedDiscount = festivalDiscount + vipDiscount;

       Console.WriteLine("Final Prices After Discounts:\n");

       foreach (var item in market.GetItems())
       {
          double finalPrice = market.ApplyDiscount(item, combinedDiscount);
          Console.WriteLine($"{item.Name} -> {finalPrice}");
       }

       // LINQ operations
       market.ShowMostExpensivePerCategory();
       market.ShowAveragePricePerCategory();
       market.ShowTop3ExpensiveItems();
       market.CountItemsAbovePrice(70);

       // Func filtering
       var expensiveItems = market.FilterItems(i => i.Price > 70);

       Console.WriteLine("\nFiltered Items (Price > 70):");
       foreach (var item in expensiveItems)
          Console.WriteLine(item.Name);
    }
}