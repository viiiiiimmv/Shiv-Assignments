
using Utility_Billing_System;

internal class Program
{
    private static void Main(string[] args)
    {
        List<UtilityBill> list = new List<UtilityBill>();
        list.Add(new ElectricityBill(5)
        {
            ConsumerId = 1,
            ConsumerName = "Johnson",
            UnitsConsumed = 350
        });

        list.Add(new ElectricityBill(5)
        {
            ConsumerId = 2,
            ConsumerName = "Smith",
            UnitsConsumed = 200
        });

        list.Add(new WaterBill()
        {
            ConsumerId = 3,
            ConsumerName = "David",
            UnitsConsumed = 100,
            RatePerUnit = 2
        });

        list.Add(new GasBill()
        {
            ConsumerId = 4,
            ConsumerName = "Julie",
            UnitsConsumed = 50,
            RatePerUnit = 3
        });


        foreach (var bill in list)
        {
            bill.PrintBill();
            Console.WriteLine(new string('-', 30));
        }
    }
}