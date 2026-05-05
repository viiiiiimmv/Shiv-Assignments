using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility_Billing_System
{
    class ElectricityBill : UtilityBill
    {
        public ElectricityBill(decimal rpu)
        {
            RatePerUnit = rpu;
        }
        public override decimal CalculateBillAmount()
        {
            decimal amount = RatePerUnit * UnitsConsumed;

            if (UnitsConsumed > 300) amount *= 1.1m;

            return amount;
        }

    }
}
