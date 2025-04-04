using System;
using System.Collections.Generic;

namespace SORAPC_API.Models;

public partial class OrderPosition
{
    public int? IdOrderPosition { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int Quantity { get; set; }
}