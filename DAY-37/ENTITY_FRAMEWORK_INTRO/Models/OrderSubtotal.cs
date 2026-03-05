using System;
using System.Collections.Generic;

namespace ENTITY_FRAMEWORK_INTRO.Models;

public partial class OrderSubtotal
{
    public int OrderId { get; set; }

    public decimal? Subtotal { get; set; }
}
