using System;
using System.Collections.Generic;

namespace SORAPC_API.Models;

public partial class Review
{
    public int? IdReview { get; set; }

    public int? ProductId { get; set; }

    public int? UsersId { get; set; }

    public int Rating { get; set; }

    public string Pros { get; set; } = null!;

    public string Cons { get; set; } = null!;

    public string Comment { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
}