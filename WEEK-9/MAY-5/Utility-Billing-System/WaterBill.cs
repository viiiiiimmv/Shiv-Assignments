using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility_Billing_System
{
    class WaterBill : UtilityBill
    {
        public override decimal CalculateBillAmount()
        {
            return UnitsConsumed * RatePerUnit;
        }

        public override decimal CalculateTax()
        {
            return CalculateBillAmount() * 0.02m;
        }
    }
}
