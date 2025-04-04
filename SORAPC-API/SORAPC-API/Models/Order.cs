using System;
using System.Collections.Generic;

namespace SORAPC_API.Models;

public partial class Order
{
    public int? IdOrder { get; set; }

    public int? UsersId { get; set; }

    public string OrderNumber { get; set; } = null!;

    public DateTime? OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public int? OrderStatusId { get; set; }
}