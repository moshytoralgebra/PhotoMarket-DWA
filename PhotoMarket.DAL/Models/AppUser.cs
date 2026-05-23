using System;
using System.Collections.Generic;

namespace PhotoMarket.DAL.Models;

public partial class AppUser
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string PasswordHash { get; set; } = null!;

    public int RoleId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Download> Downloads { get; set; } = new List<Download>();

    public virtual Role Role { get; set; } = null!;
}
