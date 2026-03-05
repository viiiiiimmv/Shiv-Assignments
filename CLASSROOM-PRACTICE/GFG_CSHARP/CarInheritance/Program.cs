/*
This coding question evaluates inheritance and polymorphism.

Requirements:

1. Create an abstract base class Car.
2. Include properties like Brand and Model.
3. Add an abstract method StartEngine().
4. Create derived classes:
      - ElectricCar
      - PetrolCar
5. Override StartEngine() differently in each.
6. Demonstrate polymorphism in Main().
*/

using System;

namespace CarInheritance
{
      public abstract class Car
      {
            public string Brand { get; set; }
            public string Model { get; set; }

            protected Car(string brand, string model)
            {
                  Brand = brand;
                  Model = model;
            }

            public abstract void StartEngine();
      }

      public class ElectricCar : Car
      {
            public ElectricCar(string brand, string model) : base(brand, model) { }

            public override void StartEngine()
            {
                  // TODO
                  Console.WriteLine($"{Brand} {Model} starts electrically ...");
            }
      }

      public class PetrolCar : Car
      {
            public PetrolCar(string brand, string model) : base(brand, model) { }

            public override void StartEngine()
            {
                  // TODO
                  Console.WriteLine($"{Brand} {Model} starts on petrol ...");
            }
      }

      class Program
      {
            static void Main(string[] args)
            {
                  Car car1 = new ElectricCar("Tesla", "Model 3");
                  Car car2 = new PetrolCar("Toyota", "Corolla");

                  car1.StartEngine();
                  car2.StartEngine();

                  Console.ReadLine();
            }
      }
}