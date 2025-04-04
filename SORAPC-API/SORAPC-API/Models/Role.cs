using System;
using System.Collections.Generic;

namespace SORAPC_API.Models;

public partial class Role
{
    public int? IdRole { get; set; }

    public string Title { get; set; } = null!;
}