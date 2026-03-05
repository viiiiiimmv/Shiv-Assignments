namespace Delegates_5;

using System;
using System.Collections.Generic;
using System.Linq;

/*
==========================================================
SPACE FLEET MISSION CONTROL SYSTEM
==========================================================

STORY
The Interstellar Command manages a fleet of spacecraft
used for cargo transport, exploration, and combat.

Each mission requires specific ship capabilities such as
fuel levels, crew size, and ship type.

Mission assignments must be dynamic and flexible.

Your task is to implement the system using:

- OOP
- Delegates
- Functional Programming
- LINQ

----------------------------------------------------------
TASKS TO IMPLEMENT
----------------------------------------------------------

1️⃣ Create abstract class SpaceShip

Properties
- int Id
- string Name
- int FuelLevel
- int CrewCount

Method
- abstract bool CanExecuteMission()

RULES
Each ship type should determine whether it can execute
its mission based on its resources.

*/
public abstract class SpaceShip
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int FuelLevel { get; set; }
    public int CrewCount { get; set; }

    public abstract bool CanExecuteMission();
}
/*
----------------------------------------------------------

2️⃣ Create Derived Classes

CargoShip
BattleShip
ExplorerShip

Override CanExecuteMission()

Example Rules (you may follow these)

CargoShip
- FuelLevel >= 50
- CrewCount >= 10

BattleShip
- FuelLevel >= 70
- CrewCount >= 20

ExplorerShip
- FuelLevel >= 60
- CrewCount >= 5

*/
public class CargoShip : SpaceShip
{
    public override bool CanExecuteMission()
    {
        return FuelLevel >= 50 && CrewCount >= 10;
    }
}

public class BattleShip : SpaceShip
{
    public override bool CanExecuteMission()
    {
        return FuelLevel >= 70 && CrewCount >= 20;
    }
}

public class ExplorerShip : SpaceShip
{
    public override bool CanExecuteMission()
    {
        return FuelLevel >= 60 && CrewCount >= 5;
    }
}
/*
----------------------------------------------------------

3️⃣ Create delegate

public delegate bool MissionCriteria(SpaceShip ship);

This delegate allows dynamic mission requirements.
*/
public delegate bool MissionCriteria(SpaceShip ship);

/*

----------------------------------------------------------

4️⃣ Create FleetManager class

Fields
- private List<SpaceShip> ships

Methods to implement

AddShip(SpaceShip ship)

GetShips()

AssignMission(MissionCriteria criteria)

RULES
- Evaluate each ship using the delegate
- If ship satisfies criteria:
    Print
    "Mission assigned to {ship name}"

- Otherwise print
    "{ship name} cannot execute mission"


FilterShips(Func<SpaceShip,bool> filter)

RULES
- Use LINQ Where
- Return filtered ships

*/
public class FleetManager
{
    private List<SpaceShip> _spaceShips = new List<SpaceShip>();

    public void AddShip(SpaceShip spaceShip)
    {
        _spaceShips.Add(spaceShip);
    }

    public List<SpaceShip> GetShips()
    {
        return _spaceShips;
    }

    public void AssignMission(MissionCriteria criteria)
    {
        foreach (var spaceShip in _spaceShips)
        {
            if (criteria(spaceShip))
            {
                Console.WriteLine($"Mission assigned to {spaceShip.Name}");
            }
            else
            {
                Console.WriteLine($"{spaceShip.Name} cannot execute mission");
            }
        }
    }

    public List<SpaceShip> FilterShips(Func<SpaceShip, bool> filter)
    {
        return _spaceShips.Where(filter).ToList();
    }

    public List<SpaceShip> ShipsWithSufficientFuel()
    {
        return _spaceShips.Where(s => s.FuelLevel > 70).ToList();
    }

    public SpaceShip MaximumCrew()
    {
        return _spaceShips.OrderByDescending(s => s.CrewCount).First();
    }

    public List<List<SpaceShip>> GroupShips()
    {
        return _spaceShips.GroupBy(s => s.GetType()).Select(g => g.ToList()).ToList();
    }
}
/*
----------------------------------------------------------

5️⃣ LINQ REPORTS

Implement reports (either inside FleetManager or Main)

A) Ships with FuelLevel > 70

B) Ship with Maximum Crew

C) Group Ships by Type

Example groups
CargoShip
BattleShip
ExplorerShip


----------------------------------------------------------

6️⃣ DO NOT MODIFY MAIN()

Main assumes all required classes and
methods are correctly implemented.

==========================================================
*/

class Program
{
    static void Main()
    {
        FleetManager fleet = new FleetManager();

        //--------------------------------------------------
        // CREATE SHIPS
        //--------------------------------------------------

        SpaceShip s1 = new CargoShip
        {
            Id = 1,
            Name = "Titan Carrier",
            FuelLevel = 80,
            CrewCount = 12
        };

        SpaceShip s2 = new BattleShip
        {
            Id = 2,
            Name = "War Hammer",
            FuelLevel = 90,
            CrewCount = 25
        };

        SpaceShip s3 = new ExplorerShip
        {
            Id = 3,
            Name = "Star Seeker",
            FuelLevel = 65,
            CrewCount = 6
        };

        SpaceShip s4 = new CargoShip
        {
            Id = 4,
            Name = "Orion Transport",
            FuelLevel = 40,
            CrewCount = 8
        };

        SpaceShip s5 = new BattleShip
        {
            Id = 5,
            Name = "Nova Destroyer",
            FuelLevel = 75,
            CrewCount = 30
        };

        fleet.AddShip(s1);
        fleet.AddShip(s2);
        fleet.AddShip(s3);
        fleet.AddShip(s4);
        fleet.AddShip(s5);

        //--------------------------------------------------
        // DEFINE MISSION CRITERIA USING DELEGATE
        //--------------------------------------------------

        MissionCriteria combatMission =
            ship => ship.FuelLevel >= 70 && ship.CrewCount >= 20;

        MissionCriteria explorationMission =
            ship => ship.FuelLevel >= 60;

        //--------------------------------------------------
        // ASSIGN MISSIONS
        //--------------------------------------------------

        Console.WriteLine("\nCOMBAT MISSION ASSIGNMENT\n");

        fleet.AssignMission(combatMission);

        Console.WriteLine("\nEXPLORATION MISSION ASSIGNMENT\n");

        fleet.AssignMission(explorationMission);

        //--------------------------------------------------
        // FILTER SHIPS
        //--------------------------------------------------

        Console.WriteLine("\nSHIPS WITH FUEL > 70\n");

        var highFuelShips = fleet.FilterShips(s => s.FuelLevel > 70);

        foreach (var ship in highFuelShips)
        {
            Console.WriteLine($"{ship.Name} | Fuel: {ship.FuelLevel}");
        }

        //--------------------------------------------------
        // SHIP WITH MAX CREW
        //--------------------------------------------------

        Console.WriteLine("\nSHIP WITH MAX CREW\n");

        var maxCrewShip = fleet
            .GetShips()
            .OrderByDescending(s => s.CrewCount)
            .First();

        Console.WriteLine($"{maxCrewShip.Name} - Crew: {maxCrewShip.CrewCount}");

        //--------------------------------------------------
        // GROUP SHIPS BY TYPE
        //--------------------------------------------------

        Console.WriteLine("\nGROUP SHIPS BY TYPE\n");

        var grouped = fleet
            .GetShips()
            .GroupBy(s => s.GetType().Name);

        foreach (var group in grouped)
        {
            Console.WriteLine($"Type: {group.Key}");

            foreach (var ship in group)
            {
                Console.WriteLine($"   {ship.Name}");
            }
        }
    }
}