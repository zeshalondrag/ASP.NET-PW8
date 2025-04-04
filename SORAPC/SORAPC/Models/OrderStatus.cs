using System;
using System.Collections.Generic;

namespace SORAPC_API.Models;

public partial class OrderStatus
{
    public int? IdOrderStatus { get; set; }

    public string Title { get; set; } = null!;
}