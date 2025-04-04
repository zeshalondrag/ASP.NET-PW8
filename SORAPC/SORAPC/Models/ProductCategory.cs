using System;
using System.Collections.Generic;

namespace SORAPC_API.Models;

public partial class ProductCategory
{
    public int? IdProductCategory { get; set; }

    public string Title { get; set; } = null!;
}