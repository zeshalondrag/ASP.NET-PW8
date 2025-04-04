using System;
using System.Collections.Generic;

namespace SORAPC_API.Models;

public partial class Product
{
    public int? IdProduct { get; set; }

    public string Names { get; set; } = null!;

    public decimal Price { get; set; }

    public string Descriptions { get; set; } = null!;

    public int Quantity { get; set; }

    public int? ProductStatusId { get; set; }

    public int? ProductCategoryId { get; set; }

    public string Img { get; set; } = null!;

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}