using System;
using System.Collections.Generic;

/* ================= ABSTRACT CLASS ================= */

public abstract class Vehicle
{
    public string VehicleNumber { get; set; }
    public double RatePerDay { get; set; }

    public Vehicle(string number, double rate)
    {
        VehicleNumber = number;
        RatePerDay = rate;
    }

    public abstract double CalculateRent(int days);
}

/* =================================================
   IMPLEMENT BELOW CLASSES

   class Car : Vehicle
   class Bike : Vehicle

   Rules:
   - Car rent = RatePerDay * days
   - Bike rent = (RatePerDay * days) - 10% discount
   ================================================= */
public class Car : Vehicle
{
    public Car(string number, double rate) : base(number, rate){}

    public override double CalculateRent(int days)
    {
        return days * RatePerDay;
    }
}

public class Bike : Vehicle
{
    public Bike(string number, double rate) : base(number, rate){}

    public override double CalculateRent(int days)
    {
        return days * RatePerDay;
    }
}

class Program
{
    static void Main()
    {
        List<Vehicle> vehicles = new List<Vehicle>
        {
            new Car("CAR101", 2000),
            new Bike("BIKE201", 800),
            new Car("CAR102", 2500)
        };

        foreach (var v in vehicles)
        {
            Console.WriteLine($"{v.VehicleNumber} Rent for 3 days: {v.CalculateRent(3)}");
        }
    }
}