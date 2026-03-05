/*
This coding question evaluates LINQ and data analysis.

Requirements:

1. Create a Car class with:
     - Brand
     - Model
     - Price
2. Create CarManager class that:
     - Adds cars
     - Finds most expensive car
     - Finds least expensive car
     - Calculates average price
3. Use LINQ where appropriate.
4. Demonstrate functionality in Main().
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace CarManagement
{
     public class Car
     {
          public string Brand { get; set; }
          public string Model { get; set; }
          public double Price { get; set; }

          public Car(string brand, string model, double price)
          {
               Brand = brand;
               Model = model;
               Price = price;
          }
     }

     public class CarManager
     {
          private List<Car> cars = new List<Car>();

          public void AddCar(Car car)
          {
               // TODO
               cars.Add(car);
          }

          public Car? GetMostExpensiveCar()
          {
               // TODO
               return cars.OrderByDescending(x => x.Price).FirstOrDefault();
          }

          public Car? GetLeastExpensiveCar()
          {
               // TODO
               return cars.OrderBy(x => x.Price).FirstOrDefault();
          }

          public double GetAveragePrice()
          {
               // TODO
               return cars.Average(x=>x.Price);
          }
     }

     class Program
     {
          static void Main(string[] args)
          {
               CarManager manager = new CarManager();

               manager.AddCar(new Car("BMW", "X5", 7000000));
               manager.AddCar(new Car("Audi", "A4", 5500000));
               manager.AddCar(new Car("Hyundai", "i20", 900000));

               Console.WriteLine("Average Price: " + manager.GetAveragePrice());
               Console.WriteLine("Least Expensive Car: " + manager.GetLeastExpensiveCar());
               Console.WriteLine("Most Expensive Car: " + manager.GetMostExpensiveCar());

               Console.ReadLine();
          }
     }
}