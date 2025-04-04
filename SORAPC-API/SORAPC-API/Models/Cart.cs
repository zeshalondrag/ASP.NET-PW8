using System;
using System.Collections.Generic;

namespace SORAPC_API.Models;

public partial class Cart
{
    public int? IdCart { get; set; }

    public int? UsersId { get; set; }

    public int? ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public virtual Product? Product { get; set; }
    public virtual User? Users { get; set; }
}