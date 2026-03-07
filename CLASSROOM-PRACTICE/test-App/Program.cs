namespace test_App;

using System;
using System.Collections.Generic;
using System.Linq;

/*
==========================================================
SMART LOGISTICS DISPATCH SYSTEM
==========================================================

STORY
A global logistics company manages thousands of delivery
vehicles across different regions.

Packages must be assigned dynamically depending on:
- vehicle capacity
- fuel level
- region
- dispatch rules

Dispatch rules must be flexible and defined using delegates.

Your task is to design the dispatch system using:
- OOP
- Delegates
- LINQ
- Dynamic rule engines

----------------------------------------------------------
TASKS TO IMPLEMENT
----------------------------------------------------------

1️⃣ Create abstract class Vehicle

Properties
- int VehicleId
- string Name
- int Capacity
- int FuelLevel
- string Region

Method

abstract bool CanDeliver(Package p)

RULES
This method determines if a vehicle can carry the package.


----------------------------------------------------------

2️⃣ Create derived classes

Truck
Drone
Bike

Override CanDeliver()

Rules

Truck
Capacity >= package weight

Drone
Weight <= 5

Bike
Weight <= 20


----------------------------------------------------------

3️⃣ Create class Package

Properties

- int PackageId
- int Weight
- int Priority
- string DestinationRegion


----------------------------------------------------------

4️⃣ Create delegate

public delegate bool DispatchRule(Vehicle v, Package p);

This delegate represents dynamic dispatch eligibility.


----------------------------------------------------------

5️⃣ Create DispatchManager class

Fields

private List<Vehicle> vehicles
private List<Package> packages


Methods to implement

AddVehicle(Vehicle v)

AddPackage(Package p)

GetVehicles()

GetPackages()


AssignPackages(DispatchRule rule)

RULES
- For each package, find vehicles that satisfy the rule
- Assign the package to the first eligible vehicle
- Print assignment result

Example output

Package 101 assigned to Heavy Truck

or

Package 102 could not be assigned


FilterVehicles(Func<Vehicle,bool> filter)

RULES
Use LINQ Where
Return filtered vehicles


----------------------------------------------------------

6️⃣ LINQ REPORT REQUIREMENTS

Implement queries in Main (already provided).

A) Vehicles with fuel > 60

B) Total capacity per region

Group vehicles by region
Sum capacities


C) Vehicle with maximum capacity

D) Top 5 high priority packages


----------------------------------------------------------

7️⃣ BONUS (Hard)

Create delegate rules

fuelCheck
regionCheck
capacityCheck

Combine them dynamically in Main.


----------------------------------------------------------

8️⃣ DO NOT MODIFY MAIN()

Main already contains the test runner that will validate
your implementation.

==========================================================
*/

public abstract class Vehicle
{
    public int VehicleId { get; set; }
    public string Name { get; set; }
    public int Capacity { get; set; }
    public int FuelLevel { get; set; }
    public string Region { get; set; }

    // TODO: implement in derived classes
    public abstract bool CanDeliver(Package p);
}

class Truck : Vehicle
{
    // TODO: override CanDeliver
    public override bool CanDeliver(Package p)
    {
        return Capacity >= p.Weight;
    }
}

class Drone : Vehicle
{
    // TODO: override CanDeliver
    public override bool CanDeliver(Package p)
    {
        return p.Weight < 5;
    }
}

class Bike : Vehicle
{
    // TODO: override CanDeliver
    public override bool CanDeliver(Package p)
    {
        return p.Weight < 20;
    }
}

public class Package
{
    public int PackageId { get; set; }
    public int Weight { get; set; }
    public int Priority { get; set; }
    public string DestinationRegion { get; set; }
}

public delegate bool DispatchRule(Vehicle v, Package p);

class DispatchManager
{
    private List<Vehicle> vehicles = new List<Vehicle>();
    private List<Package> packages = new List<Package>();

    // TODO
    public void AddVehicle(Vehicle v)
    {
        vehicles.Add(v);
    }

    // TODO
    public void AddPackage(Package p)
    {
        packages.Add(p);
    }

    // TODO
    public List<Vehicle> GetVehicles()
    {
        return vehicles.ToList();
    }

    // TODO
    public List<Package> GetPackages()
    {
        return packages.ToList();
    }

    // TODO
    public void AssignPackages(DispatchRule rule)
    {
        foreach (var p in packages)
        {
            bool assigned = false;
            foreach (var v in vehicles)
            {
                if (rule(v, p))
                {
                    Console.WriteLine($"Package {p.PackageId} assigned to {v.Name}");
                    assigned = true;
                    break;
                }
            }

            if(!assigned)
            {
                Console.WriteLine($"Package {p.PackageId} could not be assigned");
            }
        }
    }

    // TODO
    public List<Vehicle> FilterVehicles(Func<Vehicle, bool> filter)
    {
        return vehicles.Where(filter).ToList();
    }
}

class Program
{
    static void Main()
    {
        DispatchManager manager = new DispatchManager();

        //--------------------------------------------------
        // CREATE VEHICLES
        //--------------------------------------------------

        Vehicle v1 = new Truck
        {
            VehicleId = 1,
            Name = "Heavy Truck",
            Capacity = 100,
            FuelLevel = 80,
            Region = "North"
        };

        Vehicle v2 = new Drone
        {
            VehicleId = 2,
            Name = "Delivery Drone",
            Capacity = 5,
            FuelLevel = 70,
            Region = "North"
        };

        Vehicle v3 = new Bike
        {
            VehicleId = 3,
            Name = "Courier Bike",
            Capacity = 20,
            FuelLevel = 50,
            Region = "South"
        };

        Vehicle v4 = new Truck
        {
            VehicleId = 4,
            Name = "Regional Truck",
            Capacity = 120,
            FuelLevel = 90,
            Region = "South"
        };

        manager.AddVehicle(v1);
        manager.AddVehicle(v2);
        manager.AddVehicle(v3);
        manager.AddVehicle(v4);

        //--------------------------------------------------
        // CREATE PACKAGES
        //--------------------------------------------------

        Package p1 = new Package
        {
            PackageId = 101,
            Weight = 4,
            Priority = 8,
            DestinationRegion = "North"
        };

        Package p2 = new Package
        {
            PackageId = 102,
            Weight = 15,
            Priority = 6,
            DestinationRegion = "South"
        };

        Package p3 = new Package
        {
            PackageId = 103,
            Weight = 60,
            Priority = 9,
            DestinationRegion = "North"
        };

        Package p4 = new Package
        {
            PackageId = 104,
            Weight = 3,
            Priority = 7,
            DestinationRegion = "South"
        };

        manager.AddPackage(p1);
        manager.AddPackage(p2);
        manager.AddPackage(p3);
        manager.AddPackage(p4);

        //--------------------------------------------------
        // DEFINE DISPATCH RULES
        //--------------------------------------------------

        DispatchRule fuelCheck =
            (v, p) => v.FuelLevel > 40;

        DispatchRule regionCheck =
            (v, p) => v.Region == p.DestinationRegion;

        DispatchRule capacityCheck =
            (v, p) => v.CanDeliver(p);

        DispatchRule combinedRule =
            (v, p) => fuelCheck(v, p) && regionCheck(v, p) && capacityCheck(v, p);

        //--------------------------------------------------
        // ASSIGN PACKAGES
        //--------------------------------------------------

        Console.WriteLine("\nPACKAGE ASSIGNMENTS\n");

        manager.AssignPackages(combinedRule);

        //--------------------------------------------------
        // FILTER VEHICLES
        //--------------------------------------------------

        Console.WriteLine("\nVEHICLES WITH FUEL > 60\n");

        var highFuel = manager.FilterVehicles(v => v.FuelLevel > 60);

        foreach (var v in highFuel)
        {
            Console.WriteLine($"{v.Name} | Fuel {v.FuelLevel}");
        }

        //--------------------------------------------------
        // LINQ REPORTS
        //--------------------------------------------------

        Console.WriteLine("\nTOTAL CAPACITY PER REGION\n");

        var capacityPerRegion = manager
            .GetVehicles()
            .GroupBy(v => v.Region)
            .Select(g => new
            {
                Region = g.Key,
                TotalCapacity = g.Sum(x => x.Capacity)
            });

        foreach (var r in capacityPerRegion)
        {
            Console.WriteLine($"{r.Region} -> {r.TotalCapacity}");
        }

        //--------------------------------------------------

        Console.WriteLine("\nVEHICLE WITH MAX CAPACITY\n");

        var maxVehicle = manager
            .GetVehicles()
            .OrderByDescending(v => v.Capacity)
            .First();

        Console.WriteLine($"{maxVehicle.Name} - {maxVehicle.Capacity}");

        //--------------------------------------------------

        Console.WriteLine("\nTOP PRIORITY PACKAGES\n");

        var topPackages = manager
            .GetPackages()
            .OrderByDescending(p => p.Priority)
            .Take(5);

        foreach (var p in topPackages)
        {
            Console.WriteLine($"Package {p.PackageId} Priority {p.Priority}");
        }
    }
}