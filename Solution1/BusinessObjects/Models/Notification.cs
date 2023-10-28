using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Notification
{
    public string NotificationId { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Message { get; set; } = null!;

    public string AccountId { get; set; } = null!;

    public virtual Account Account { get; set; } = null!;
}
