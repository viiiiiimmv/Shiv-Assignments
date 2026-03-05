namespace EVENING_1;
using System;
using System.Collections.Generic;
using System.Linq;

abstract class Car
{
    protected bool isSedan;
    protected string seats;

    public Car() { }

    public Car(bool isSedan, string seats)
    {
        this.isSedan = isSedan;
        this.seats = seats;
    }

    public bool getIsSedan()
    {
        return this.isSedan;
    }

    public string getSeats()
    {
        return this.seats;
    }

    abstract public string getMileage();

    public void printCar(string name)
    {
        Console.WriteLine(
            "A {0} is {1} Sedan, is {2}-seater, and has a mileage of around {3}.",
            name,
            this.getIsSedan() ? "" : "not",
            this.getSeats(),
            this.getMileage()
        );
    }
}

// ================= Write your classes below =================

class WagonR : Car
{
    private int mileage;

    public WagonR(int mileage) : base(false, "4")
    {
        this.mileage = mileage;
    }

    public override string getMileage()
    {
        return mileage + " kmpl";
    }
}

// ================= HondaCity Class =================
class HondaCity : Car
{
    private int mileage;

    public HondaCity(int mileage) : base(true, "4")
    {
        this.mileage = mileage;
    }

    public override string getMileage()
    {
        return mileage + " kmpl";
    }
}

// ================= InnovaCrysta Class =================
class InnovaCrysta : Car
{
    private int mileage;

    public InnovaCrysta(int mileage) : base(false, "6")
    {
        this.mileage = mileage;
    }

    public override string getMileage()
    {
        return mileage + " kmpl";
    }
}
// =============================================================

class Solution
{
    static void Main(string[] args)
    {
        int carType = Convert.ToInt32(Console.ReadLine().Trim());
        int carMileage = Convert.ToInt32(Console.ReadLine().Trim());

        if (carType == 0)
        {
            Car wagonR = new WagonR(carMileage);
            wagonR.printCar("WagonR");
        }
        else if (carType == 1)
        {
            Car hondaCity = new HondaCity(carMileage);
            hondaCity.printCar("HondaCity");
        }
        else
        {
            Car innovaCrysta = new InnovaCrysta(carMileage);
            innovaCrysta.printCar("InnovaCrysta");
        }
    }
}