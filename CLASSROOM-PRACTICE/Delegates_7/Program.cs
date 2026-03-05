namespace Delegates_7;

using System;
using System.Collections.Generic;
using System.Linq;

/*
==========================================================
STREAMING PLATFORM SUBSCRIPTION SYSTEM
==========================================================

STORY
A streaming platform manages multiple subscription tiers
such as Basic, Premium, and Family plans.

Pricing strategies change frequently due to promotions,
regional discounts, and loyalty programs.

The system must support dynamic pricing rules and provide
analytics about subscriptions and revenue.

You must design the system using:

- OOP
- Delegates
- Functional Programming
- LINQ analytics

----------------------------------------------------------
TASKS TO IMPLEMENT
----------------------------------------------------------

1️⃣ Create abstract class Subscription

Properties
- int UserId
- string PlanName
- double MonthlyFee

Method
- abstract string GetBenefits()

RULES
Each plan should return a string describing its benefits.

*/
public abstract class Subscription
{
    public int UserId { get; set; }
    public string PlanName { get; set; }
    public double MonthlyFee { get; set; }

    public abstract string GetBenefits();
}
/*
----------------------------------------------------------

2️⃣ Create derived classes

BasicPlan
PremiumPlan
FamilyPlan

Override GetBenefits()

Example benefits

BasicPlan
- HD streaming
- 1 device

PremiumPlan
- 4K streaming
- 4 devices

FamilyPlan
- 4K streaming
- 6 devices
- parental controls

*/
public class BasicPlan : Subscription
{
    public override string GetBenefits()
    {
        return "Streaming : HD\nDevices : 1";
    }
}

public class PremiumPlan : Subscription
{
    public override string GetBenefits()
    {
        return "Streaming : 4K\nDevices : 4";
    }
}

public class FamilyPlan : Subscription
{
    public override string GetBenefits()
    {
        return "Devices : 6\nDevices : 6\n* Parental Controls";
    }
}

public delegate double PricingStrategy(Subscription s);
/*
----------------------------------------------------------

3️⃣ Create delegate

public delegate double PricingStrategy(Subscription s);

This delegate represents dynamic pricing logic.


----------------------------------------------------------

4️⃣ Create class SubscriptionManager

Fields
- private List<Subscription> subscriptions

Methods to implement

AddSubscription(Subscription s)

GetSubscriptions()

ApplyPricingStrategy(Subscription s, PricingStrategy strategy)

RULES
- Use the delegate to compute final monthly price
- Print result

Example output
"User 101 final price: 19.99"


FilterSubscribers(Func<Subscription,bool> filter)

RULES
- Use LINQ Where
- Return filtered list

*/
public class SubscriptionManager
{
    private List<Subscription> _subscriptions = new List<Subscription>();

    public void AddSubscription(Subscription subscription)
    {
        _subscriptions.Add(subscription);
    }

    public List<Subscription> GetSubscriptions()
    {
        return _subscriptions;
    }

    public void ApplyPricingStrategy(Subscription subscription, PricingStrategy strategy)
    {
        var price = strategy(subscription);
        Console.WriteLine($"User : {subscription.UserId}\t Final Price : {price}");
    }

    public List<Subscription> FilterSubscribers(Func<Subscription, bool> filter)
    {
        return _subscriptions.Where(filter).ToList();
    }

    public List<(string Plan, double TotalRevenue)> RevenuePerPlan()
    {
        return _subscriptions
            .GroupBy(s => s.PlanName)
            .Select(g => (g.Key, g.Sum(p => p.MonthlyFee)))
            .ToList();
    }

    public string MostPopular()
    {
        return _subscriptions
            .GroupBy(p => p.PlanName)
            .OrderByDescending(p => p.Count())
            .First()
            .Key;
    }

    public List<Subscription> AboveThreshold(double threshold)
    {
        return _subscriptions.Where(s => s.MonthlyFee > threshold).ToList();
    }
}
/*
----------------------------------------------------------

5️⃣ LINQ ANALYTICS REQUIREMENTS

Implement the following reports

A) Revenue per plan

Rules
Group subscriptions by PlanName
Sum MonthlyFee


B) Most popular plan

Rules
GroupBy PlanName
Select plan with highest count


C) Users paying above threshold

Rules
MonthlyFee > given threshold


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
        SubscriptionManager manager = new SubscriptionManager();

        //--------------------------------------------------
        // CREATE SUBSCRIPTIONS
        //--------------------------------------------------

        Subscription s1 = new BasicPlan
        {
            UserId = 101,
            PlanName = "Basic",
            MonthlyFee = 8
        };

        Subscription s2 = new PremiumPlan
        {
            UserId = 102,
            PlanName = "Premium",
            MonthlyFee = 15
        };

        Subscription s3 = new FamilyPlan
        {
            UserId = 103,
            PlanName = "Family",
            MonthlyFee = 20
        };

        Subscription s4 = new PremiumPlan
        {
            UserId = 104,
            PlanName = "Premium",
            MonthlyFee = 15
        };

        Subscription s5 = new BasicPlan
        {
            UserId = 105,
            PlanName = "Basic",
            MonthlyFee = 8
        };

        manager.AddSubscription(s1);
        manager.AddSubscription(s2);
        manager.AddSubscription(s3);
        manager.AddSubscription(s4);
        manager.AddSubscription(s5);

        //--------------------------------------------------
        // PRICING STRATEGIES USING DELEGATES
        //--------------------------------------------------

        PricingStrategy festivalDiscount =
            s => s.MonthlyFee * 0.9;

        PricingStrategy loyaltyDiscount =
            s => s.MonthlyFee * 0.85;

        Console.WriteLine("\nFINAL PRICES WITH FESTIVAL DISCOUNT\n");

        foreach (var sub in manager.GetSubscriptions())
        {
            manager.ApplyPricingStrategy(sub, festivalDiscount);
        }

        //--------------------------------------------------
        // FILTER SUBSCRIBERS
        //--------------------------------------------------

        Console.WriteLine("\nSUBSCRIBERS PAYING ABOVE 10\n");

        var highPaying = manager.FilterSubscribers(s => s.MonthlyFee > 10);

        foreach (var s in highPaying)
        {
            Console.WriteLine($"User {s.UserId} | Plan: {s.PlanName} | Fee: {s.MonthlyFee}");
        }

        //--------------------------------------------------
        // REVENUE PER PLAN
        //--------------------------------------------------

        Console.WriteLine("\nREVENUE PER PLAN\n");

        var revenue = manager
            .GetSubscriptions()
            .GroupBy(s => s.PlanName)
            .Select(g => new
            {
                Plan = g.Key,
                Revenue = g.Sum(x => x.MonthlyFee)
            });

        foreach (var r in revenue)
        {
            Console.WriteLine($"{r.Plan} -> {r.Revenue}");
        }

        //--------------------------------------------------
        // MOST POPULAR PLAN
        //--------------------------------------------------

        Console.WriteLine("\nMOST POPULAR PLAN\n");

        var popularPlan = manager
            .GetSubscriptions()
            .GroupBy(s => s.PlanName)
            .OrderByDescending(g => g.Count())
            .First();

        Console.WriteLine($"{popularPlan.Key} ({popularPlan.Count()} users)");

        //--------------------------------------------------
        // USERS PAYING ABOVE THRESHOLD
        //--------------------------------------------------

        Console.WriteLine("\nUSERS PAYING ABOVE 12\n");

        var premiumUsers = manager
            .GetSubscriptions()
            .Where(s => s.MonthlyFee > 12);

        foreach (var u in premiumUsers)
        {
            Console.WriteLine($"User {u.UserId} - {u.PlanName} - {u.MonthlyFee}");
        }
    }
}