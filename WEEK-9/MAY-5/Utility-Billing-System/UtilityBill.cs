using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility_Billing_System
{
    abstract class UtilityBill
    {
        // Members variables

        public int ConsumerId { get; set; }
        public string? ConsumerName { get; set; }
        public decimal UnitsConsumed { get; set; }
        public decimal RatePerUnit { get; set; }

        public abstract decimal CalculateBillAmount();

        public virtual decimal CalculateTax()
        {
            return CalculateBillAmount() * 0.5m;
        }

        public void PrintBill()
        {
            Console.WriteLine($"Consumer ID: {ConsumerId}");
            Console.WriteLine($"Consumer Name: {ConsumerName}");
            Console.WriteLine($"Units Consumed: {UnitsConsumed}");
            Console.WriteLine($"Rate Per Unit: {RatePerUnit:C}");
            Console.WriteLine($"Bill Amount: {CalculateBillAmount():C}");
            Console.WriteLine($"Tax: {CalculateTax():C}");
        }
    }
}
