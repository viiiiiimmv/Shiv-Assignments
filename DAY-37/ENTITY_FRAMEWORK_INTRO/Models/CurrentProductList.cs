using System;
using System.Collections.Generic;

namespace ENTITY_FRAMEWORK_INTRO.Models;

public partial class CurrentProductList
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;
}
