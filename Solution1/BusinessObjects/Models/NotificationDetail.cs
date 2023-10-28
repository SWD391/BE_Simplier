using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class NotificationDetail
{
    public string NotificationId { get; set; } = null!;

    public string ToUserId { get; set; } = null!;

    public virtual Account ToUser { get; set; } = null!;
}
