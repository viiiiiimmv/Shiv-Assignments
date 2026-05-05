using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility_Billing_System
{
    class GasBill : UtilityBill
    {
        public override decimal CalculateBillAmount()
        {
            return( UnitsConsumed * RatePerUnit) + 150;
        }

        public override decimal CalculateTax()
        {
            return 0;
        }
    }
}
