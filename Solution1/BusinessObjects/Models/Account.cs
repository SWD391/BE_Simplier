using System;
using System.Collections.Generic;
using static BusinessObjects.Enums.Status;

namespace BusinessObjects.Models;

public partial class Account
{
    public string AccountId { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Password { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime? Birthdate { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public AccountRole Role { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<AssignedDetail> AssignedDetails { get; set; } = new List<AssignedDetail>();

    public virtual ICollection<FixTask> FixTasks { get; set; } = new List<FixTask>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
