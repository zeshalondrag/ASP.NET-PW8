using System;
using System.Collections.Generic;

namespace SORAPC_API.Models;

public partial class User
{
    public int? IdUsers { get; set; }

    public string UserSurname { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string UserMiddlename { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Logins { get; set; } = null!;

    public string Passwords { get; set; } = null!;

    public int? RoleId { get; set; }
}